using AccountLimit.Domain.Entities.LimitManagement;
using AccountLimit.Domain.Entities.LimitManagement.Request;

namespace AccountLimit.Domain.Interface
{
    public interface ILimitManagementRepository
    {
        Task CreateLimitManagement(LimitManagementInfo request);
        Task UpdateLimitManagement(LimitManagementInfo request);
        Task DeleteLimitManagement(string cpf, string agency);
        Task<List<LimitManagementInfo>> SelectLimitManagement(LimitManagementRequest request);
    }
}
