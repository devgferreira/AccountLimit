using AccountLimit.Domain.Enums;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Infra.Data.Entities.LimitManagement
{
    [DynamoDBTable("gestor-de-limites")]
    public class LimitManagementEntity
    {
        [DynamoDBHashKey("pk")]
        public string Cpf { get; set; }
        [DynamoDBProperty]
        public string Agency { get; set; }
        [DynamoDBProperty]
        public string Account { get; set; }
        [DynamoDBProperty]
        public decimal PixTransactionLimit { get; set; }
    }
}
