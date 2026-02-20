using AccountLimit.Domain.Entities.LimitManagement;
using AccountLimit.Domain.Entities.LimitManagement.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Domain.Interface
{
    public interface ILimitManagementRepository
    {
        Task CreateLimitManagement(LimitManagementInfo request);
        Task UpdateLimitManagement(LimitManagementInfo request);
        Task DeleteLimitManagement(string cpf);
        Task<List<LimitManagementInfo>> SelectLimitManagement(LimitManagementRequest request);
    }
}
