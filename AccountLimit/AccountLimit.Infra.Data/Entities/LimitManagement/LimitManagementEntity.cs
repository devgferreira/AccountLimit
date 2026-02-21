using Amazon.DynamoDBv2.DataModel;

namespace AccountLimit.Infra.Data.Entities.LimitManagement
{
    [DynamoDBTable("gestor-de-limites")]
    public class LimitManagementEntity
    {
        [DynamoDBHashKey("pk")]
        public string Cpf { get; set; }
        [DynamoDBRangeKey("sk")]
        public string Agency { get; set; }
        [DynamoDBProperty]
        public string Account { get; set; }

        [DynamoDBProperty]
        public decimal PixTransactionLimit { get; set; }
    }
}
