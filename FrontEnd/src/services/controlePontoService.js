import api from './api';

const controlePontoService = {
  listarTodos: () => api.get('/controle-ponto'),
  listarPorFuncionario: (funcionarioId) =>
    api.get('/controle-ponto').then(res => ({
      ...res,
      data: (res.data || []).filter(p => p.funcionario_Id === funcionarioId),
    })),
  obterPorId: (id) => api.get(`/controle-ponto/${id}`),

  registrarPonto: (dto) =>
    api.post('/controle-ponto', {
      FuncionarioId: dto.funcionarioId,
      Tipo:          dto.tipo,
      EmpresaId:     dto.empresaId || 1,
    }),

  deletar: (id) => api.delete(`/controle-ponto/${id}`),
};

export default controlePontoService;
