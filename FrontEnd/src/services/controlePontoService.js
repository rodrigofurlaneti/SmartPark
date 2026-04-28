import api from './api';
const controlePontoService = {
  listarPorFuncionario: (funcionarioId) => api.get(`/controleponto/funcionario/${funcionarioId}`),
  registrarPonto:       (dto)           => api.post('/controleponto', dto),
  obterPorId:           (id)            => api.get(`/controleponto/${id}`),
};
export default controlePontoService;
