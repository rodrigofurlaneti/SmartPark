import api from './api';

const pedidoSeloService = {
  listarTodos:      ()        => api.get('/pedidos-selo'),
  obterPorId:       (id)      => api.get(`/pedidos-selo/${id}`),
  listarPorCliente: (cliId)   =>
    api.get('/pedidos-selo').then(res => ({
      ...res,
      data: (res.data || []).filter(p => p.cliente_Id === cliId),
    })),

  criar: (dto) =>
    api.post('/pedidos-selo', {
      ClienteId:      dto.clienteId,
      QuantidadeSelo: dto.quantidadeSelo,
      ValorTotal:     dto.valorTotal,
      EmpresaId:      dto.empresaId || 1,
    }),

  atualizar: (id, dto) =>
    api.put(`/pedidos-selo/${id}`, {
      QuantidadeSelo: dto.quantidadeSelo,
      ValorTotal:     dto.valorTotal,
      Status:         dto.status,
    }),

  cancelar: (id) => api.delete(`/pedidos-selo/${id}`),
};

export default pedidoSeloService;
