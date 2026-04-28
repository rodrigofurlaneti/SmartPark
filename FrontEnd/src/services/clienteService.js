import api from './api';
const clienteService = {
  listarTodos:       ()        => api.get('/cliente'),
  listarMensalistas: ()        => api.get('/cliente/mensalistas'),
  obterPorId:        (id)      => api.get(`/cliente/${id}`),
  obterPorDocumento: (doc)     => api.get(`/cliente/documento/${doc}`),
  criar:             (dto)     => api.post('/cliente', dto),
  atualizar:         (id, dto) => api.put(`/cliente/${id}`, dto),
  inativar:          (id)      => api.delete(`/cliente/${id}`),
};
export default clienteService;
