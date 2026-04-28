import api from './api';

const clienteService = {
  listarTodos:       ()        => api.get('/clientes'),
  listarMensalistas: ()        =>
    api.get('/clientes').then(res => ({
      ...res,
      data: (res.data || []).filter(c => c.isMensalista),
    })),
  obterPorId:        (id)      => api.get(`/clientes/${id}`),
  obterPorDocumento: (doc)     =>
    api.get('/clientes').then(res => ({
      ...res,
      data: (res.data || []).find(c => c.documentoNumero === doc) ?? null,
    })),

  criar: (dto) =>
    api.post('/clientes', {
      Nome:         dto.nome,
      Documento:    dto.documento,
      IsMensalista: dto.isMensalista,
      EmpresaId:    dto.empresaId || 1,
    }),

  atualizar: (id, dto) =>
    api.put(`/clientes/${id}`, {
      Nome:         dto.nome,
      Documento:    dto.documento,
      IsMensalista: dto.isMensalista,
      Ativo:        dto.ativo,
    }),

  inativar: (id) => api.delete(`/clientes/${id}`),
};

export default clienteService;
