namespace FSI.SmartPark.Domain.ValueObjects
{
    public record Placa(string Valor)
    {
        public string Valor { get; } = Validar(Valor);

        private static string Validar(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor) || valor.Length < 7 || valor.Length > 8)
                throw new ArgumentException("Placa em formato inválido.");
            return valor.ToUpper().Replace("-", "");
        }
    }
}
