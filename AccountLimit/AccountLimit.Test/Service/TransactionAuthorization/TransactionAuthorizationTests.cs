using AccountLimit.Application.DTO.TransactionAuthorization;
using AccountLimit.Application.Service.TransactionAuthorization;
using AccountLimit.Domain.Commom;
using AccountLimit.Domain.Entities.LimitManagement;
using AccountLimit.Domain.Entities.LimitManagement.Request;
using AccountLimit.Domain.Interface;
using Moq;
using System.Net;


namespace AccountLimit.Test.Service.TransactionAuthorization
{
    public class TransactionAuthorizationServiceTests
    {
        private readonly Mock<ILimitManagementRepository> _repoMock;
        private readonly TransactionAuthorizationService _service;

        public TransactionAuthorizationServiceTests()
        {
            _repoMock = new Mock<ILimitManagementRepository>(MockBehavior.Strict);
            _service = new TransactionAuthorizationService(_repoMock.Object);
        }

        #region AuthorizePixTransaction - Validation (Payer)

        [Fact]
        public async Task AuthorizePixTransaction_WhenPayerCpfInvalid_ShouldReturnBadRequest_AndNotCallRepository()
        {
            // Arrange
            var request = ValidRequest();
            request.PayerCpf = "123";

            // Act
            var result = await _service.AuthorizePixTransaction(request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Code.ToString());
            Assert.Contains("Payer:", result.Error);

            _repoMock.Verify(r => r.SelectLimitManagement(It.IsAny<LimitManagementRequest>()), Times.Never);
            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
        }

        [Fact]
        public async Task AuthorizePixTransaction_WhenPayerAgencyInvalid_ShouldReturnBadRequest_AndNotCallRepository()
        {
            // Arrange
            var request = ValidRequest();
            request.PayerAgency = "1";

            // Act
            var result = await _service.AuthorizePixTransaction(request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Code.ToString());
            Assert.Contains("Payer:", result.Error);

            _repoMock.Verify(r => r.SelectLimitManagement(It.IsAny<LimitManagementRequest>()), Times.Never);
            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
        }

        [Fact]
        public async Task AuthorizePixTransaction_WhenPayerAccountInvalid_ShouldReturnBadRequest_AndNotCallRepository()
        {
            // Arrange
            var request = ValidRequest();
            request.PayerAccount = "";

            // Act
            var result = await _service.AuthorizePixTransaction(request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Code.ToString());
            Assert.Contains("Payer:", result.Error);

            _repoMock.Verify(r => r.SelectLimitManagement(It.IsAny<LimitManagementRequest>()), Times.Never);
            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
        }

        #endregion

        #region AuthorizePixTransaction - Validation (Receiver)

        [Fact]
        public async Task AuthorizePixTransaction_WhenReceiverCpfInvalid_ShouldReturnBadRequest_AndNotCallRepository()
        {
            // Arrange
            var request = ValidRequest();
            request.ReceiverCpf = "123";

            // Act
            var result = await _service.AuthorizePixTransaction(request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Code.ToString());
            Assert.Contains("Receiver:", result.Error);

            _repoMock.Verify(r => r.SelectLimitManagement(It.IsAny<LimitManagementRequest>()), Times.Never);
            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
        }

        [Fact]
        public async Task AuthorizePixTransaction_WhenReceiverAgencyInvalid_ShouldReturnBadRequest_AndNotCallRepository()
        {
            // Arrange
            var request = ValidRequest();
            request.ReceiverAgency = "1";

            // Act
            var result = await _service.AuthorizePixTransaction(request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Code.ToString());
            Assert.Contains("Receiver:", result.Error);

            _repoMock.Verify(r => r.SelectLimitManagement(It.IsAny<LimitManagementRequest>()), Times.Never);
            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
        }

        [Fact]
        public async Task AuthorizePixTransaction_WhenReceiverAccountInvalid_ShouldReturnBadRequest_AndNotCallRepository()
        {
            // Arrange
            var request = ValidRequest();
            request.ReceiverAccount = "";

            // Act
            var result = await _service.AuthorizePixTransaction(request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Code.ToString());
            Assert.Contains("Receiver:", result.Error);

            _repoMock.Verify(r => r.SelectLimitManagement(It.IsAny<LimitManagementRequest>()), Times.Never);
            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
        }

        #endregion

        #region AuthorizePixTransaction - Payer not found

        [Fact]
        public async Task AuthorizePixTransaction_WhenPayerNotFound_ShouldReturnNotFound_AndNotUpdate()
        {
            // Arrange
            var request = ValidRequest();

            _repoMock
                .Setup(r => r.SelectLimitManagement(It.Is<LimitManagementRequest>(x =>
                    x.Cpf == request.PayerCpf &&
                    x.Agency == request.PayerAgency)))
                .ReturnsAsync(new List<LimitManagementInfo>());

            // Act
            var result = await _service.AuthorizePixTransaction(request);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.NotFound.ToString(), result.Code.ToString());
            Assert.Contains("Account Payer not found", result.Error, StringComparison.OrdinalIgnoreCase);

            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
            _repoMock.VerifyAll();
        }

        #endregion

        #region AuthorizePixTransaction - Domain authorization (fail/success)

        [Fact]
        public async Task AuthorizePixTransaction_WhenDomainAuthorizationFails_ShouldReturnFailure_WithIsAuthorizedFalse_AndNotUpdate()
        {
            // Arrange
            var request = ValidRequest();
            request.Amount = 999999;

            var payer = CreateLimitManagementWithLimit(
                cpf: request.PayerCpf,
                agency: request.PayerAgency,
                account: request.PayerAccount,
                pixLimit: 100
            );

            _repoMock
                .Setup(r => r.SelectLimitManagement(It.Is<LimitManagementRequest>(x =>
                    x.Cpf == request.PayerCpf && x.Agency == request.PayerAgency)))
                .ReturnsAsync(new List<LimitManagementInfo> { payer });

            // Act
            var result = await _service.AuthorizePixTransaction(request);

            // Assert
            Assert.True(result.IsFailure);

            var response = Assert.IsType<Result<TransactionAuthorizationResponseDTO>>(result);
            Assert.False(response.Value.IsAuthorized);
            Assert.Equal(payer.PixTransactionLimit.Value, response.Value.LimitActual);

            _repoMock.Verify(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()), Times.Never);
            _repoMock.VerifyAll();
        }

        [Fact]
        public async Task AuthorizePixTransaction_WhenAuthorized_ShouldUpdateLimitManagement_AndReturnSuccess()
        {
            // Arrange
            var request = ValidRequest();
            request.Amount = 10;

            var payer = CreateLimitManagementWithLimit(
                cpf: request.PayerCpf,
                agency: request.PayerAgency,
                account: request.PayerAccount,
                pixLimit: 100
            );

            _repoMock
                .Setup(r => r.SelectLimitManagement(It.Is<LimitManagementRequest>(x =>
                    x.Cpf == request.PayerCpf && x.Agency == request.PayerAgency)))
                .ReturnsAsync(new List<LimitManagementInfo> { payer });

            _repoMock
                .Setup(r => r.UpdateLimitManagement(It.IsAny<LimitManagementInfo>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AuthorizePixTransaction(request);

            // Assert
            Assert.True(result.IsSuccess);

            var response = Assert.IsType<Result<TransactionAuthorizationResponseDTO>>(result);
            Assert.True(response.Value.IsAuthorized);

            Assert.True(response.Value.LimitActual >= 0);

            _repoMock.Verify(r => r.UpdateLimitManagement(It.Is<LimitManagementInfo>(lm =>
                lm.Cpf.ToString() == request.PayerCpf &&
                lm.Agency.ToString() == request.PayerAgency
            )), Times.Once);

            _repoMock.VerifyAll();
        }

        #endregion

        #region Helpers

        private static TransactionAuthorizationDTO ValidRequest()
            => new TransactionAuthorizationDTO
            {
                PayerCpf = ValidCpf(),
                PayerAgency = ValidAgency(),
                PayerAccount = ValidAccount(),

                ReceiverCpf = "98765432100",
                ReceiverAgency = "0002",
                ReceiverAccount = "654321",

                Amount = 10
            };

        private static string ValidCpf() => "12345678909";
        private static string ValidAgency() => "0001";
        private static string ValidAccount() => "123456";

        private static LimitManagementInfo CreateLimitManagementWithLimit(string cpf, string agency, string account, decimal pixLimit)
        {
            var created = LimitManagementInfo.Create(cpf, agency, account, pixLimit);
            if (created.IsFailure)
                throw new InvalidOperationException($"Falha criando LimitManagementInfo válido para teste: {created.Error}");

            return created.Value;
        }

        #endregion
    }
}