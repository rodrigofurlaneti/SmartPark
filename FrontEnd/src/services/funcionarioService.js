import api from './api';
const funcionarioService = {
  listarAtivos:     ()          => api.get('/funcionario/ativos'),
  listarPorUnidade: (unidadeId) => api.get(`/funcionario/unidade/${unidadeId}`),
  obterPorId:       (id)        => api.get(`/funcionario/${id}`),
  criar:            (dto)       => api.post('/funcionario', dto),
  alterarSalario:   (id, valor) => api.put(`/funcionario/${id}/salario/${valor}`),
  desligar:         (id)        => api.delete(`/funcionario/${id}`),
};
export default funcionarioService;
