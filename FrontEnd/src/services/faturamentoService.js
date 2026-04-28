import api from './api';

const faturamentoService = {
  // Lista todos e filtra client-side
  listarTodos: ()                           => api.get('/faturamentos'),
  listarPorPeriodo: (unidadeId, inicio, fim) =>
    api.get('/faturamentos').then(res => ({
      ...res,
      data: (res.data || []).filter(f => {
        const d = new Date(f.dataAbertura);
        return f.unidade_Id === unidadeId && d >= new Date(inicio) && d <= new Date(fim);
      }),
    })),

  obterPorId: (id) => api.get(`/faturamentos/${id}`),

  // Abrir turno — POST /api/faturamentos
  abrirTurno: (dto) =>
    api.post('/faturamentos', {
      NumFechamento: dto.numFechamento,
      NumTerminal:   dto.numTerminal,
      UnidadeId:     dto.unidadeId,
      UsuarioId:     dto.usuarioId,
      EmpresaId:     dto.empresaId || 1,
      SaldoInicial:  dto.saldoInicial,
    }),

  // Fechar turno — PUT /api/faturamentos/{id}
  fecharTurno: (dto) =>
    api.put(`/faturamentos/${dto.faturamentoId}`, {
      ValorTotal:          dto.valorTotal,
      ValorDinheiro:       dto.valorDinheiro,
      ValorCartaoDebito:   dto.valorCartaoDebito,
      ValorCartaoCredito:  dto.valorCartaoCredito,
      ValorRotativo:       dto.valorRotativo       ?? null,
      ValorMensalidade:    dto.valorMensalidade     ?? null,
      ValorSemParar:       dto.valorSemParar        ?? null,
      ValorSeloDesconto:   dto.valorSeloDesconto    ?? null,
      TicketFinal:         dto.ticketFinal          ?? null,
    }),

  deletar: (id) => api.delete(`/faturamentos/${id}`),
};

export default faturamentoService;
