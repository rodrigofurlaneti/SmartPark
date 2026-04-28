import api from './api';

const funcionarioService = {
  listarTodos:      ()          => api.get('/funcionarios'),
  listarAtivos:     ()          =>
    api.get('/funcionarios').then(res => ({
      ...res,
      data: (res.data || []).filter(f => f.ativo !== false),
    })),
  listarPorUnidade: (unidadeId) =>
    api.get('/funcionarios').then(res => ({
      ...res,
      data: (res.data || []).filter(f => f.unidade_Id === unidadeId),
    })),
  obterPorId:       (id)        => api.get(`/funcionarios/${id}`),

  criar: (dto) =>
    api.post('/funcionarios', {
      PessoaId:  dto.pessoaId,
      Salario:   dto.salario,
      Escala:    dto.escala,
      UnidadeId: dto.unidadeId,
      EmpresaId: dto.empresaId || 1,
    }),

  atualizar: (id, dto) =>
    api.put(`/funcionarios/${id}`, {
      Salario:  dto.salario ?? null,
      Escala:   dto.escala  ?? null,
      Ativo:    dto.ativo   ?? null,
    }),

  alterarSalario: (id, salario) =>
    api.put(`/funcionarios/${id}`, { Salario: salario }),

  desligar: (id) => api.delete(`/funcionarios/${id}`),
};

export default funcionarioService;
