using FSI.SmartPark.Domain.Enums;

namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class PedidoSelo : EntityBase
    {
        public int Quantidade { get; private set; }
        public StatusPedidoSelo StatusPedido { get; private set; }
        public int Cliente_Id { get; private set; }
        public int Unidade_Id { get; private set; }

        // [A09] Campos adicionados conforme modelagem MySQL
        public int TipoSelo_Id { get; private set; }               // Tipo do selo (determina valor e validade)
        public FormaPagamento TiposPagamento { get; private set; } // Forma de pagamento do pedido
        public int? Convenio_Id { get; private set; }              // Convênio vinculado (se houver desconto)
        public int? Desconto_Id { get; private set; }              // Desconto aplicado
        public int? EmissaoSelo_Id { get; private set; }           // Emissão física gerada após aprovação
        public int? Usuario_Id { get; private set; }               // Usuário que registrou o pedido
        public int DiasVencimento { get; private set; }            // Prazo em dias para pagamento
        public DateTime? DataVencimento { get; private set; }      // Data limite para pagamento
        public DateTime? ValidadePedido { get; private set; }      // Data de expiração do pedido não confirmado
        public TipoPedidoSelo TipoPedidoSelo { get; private set; } // Avulso, Contrato, Convênio

        public PedidoSelo(
            int quantidade,
            int clienteId,
            int unidadeId,
            int tipoSeloId,
            FormaPagamento formaPagamento,
            TipoPedidoSelo tipoPedido,
            int usuarioId,
            int diasVencimento = 5)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade mínima é 1 selo.");
            if (diasVencimento <= 0)
                throw new ArgumentException("Dias de vencimento deve ser maior que zero.");

            Quantidade = quantidade;
            Cliente_Id = clienteId;
            Unidade_Id = unidadeId;
            TipoSelo_Id = tipoSeloId;
            TiposPagamento = formaPagamento;
            TipoPedidoSelo = tipoPedido;
            Usuario_Id = usuarioId;
            DiasVencimento = diasVencimento;
            StatusPedido = StatusPedidoSelo.Pendente;
            DataVencimento = DateTime.Now.AddDays(diasVencimento);
            ValidadePedido = DateTime.Now.AddDays(diasVencimento);
        }

        public void VincularEmissao(int emissaoSeloId)
        {
            EmissaoSelo_Id = emissaoSeloId;
            StatusPedido = StatusPedidoSelo.Emitido;
        }

        public void VincularConvenio(int convenioId, int? descontoId = null)
        {
            Convenio_Id = convenioId;
            Desconto_Id = descontoId;
        }

        public void Cancelar()
        {
            if (StatusPedido == StatusPedidoSelo.Emitido)
                throw new InvalidOperationException("Pedido já emitido não pode ser cancelado.");
            StatusPedido = StatusPedidoSelo.Cancelado;
        }

        public bool EstaVencido() => DataVencimento.HasValue && DataVencimento < DateTime.Now;
    }
}
