import api from './api';
const movimentacaoService = {
  registrarEntrada:  (dto)                    => api.post('/movimentacao/entrada', dto),
  registrarSaida:    (dto)                    => api.post('/movimentacao/saida', dto),
  obterPorId:        (id)                     => api.get(`/movimentacao/${id}`),
  listarAbertas:     (unidadeId)              => api.get(`/movimentacao/abertas/${unidadeId}`),
  listarPorPeriodo:  (unidadeId, inicio, fim) => api.get(`/movimentacao/periodo/${unidadeId}`, { params: { inicio, fim } }),
};
export default movimentacaoService;
