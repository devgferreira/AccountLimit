namespace AccountLimit.Application.DTO.LimitManagement
{
    public class LimitManagementDTO
    {
        public string Cpf { get; set; }
        public string Agency { get; set; }
        public string Account { get; set; }
        public decimal PixTransactionLimit { get; set; }
    }
}
