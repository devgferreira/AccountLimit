using AccountLimit.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Application.DTO.LimitManagement
{
    public class LimitManagementCreateDTO
    {
        public string Cpf { get; set; }
        public string Agency { get; set; }
        public string Account { get; set; }
        public Period Period { get; set; }
        public decimal PixTransactionLimit { get; set; }
    }
}
