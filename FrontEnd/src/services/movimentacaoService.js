import api from './api';

const movimentacaoService = {
  // Lista todas e filtra client-side
  listarTodas: ()                    => api.get('/movimentacoes'),
  listarAbertas: (unidadeId)         =>
    api.get('/movimentacoes').then(res => ({
      ...res,
      data: (res.data || []).filter(m => !m.dataSaida && m.unidade_Id === unidadeId),
    })),
  listarPorPeriodo: (unidadeId, inicio, fim) =>
    api.get('/movimentacoes').then(res => ({
      ...res,
      data: (res.data || []).filter(m => {
        const d = new Date(m.dataEntrada);
        return m.unidade_Id === unidadeId && d >= new Date(inicio) && d <= new Date(fim);
      }),
    })),

  obterPorId: (id) => api.get(`/movimentacoes/${id}`),

  // Registrar entrada — POST /api/movimentacoes
  registrarEntrada: (dto) =>
    api.post('/movimentacoes', {
      Placa:           dto.placa,
      UnidadeId:       dto.unidadeId,
      EmpresaId:       dto.empresaId || 1,
      ClienteId:       dto.clienteId ?? null,
      NumeroContrato:  dto.numeroContrato ?? null,
    }),

  // Registrar saída — PUT /api/movimentacoes/{id}
  registrarSaida: (dto) =>
    api.put(`/movimentacoes/${dto.movimentacaoId}`, {
      ValorCobrado:    dto.valorCobrado,
      FormaPagamento:  dto.formaPagamento ?? null,
      CpfParaNF:       dto.cpfParaNF ?? null,
    }),

  deletar: (id) => api.delete(`/movimentacoes/${id}`),
};

export default movimentacaoService;
