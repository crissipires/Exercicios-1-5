using System.Globalization;

namespace Questao1
{
    class ContaBancaria 
    {
        public int NumeroConta { get; }
        public string Titular { get; set; }
        public double Saldo { get; private set; }

        private const double TaxaSaque = 3.50;
        public ContaBancaria(int numeroConta, string titular)
        {
            NumeroConta = numeroConta;
            Titular = titular;
            Saldo = 0.0;
        }

        public ContaBancaria(int numeroConta, string titular, double depositoInicial)
        {
            NumeroConta = numeroConta;
            Titular = titular;
            Saldo = depositoInicial;
        }

        public void Deposito(double quantia)
        {
            if (quantia > 0)
            {
                Saldo += quantia;
            }
        }

        public void Saque(double quantia)
        {
            if (quantia > 0)
            {
                Saldo -= quantia + TaxaSaque;
            }
        }

        public override string ToString()
        {
            return $"Conta {NumeroConta}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }

    }
}
