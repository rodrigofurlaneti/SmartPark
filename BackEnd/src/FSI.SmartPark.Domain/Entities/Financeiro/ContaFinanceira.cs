using FSI.SmartPark.Domain.ValueObjects;

namespace FSI.SmartPark.Domain.Entities.Financeiro
{
    public class ContaFinanceira : EntityBase
    {
        public string Descricao { get; private set; }
        public string Agencia { get; private set; }
        public string Conta { get; private set; }
        public bool ContaPadrao { get; private set; }
        public int? Banco_Id { get; private set; }
        public int Empresa_Id { get; private set; }

        // [A17] Campos obrigatórios para geração de boleto bancário (CNAB 240/400)
        public string? DigitoAgencia { get; private set; }      // Dígito verificador da agência
        public string? DigitoConta { get; private set; }        // Dígito verificador da conta
        public string? Carteira { get; private set; }           // Carteira bancária (ex: "109", "17")
        public string? Convenio { get; private set; }           // Código de convênio no banco
        public string? CodigoTransmissao { get; private set; }  // Código para remessa CNAB
        public string? ConvenioPagamento { get; private set; }  // Convênio para pagamentos (CNAB saída)
        public Cpf? CPF { get; private set; }                   // CPF do titular (conta PF)
        public Cnpj? CNPJ { get; private set; }                 // CNPJ do titular (conta PJ)

        public ContaFinanceira(string descricao, string agencia, string conta, int empresaId)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição da conta financeira é obrigatória.");
            if (string.IsNullOrWhiteSpace(agencia))
                throw new ArgumentException("Agência é obrigatória.");
            if (string.IsNullOrWhiteSpace(conta))
                throw new ArgumentException("Número da conta é obrigatório.");

            Descricao = descricao;
            Agencia = agencia;
            Conta = conta;
            Empresa_Id = empresaId;
        }

        public void ConfigurarBoleto(string carteira, string convenio, string codigoTransmissao)
        {
            if (string.IsNullOrWhiteSpace(carteira))
                throw new ArgumentException("Carteira é obrigatória para configurar boleto.");

            Carteira = carteira;
            Convenio = convenio;
            CodigoTransmissao = codigoTransmissao;
        }

        public void DefinirTitularPF(Cpf cpf)   => CPF = cpf;
        public void DefinirTitularPJ(Cnpj cnpj) => CNPJ = cnpj;
        public void DefinirComoPardrão()         => ContaPadrao = true;

        public bool PodeGerarBoleto() =>
            Banco_Id.HasValue && !string.IsNullOrWhiteSpace(Carteira) && !string.IsNullOrWhiteSpace(Convenio);
    }
}
