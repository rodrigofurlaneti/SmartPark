import api from './api';
const unidadeService = {
  listarTodas:  ()        => api.get('/unidade'),
  listarAtivas: ()        => api.get('/unidade/ativas'),
  obterPorId:   (id)      => api.get(`/unidade/${id}`),
  criar:        (dto)     => api.post('/unidade', dto),
  atualizar:    (id, dto) => api.put(`/unidade/${id}`, dto),
  inativar:     (id)      => api.delete(`/unidade/${id}`),
};
export default unidadeService;
