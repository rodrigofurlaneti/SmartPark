import api from './api';

const usuarioService = {
  // Autenticação — POST /api/usuarios/autenticar  {Login, Senha}
  login: ({ login, senha }) =>
    api.post('/usuarios/autenticar', { Login: login, Senha: senha }),

  // CRUD
  listarTodos: ()       => api.get('/usuarios'),
  obterPorId:  (id)     => api.get(`/usuarios/${id}`),
  criar: (dto) =>
    api.post('/usuarios', {
      Login:     dto.login,
      Senha:     dto.senha,
      EmpresaId: dto.empresaId,
      UnidadeId: dto.unidadeId ?? null,
    }),
  atualizar: (id, dto) => api.put(`/usuarios/${id}`, { Ativo: dto.ativo }),
  bloquear:  (id)      => api.delete(`/usuarios/${id}`),
};

export default usuarioService;
