import api from './api';
const contasAPagarService = {
  obterPorId:       (id)        => api.get(`/contasapagar/${id}`),
  listarPorUnidade: (unidadeId) => api.get(`/contasapagar/unidade/${unidadeId}`),
  listarEmAberto:   ()          => api.get('/contasapagar/em-aberto'),
  criar:            (dto)       => api.post('/contasapagar', dto),
  pagar:            (id)        => api.put(`/contasapagar/${id}/pagar`),
};
export default contasAPagarService;
