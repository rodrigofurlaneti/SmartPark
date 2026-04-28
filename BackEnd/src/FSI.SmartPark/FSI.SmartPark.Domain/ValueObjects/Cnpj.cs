using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.ValueObjects
{
    public record Cnpj(string Numero)
    {
        public string Numero { get; } = Validar(Numero);

        private static string Validar(string numero)
        {
            var limpo = numero?.Replace(".", "").Replace("/", "").Replace("-", "");
            if (limpo?.Length != 14) throw new ArgumentException("CNPJ deve conter 14 dígitos.");
            return limpo;
        }
    }
}
