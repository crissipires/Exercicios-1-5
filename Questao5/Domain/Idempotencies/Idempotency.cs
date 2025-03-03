namespace Questao5.Domain.Idempotencies
{
    public class Idempotency
    {
        public string IdempotencyKey { get; set; }
        public string Request { get; set; }
        public string Result { get; set; }
    }
}
