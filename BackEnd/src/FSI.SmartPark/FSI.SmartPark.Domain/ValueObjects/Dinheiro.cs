using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.ValueObjects
{
    public record Dinheiro(decimal Valor)
    {
        public decimal Valor { get; } = Valor >= 0 ? Valor : throw new ArgumentException("Valor não pode ser negativo.");
        public static Dinheiro operator +(Dinheiro a, Dinheiro b) => new(a.Valor + b.Valor);
    }
}
