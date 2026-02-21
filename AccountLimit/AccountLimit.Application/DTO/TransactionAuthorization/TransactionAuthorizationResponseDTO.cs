namespace AccountLimit.Application.DTO.TransactionAuthorization
{
    public class TransactionAuthorizationResponseDTO
    {
        public bool IsAuthorized { get; set; }
        public decimal LimitActual { get; set; }
    }
}
