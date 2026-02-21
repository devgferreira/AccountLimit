using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Application.DTO.TransactionAuthorization
{
    public class TransactionAuthorizationDTO
    {
        public Guid TransactionId { get; set; } = Guid.NewGuid(); // apenas para simular um Guid, já que não temos um banco de dados para gerar isso automaticamente.
        public string Cpf { get; set; }
        public string Agency { get; set; }
        public string Account { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
