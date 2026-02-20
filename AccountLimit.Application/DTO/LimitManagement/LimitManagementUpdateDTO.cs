using AccountLimit.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Application.DTO.LimitManagement
{
    public class LimitManagementUpdateDTO
    {
        public decimal PixTransactionLimit { get; set; }
    }
}
