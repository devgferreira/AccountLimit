using AccountLimit.Application.DTO.LimitManagement;
using AccountLimit.Domain.Commom;
using AccountLimit.Domain.Entities.LimitManagement.Request;

namespace AccountLimit.Application.Interface.LimitManagement
{
    public interface ILimitManagementService
    {
        Task<Result> CreateLimitManagement(LimitManagementCreateDTO request);
        Task<Result> UpdateLimitManagement(string cpf, string agency, LimitManagementUpdateDTO request);
        Task<Result> DeleteLimitManagement(string cpf, string agency);
        Task<Result> SelectLimitManagement(LimitManagementRequest request);
    }
}
