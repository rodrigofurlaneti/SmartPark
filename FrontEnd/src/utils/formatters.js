export const formatCurrency = (v) =>
  (v ?? 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });

export const formatDate = (d) =>
  d ? new Date(d).toLocaleDateString('pt-BR') : '—';

export const formatDateTime = (d) =>
  d ? new Date(d).toLocaleString('pt-BR') : '—';

export const formatTime = (d) =>
  d ? new Date(d).toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' }) : '—';

export const formatDuration = (start) => {
  if (!start) return '—';
  const mins = Math.floor((Date.now() - new Date(start)) / 60000);
  if (mins < 60) return `${mins}min`;
  return `${Math.floor(mins / 60)}h ${mins % 60}min`;
};

export const formatDoc = (doc) => {
  if (!doc) return '—';
  const d = doc.replace(/\D/g, '');
  if (d.length === 11) return d.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  if (d.length === 14) return d.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
  return doc;
};
