import api from './api';

const contasAPagarService = {
  listarTodas:      ()           => api.get('/contas-a-pagar'),
  listarEmAberto:   ()           =>
    api.get('/contas-a-pagar').then(res => ({
      ...res,
      data: (res.data || []).filter(c => c.ativo !== false),
    })),
  listarPorUnidade: (unidadeId)  =>
    api.get('/contas-a-pagar').then(res => ({
      ...res,
      data: (res.data || []).filter(c => c.unidade_Id === unidadeId),
    })),
  obterPorId: (id) => api.get(`/contas-a-pagar/${id}`),

  criar: (dto) =>
    api.post('/contas-a-pagar', {
      NumeroDocumento: dto.numeroDocumento,
      DataVencimento:  dto.dataVencimento,
      ValorTotal:      dto.valorTotal,
      EmpresaId:       dto.empresaId || 1,
      FornecedorId:    dto.fornecedorId  ?? null,
      UnidadeId:       dto.unidadeId     ?? null,
    }),

  atualizar: (id, dto) =>
    api.put(`/contas-a-pagar/${id}`, {
      DataVencimento: dto.dataVencimento,
      ValorTotal:     dto.valorTotal,
    }),

  // Soft delete marca como pago (remove da lista ativa)
  pagar: (id) => api.delete(`/contas-a-pagar/${id}`),
};

export default contasAPagarService;
