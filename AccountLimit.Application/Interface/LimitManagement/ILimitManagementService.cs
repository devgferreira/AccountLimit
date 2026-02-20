using AccountLimit.Application.DTO.LimitManagement;
using AccountLimit.Domain.Commom;
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
        Task<Result> CreateLimitManagement(LimitManagementCreateDTO request);
        Task<Result> UpdateLimitManagement(string cpf, LimitManagementUpdateDTO request);
        Task<Result> DeleteLimitManagement(string cpf);
        Task<Result> SelectLimitManagement(LimitManagementRequest request);
    }
}
