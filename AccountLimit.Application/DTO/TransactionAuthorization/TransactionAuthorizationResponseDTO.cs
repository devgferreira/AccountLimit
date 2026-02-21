using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Application.DTO.TransactionAuthorization
{
    public class TransactionAuthorizationResponseDTO
    {
        public bool IsAuthorized { get; set; }
        public decimal LimitActual { get; set; }
    }
}
