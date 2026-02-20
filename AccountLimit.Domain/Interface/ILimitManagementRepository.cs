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
        Task UpdateLimitManagement(Guid id, LimitManagementInfo request);
        Task DeleteLimitManagement(Guid id);
        Task<List<LimitManagementInfo>> SelectLimitManagement(LimitManagementRequest request);
    }
}
