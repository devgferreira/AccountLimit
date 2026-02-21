using AccountLimit.Application.DTO.LimitManagement;
using AccountLimit.Application.Service.LimitManagement;
using AccountLimit.Domain.Commom;
using AccountLimit.Domain.Entities.LimitManagement;
using AccountLimit.Domain.Entities.LimitManagement.Request;
using AccountLimit.Domain.Interface;
using AccountLimit.Infra.Data.Repository.LimitManagementRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;


namespace AccountLimit.Test.Service.LimitManagement
{
    public class LimitManagementServiceTests
    {
        private readonly Mock<ILimitManagementRepository> _repoMock;
        private readonly LimitManagementService _service;

        public LimitManagementServiceTests()
        {
            _repoMock = new Mock<ILimitManagementRepository>();
            _service = new LimitManagementService(_repoMock.Object);
        }

        #region CreateLimitManagement

        [Fact]
        public async Task CreateLimitManagement_WhenDomainCreateFails_ShouldReturnBadRequest_AndNotCallRepository()
        {
            // Arrange
            var request = new LimitManagementCreateDTO
            {
                Cpf = "123",              
                Agency = "1",            
                Account = "0",           
                PixTransactionLimit = -1  
            };

            // Act
            var result = await _service.CreateLimitManagement(request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Code.ToString());

            _repoMock.Verify(r => r.SelectLimitManagement(It.IsAny<LimitManagementRequest>()), Times.Never);
            _repoMock.Verify(r => r.CreateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
        }

        [Fact]
        public async Task CreateLimitManagement_WhenAlreadyExists_ShouldReturnFailureOk_AndNotCreate()
        {
            // Arrange
            var request = new LimitManagementCreateDTO
            {
                Cpf = ValidCpf(),
                Agency = ValidAgency(),
                Account = ValidAccount(),
                PixTransactionLimit = 100
            };

            var existing = CreateValidLimitManagementInfo(request.Cpf, request.Agency, request.Account, request.PixTransactionLimit);

            _repoMock
                .Setup(r => r.SelectLimitManagement(It.Is<LimitManagementRequest>(x =>
                    x.Cpf == request.Cpf && x.Agency == request.Agency)))
                .ReturnsAsync(new List<LimitManagementInfo> { existing });

            // Act
            var result = await _service.CreateLimitManagement(request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.OK.ToString(), result.Code.ToString());
            Assert.Contains("already exists", result.Error, StringComparison.OrdinalIgnoreCase);

            _repoMock.Verify(r => r.CreateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
            _repoMock.VerifyAll();
        }

        [Fact]
        public async Task CreateLimitManagement_WhenNotExists_ShouldCreateAndReturnSuccess()
        {
            // Arrange
            var request = new LimitManagementCreateDTO
            {
                Cpf = ValidCpf(),
                Agency = ValidAgency(),
                Account = ValidAccount(),
                PixTransactionLimit = 100
            };

            _repoMock
                .Setup(r => r.SelectLimitManagement(It.Is<LimitManagementRequest>(x =>
                    x.Cpf == request.Cpf && x.Agency == request.Agency)))
                .ReturnsAsync(new List<LimitManagementInfo>()); 

            _repoMock
                .Setup(r => r.CreateLimitManagement(It.IsAny<LimitManagementInfo>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateLimitManagement(request);

            // Assert
            Assert.True(result.IsSuccess);

            _repoMock.Verify(r => r.CreateLimitManagement(It.Is<LimitManagementInfo>(lm =>
                lm.Cpf.ToString() == request.Cpf &&
                lm.Agency.ToString() == request.Agency &&
                lm.Account.ToString() == request.Account &&
                lm.PixTransactionLimit.Value == request.PixTransactionLimit
            )), Times.Once);

            _repoMock.VerifyAll();
        }

        #endregion

        #region DeleteLimitManagement

        [Fact]
        public async Task DeleteLimitManagement_WhenCpfInvalid_ShouldReturnBadRequest_AndNotQueryRepository()
        {
            // Arrange
            var cpf = "123"; 
            var agency = ValidAgency();

            // Act
            var result = await _service.DeleteLimitManagement(cpf, agency);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Code.ToString());

            _repoMock.Verify(r => r.SelectLimitManagement(It.IsAny<LimitManagementRequest>()), Times.Never);
            _repoMock.Verify(r => r.DeleteLimitManagement(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteLimitManagement_WhenAgencyInvalid_ShouldReturnBadRequest_AndNotQueryRepository()
        {
            // Arrange
            var cpf = ValidCpf();
            var agency = "1"; 

            // Act
            var result = await _service.DeleteLimitManagement(cpf, agency);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Code.ToString());

            _repoMock.Verify(r => r.SelectLimitManagement(It.IsAny<LimitManagementRequest>()), Times.Never);
            _repoMock.Verify(r => r.DeleteLimitManagement(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteLimitManagement_WhenNotFound_ShouldReturnNotFound_AndNotDelete()
        {
            // Arrange
            var cpf = ValidCpf();
            var agency = ValidAgency();

            _repoMock
                .Setup(r => r.SelectLimitManagement(It.Is<LimitManagementRequest>(x =>
                    x.Cpf == cpf && x.Agency == agency)))
                .ReturnsAsync(new List<LimitManagementInfo>()); 

            // Act
            var result = await _service.DeleteLimitManagement(cpf, agency);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.NotFound.ToString(), result.Code.ToString());

            _repoMock.Verify(r => r.DeleteLimitManagement(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _repoMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteLimitManagement_WhenFound_ShouldDeleteAndReturnSuccess()
        {
            // Arrange
            var cpf = ValidCpf();
            var agency = ValidAgency();

            var existing = CreateValidLimitManagementInfo(cpf, agency, ValidAccount(), 100);

            _repoMock
                .Setup(r => r.SelectLimitManagement(It.Is<LimitManagementRequest>(x =>
                    x.Cpf == cpf && x.Agency == agency)))
                .ReturnsAsync(new List<LimitManagementInfo> { existing });

            _repoMock
                .Setup(r => r.DeleteLimitManagement(cpf, agency))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteLimitManagement(cpf, agency);

            // Assert
            Assert.True(result.IsSuccess);

            _repoMock.Verify(r => r.DeleteLimitManagement(cpf, agency), Times.Once);
            _repoMock.VerifyAll();
        }

        #endregion

        #region SelectLimitManagement

        [Fact]
        public async Task SelectLimitManagement_ShouldReturnMappedDtos()
        {
            // Arrange
            var request = new LimitManagementRequest { Cpf = ValidCpf(), Agency = ValidAgency() };

            var list = new List<LimitManagementInfo>
        {
            CreateValidLimitManagementInfo(request.Cpf, request.Agency, "123456", 100),
            CreateValidLimitManagementInfo(request.Cpf, request.Agency, "654321", 250),
        };

            _repoMock
                .Setup(r => r.SelectLimitManagement(request))
                .ReturnsAsync(list);

            // Act
            var result = await _service.SelectLimitManagement(request);

            // Assert
            Assert.True(result.IsSuccess);

            Result<List<LimitManagementDTO>> data = Assert.IsType<Result<List<LimitManagementDTO>>>(result);
            Assert.Equal(2, data.Value.Count);

            Assert.Equal(list[0].Cpf.ToString(), data.Value[0].Cpf);
            Assert.Equal(list[0].Agency.ToString(), data.Value[0].Agency);
            Assert.Equal(list[0].Account.ToString(), data.Value[0].Account);
            Assert.Equal(list[0].PixTransactionLimit.Value, data.Value[0].PixTransactionLimit);

            _repoMock.VerifyAll();
        }

        #endregion

        #region UpdateLimitManagement

        [Fact]
        public async Task UpdateLimitManagement_WhenCpfInvalid_ShouldReturnBadRequest_AndNotQueryRepository()
        {
            // Arrange
            var cpf = "123"; 
            var agency = ValidAgency();
            var request = new LimitManagementUpdateDTO { PixTransactionLimit = 200 };

            // Act
            var result = await _service.UpdateLimitManagement(cpf, agency, request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Code.ToString());

            _repoMock.Verify(r => r.SelectLimitManagement(It.IsAny<LimitManagementRequest>()), Times.Never);
            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
        }

        [Fact]
        public async Task UpdateLimitManagement_WhenAgencyInvalid_ShouldReturnBadRequest_AndNotQueryRepository()
        {
            // Arrange
            var cpf = ValidCpf();
            var agency = "1"; 
            var request = new LimitManagementUpdateDTO { PixTransactionLimit = 200 };

            // Act
            var result = await _service.UpdateLimitManagement(cpf, agency, request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Code.ToString());

            _repoMock.Verify(r => r.SelectLimitManagement(It.IsAny<LimitManagementRequest>()), Times.Never);
            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
        }

        [Fact]
        public async Task UpdateLimitManagement_WhenNotFound_ShouldReturnNotFound_AndNotUpdate()
        {
            // Arrange
            var cpf = ValidCpf();
            var agency = ValidAgency();
            var request = new LimitManagementUpdateDTO { PixTransactionLimit = 200 };

            _repoMock
                .Setup(r => r.SelectLimitManagement(It.Is<LimitManagementRequest>(x =>
                    x.Cpf == cpf && x.Agency == agency)))
                .ReturnsAsync(new List<LimitManagementInfo>());

            // Act
            var result = await _service.UpdateLimitManagement(cpf, agency, request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.NotFound.ToString(), result.Code.ToString());

            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
            _repoMock.VerifyAll();
        }

        [Fact]
        public async Task UpdateLimitManagement_WhenPixLimitInvalid_ShouldReturnFailure_AndNotUpdateRepository()
        {
            // Arrange
            var cpf = ValidCpf();
            var agency = ValidAgency();

            var existing = CreateValidLimitManagementInfo(cpf, agency, ValidAccount(), 100);

            _repoMock
                .Setup(r => r.SelectLimitManagement(It.Is<LimitManagementRequest>(x =>
                    x.Cpf == cpf && x.Agency == agency)))
                .ReturnsAsync(new List<LimitManagementInfo> { existing });

            var request = new LimitManagementUpdateDTO
            {
                PixTransactionLimit = -1 
            };

            // Act
            var result = await _service.UpdateLimitManagement(cpf, agency, request);

            // Assert
            Assert.True(result.IsFailure);

            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
            _repoMock.VerifyAll();
        }

        [Fact]
        public async Task UpdateLimitManagement_WhenValid_ShouldUpdateAndReturnSuccess()
        {
            // Arrange
            var cpf = ValidCpf();
            var agency = ValidAgency();

            var existing = CreateValidLimitManagementInfo(cpf, agency, ValidAccount(), 100);

            _repoMock
                .Setup(r => r.SelectLimitManagement(It.Is<LimitManagementRequest>(x =>
                    x.Cpf == cpf && x.Agency == agency)))
                .ReturnsAsync(new List<LimitManagementInfo> { existing });

            _repoMock
                .Setup(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()))
                .Returns(Task.CompletedTask);

            var request = new LimitManagementUpdateDTO { PixTransactionLimit = 999 };

            // Act
            var result = await _service.UpdateLimitManagement(cpf, agency, request);

            // Assert
            Assert.True(result.IsSuccess);

            _repoMock.Verify(r => r.UpdateLimitManagement(It.Is<LimitManagementInfo>(lm =>
                lm.Cpf.ToString() == cpf &&
                lm.Agency.ToString() == agency &&
                lm.PixTransactionLimit.Value == request.PixTransactionLimit
            )), Times.Once);

            _repoMock.VerifyAll();
        }

        #endregion

        #region Helpers 

        private static string ValidCpf() => "12345678909";     
        private static string ValidAgency() => "0001";          
        private static string ValidAccount() => "123456";      

        private static LimitManagementInfo CreateValidLimitManagementInfo(string cpf, string agency, string account, decimal pixLimit)
        {
            var result = LimitManagementInfo.Create(cpf, agency, account, pixLimit);
            if (result.IsFailure)
                throw new InvalidOperationException($"Não consegui criar LimitManagementInfo válido para teste: {result.Error}");

            return result.Value;
        }

        #endregion
    }
}
