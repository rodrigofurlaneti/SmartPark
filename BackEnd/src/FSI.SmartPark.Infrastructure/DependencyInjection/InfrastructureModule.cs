using FSI.SmartPark.Domain.Interfaces;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Infrastructure.Data;
using FSI.SmartPark.Infrastructure.Repositories.Comercial;
using FSI.SmartPark.Infrastructure.Repositories.Equipe;
using FSI.SmartPark.Infrastructure.Repositories.Financeiro;
using FSI.SmartPark.Infrastructure.Repositories.Identidade;
using FSI.SmartPark.Infrastructure.Repositories.Operacional;
using FSI.SmartPark.Infrastructure.Repositories.Suporte;
using Microsoft.Extensions.DependencyInjection;

namespace FSI.SmartPark.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<SmartParkDbContext>();

        // Operacional
        services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
        services.AddScoped<IFaturamentoRepository, FaturamentoRepository>();
        services.AddScoped<IUnidadeRepository, UnidadeRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        services.AddScoped<IMarcaRepository, MarcaRepository>();
        services.AddScoped<IModeloRepository, ModeloRepository>();
        services.AddScoped<IHorarioUnidadeRepository, HorarioUnidadeRepository>();
        services.AddScoped<ITabelaPrecoAvulsoRepository, TabelaPrecoAvulsoRepository>();
        services.AddScoped<ITabelaPrecoAvulsoHoraValorRepository, TabelaPrecoAvulsoHoraValorRepository>();
        services.AddScoped<IEstruturaUnidadeRepository, EstruturaUnidadeRepository>();
        services.AddScoped<IEquipamentoRepository, EquipamentoRepository>();
        services.AddScoped<IPeriodoHorarioRepository, PeriodoHorarioRepository>();

        // Comercial
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IClienteCondominoRepository, ClienteCondominoRepository>();
        services.AddScoped<IContratoMensalistaRepository, ContratoMensalistaRepository>();
        services.AddScoped<IConvenioRepository, ConvenioRepository>();
        services.AddScoped<IPedidoSeloRepository, PedidoSeloRepository>();
        services.AddScoped<ISeloRepository, SeloRepository>();
        services.AddScoped<ITipoSeloRepository, TipoSeloRepository>();
        services.AddScoped<IVagaCortesiaRepository, VagaCortesiaRepository>();
        services.AddScoped<IVagaCortesiaVigenciaRepository, VagaCortesiaVigenciaRepository>();
        services.AddScoped<IPropostaRepository, PropostaRepository>();

        // Financeiro
        services.AddScoped<IBancoRepository, BancoRepository>();
        services.AddScoped<IContaFinanceiraRepository, ContaFinanceiraRepository>();
        services.AddScoped<IContasAPagarRepository, ContasAPagarRepository>();
        services.AddScoped<IContasAPagarItemRepository, ContasAPagarItemRepository>();
        services.AddScoped<ILancamentoCobrancaRepository, LancamentoCobrancaRepository>();
        services.AddScoped<IAcordoRepository, AcordoRepository>();
        services.AddScoped<IAcordoParcelaRepository, AcordoParcelaRepository>();
        services.AddScoped<IContaCorrenteClienteRepository, ContaCorrenteClienteRepository>();
        services.AddScoped<IContaCorrenteClienteDetalheRepository, ContaCorrenteClienteDetalheRepository>();

        // Identidade
        services.AddScoped<IPessoaRepository, PessoaRepository>();
        services.AddScoped<IPessoaJuridicaRepository, PessoaJuridicaRepository>();
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IFilialRepository, FilialRepository>();
        services.AddScoped<ICidadeRepository, CidadeRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        services.AddScoped<IRegionalRepository, RegionalRepository>();
        services.AddScoped<IRegionalEstadoRepository, RegionalEstadoRepository>();

        // Equipe
        services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
        services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
        services.AddScoped<IEquipeRepository, EquipeRepository>();
        services.AddScoped<IControlePontoRepository, ControlePontoRepository>();
        services.AddScoped<IControleFeriasRepository, ControleFeriasRepository>();
        services.AddScoped<IItemFuncionarioRepository, ItemFuncionarioRepository>();
        services.AddScoped<IItemFuncionarioDetalheRepository, ItemFuncionarioDetalheRepository>();
        services.AddScoped<IBeneficioFuncionarioDetalheRepository, BeneficioFuncionarioDetalheRepository>();
        services.AddScoped<ITipoBeneficioRepository, TipoBeneficioRepository>();
        services.AddScoped<IFuncionarioIntervaloDozeTrintaSeisRepository, FuncionarioIntervaloDozeTrintaSeisRepository>();
        services.AddScoped<IFuncionarioIntervaloNoturnoRepository, FuncionarioIntervaloNoturnoRepository>();
        services.AddScoped<IFuncionarioIntervaloCompensacaoRepository, FuncionarioIntervaloCompensacaoRepository>();

        // Suporte
        services.AddScoped<IAuditRepository, AuditRepository>();
        services.AddScoped<IEstoqueRepository, EstoqueRepository>();
        services.AddScoped<IEstoqueMaterialRepository, EstoqueMaterialRepository>();
        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<INotificacaoRepository, NotificacaoRepository>();
        services.AddScoped<IPerfilRepository, PerfilRepository>();
        services.AddScoped<IPermissaoRepository, PermissaoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }
}
