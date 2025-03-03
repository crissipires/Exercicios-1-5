namespace Questao5.Application.Handlers.CurrentAccounts.DTOs
{
    public class BalanceResponseDTO
    {
        public int AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public DateTime InquiryDateTime { get; set; }
        public decimal Balance { get; set; }
    }
}
