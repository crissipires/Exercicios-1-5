namespace Questao5.Domain.Results
{
    public class Error
    {
        public string Type { get; }
        public string Message { get; }

        public Error(string type, string message)
        {
            Type = type;
            Message = message;
        }

        public static readonly Error None = new Error(string.Empty, string.Empty);
    }

}
