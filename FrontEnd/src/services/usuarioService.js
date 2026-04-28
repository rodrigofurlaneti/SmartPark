import api from './api';
const usuarioService = {
  login:      (dto)  => api.post('/usuario/login', dto),
  criar:      (dto)  => api.post('/usuario', dto),
  obterPorId: (id)   => api.get(`/usuario/${id}`),
  bloquear:   (id)   => api.delete(`/usuario/${id}`),
};
export default usuarioService;
