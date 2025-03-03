namespace Questao5.Domain.Movements
{
    public class Movement
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string MovementDate { get; set; }
        public char MovementType { get; set; }
        public decimal Amount { get; set; }
    }
}
