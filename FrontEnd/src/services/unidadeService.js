import api from './api';

const unidadeService = {
  listarTodas:  ()        => api.get('/unidades'),
  listarAtivas: ()        =>
    api.get('/unidades').then(res => ({
      ...res,
      data: (res.data || []).filter(u => u.ativa),
    })),
  obterPorId:   (id)      => api.get(`/unidades/${id}`),

  criar: (dto) =>
    api.post('/unidades', {
      Nome:           dto.nome,
      NumeroVagas:    dto.numeroVagas,
      DiaVencimento:  dto.diaVencimento,
      EmpresaId:      dto.empresaId || 1,
      Cnpj:           dto.cnpj  || null,
      Ccm:            dto.ccm   || null,
    }),

  atualizar: (id, dto) =>
    api.put(`/unidades/${id}`, {
      Nome:          dto.nome,
      NumeroVagas:   dto.numeroVagas,
      DiaVencimento: dto.diaVencimento,
      Ativa:         dto.ativa,
    }),

  inativar: (id) => api.delete(`/unidades/${id}`),
};

export default unidadeService;
