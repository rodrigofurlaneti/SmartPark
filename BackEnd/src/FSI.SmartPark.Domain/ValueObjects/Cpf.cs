using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.ValueObjects
{
    public record Cpf(string Numero)
    {
        public string Numero { get; } = Validar(Numero);

        private static string Validar(string numero)
        {
            var limpo = numero?.Replace(".", "").Replace("-", "");
            if (limpo?.Length != 11) throw new ArgumentException("CPF deve conter 11 dígitos.");
            return limpo;
        }
    }
}
