namespace Questao5.Domain.Results.ErrorsMsg
{
    public class ErrorMsg
    {
        public static readonly Error InvalidAccount = new("INVALID_ACCOUNT", "Account not found.");
        public static readonly Error InactiveAccount = new("INACTIVE_ACCOUNT", "Inactive account.");
        public static readonly Error InvalidAmount = new("INVALID_AMOUNT", "Invalid amount.");
        public static readonly Error InvalidMovementType = new("INVALID_TYPE", "Invalid movement type.");
    }
}
