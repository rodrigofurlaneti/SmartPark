import api from './api';
const faturamentoService = {
  abrirTurno:       (dto)                     => api.post('/faturamento/abrir-turno', dto),
  fecharTurno:      (dto)                     => api.put('/faturamento/fechar-turno', dto),
  obterPorId:       (id)                      => api.get(`/faturamento/${id}`),
  listarPorPeriodo: (unidadeId, inicio, fim)  => api.get(`/faturamento/periodo/${unidadeId}`, { params: { inicio, fim } }),
  sangria:          (id, valor)               => api.post(`/faturamento/${id}/sangria/${valor}`),
};
export default faturamentoService;
