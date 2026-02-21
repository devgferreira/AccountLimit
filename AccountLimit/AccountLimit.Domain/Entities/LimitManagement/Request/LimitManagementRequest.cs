namespace AccountLimit.Domain.Entities.LimitManagement.Request
{
    public class LimitManagementRequest
    {
        public string Cpf { get; set; }
        public string? Agency { get; set; }

    }
}
