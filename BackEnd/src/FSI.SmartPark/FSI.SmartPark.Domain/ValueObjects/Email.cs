using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.ValueObjects
{
    public record Email(string Endereco)
    {
        public string Endereco { get; } = Validar(Endereco);

        private static string Validar(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco) || !endereco.Contains("@"))
                throw new ArgumentException("E-mail em formato inválido.");
            return endereco.ToLower();
        }
    }
}
