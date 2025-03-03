using System.ComponentModel;

namespace Questao5.Domain.Movements.Enumerators
{
    public class EnumMovementType
    {
        [Description("Credito")]
        public const char Credito = 'C';

        [Description("Débito")]
        public const char Debito = 'D';
    }
}
