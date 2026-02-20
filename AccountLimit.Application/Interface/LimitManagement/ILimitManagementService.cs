using AccountLimit.Application.DTO.LimitManagement;
using AccountLimit.Domain.Entities.LimitManagement.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Application.Interface.LimitManagement
{
    public interface ILimitManagementService
    {
        Task CreateLimitManagement(LimitManagementCreateDTO request);
        Task UpdateLimitManagement(Guid id, LimitManagementUpdateDTO request);
        Task DeleteLimitManagement(Guid id);
        Task<List<LimitManagementDTO>> SelectLimitManagement(LimitManagementRequest request);
    }
}
