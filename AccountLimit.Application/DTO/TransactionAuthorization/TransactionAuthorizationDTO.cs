using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Application.DTO.TransactionAuthorization
{
    public class TransactionAuthorizationDTO
    {
        public Guid TransactionId { get; set; } = Guid.NewGuid(); // apenas para simular um Guid, já que não temos uma API que vai consumir essa aplicação;
        public string PayerCpf { get; set; }
        public string PayerAgency { get; set; }
        public string PayerAccount { get; set; }
        public string ReceiverCpf { get; set; }
        public string ReceiverAgency { get; set; }
        public string ReceiverAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
