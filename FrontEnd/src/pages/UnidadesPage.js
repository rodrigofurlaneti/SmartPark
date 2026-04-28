import React, { useState, useContext } from 'react';
import { Card, Button, Badge, Modal, FormField, PageHeader } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import unidadeService from '../services/unidadeService';
import { ToastContext } from '../components/layout/MainLayout';

function UnidadesPage() {
  const addToast = useContext(ToastContext);
  const { data, loading, refetch } = useApi(() => unidadeService.listarTodas(), []);
  const [modal, setModal] = useState(false);
  const [saving, setSaving] = useState(false);
  const [form, setForm] = useState({ nome: '', numeroVagas: 50, diaVencimento: 10, empresaId: 1, cnpj: '', ccm: '' });

  const handleCriar = async () => {
    setSaving(true);
    try {
      await unidadeService.criar(form);
      addToast('Unidade criada com sucesso!', 'success');
      setModal(false);
      refetch();
    } catch { addToast('Erro ao criar unidade.', 'error'); }
    finally { setSaving(false); }
  };

  const handleInativar = async (id) => {
    if (!window.confirm('Inativar esta unidade?')) return;
    try { await unidadeService.inativar(id); addToast('Unidade inativada.', 'success'); refetch(); }
    catch { addToast('Erro ao inativar.', 'error'); }
  };

  if (loading) return <Spinner />;

  return (
    <div className="fade-in">
      <PageHeader
        title="Unidades"
        subtitle={`${(data || []).filter(u => u.ativa).length} ativas de ${(data || []).length}`}
        action={<Button onClick={() => setModal(true)}>＋ Nova Unidade</Button>}
      />

      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(290px, 1fr))', gap: 16 }}>
        {(data || []).map(u => (
          <Card key={u.id} style={{ transition: 'border-color 0.2s, transform 0.2s' }}
            onMouseEnter={e => { e.currentTarget.style.borderColor = 'var(--accent)'; e.currentTarget.style.transform = 'translateY(-2px)'; }}
            onMouseLeave={e => { e.currentTarget.style.borderColor = 'var(--border)';  e.currentTarget.style.transform = 'translateY(0)'; }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: 16 }}>
              <div>
                <div style={{ fontSize: 11, color: 'var(--text-muted)', letterSpacing: 1, textTransform: 'uppercase' }} className="mono">{u.codigo}</div>
                <h3 style={{ fontSize: 17, fontWeight: 700, marginTop: 4 }}>{u.nome}</h3>
              </div>
              <Badge color={u.ativa ? 'var(--success)' : 'var(--danger)'}>{u.ativa ? 'Ativa' : 'Inativa'}</Badge>
            </div>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 10, marginBottom: 14 }}>
              <div style={{ background: 'var(--bg)', borderRadius: 8, padding: '10px 14px' }}>
                <div style={{ fontSize: 22, fontWeight: 700, color: 'var(--accent)' }}>{u.numeroVaga}</div>
                <div style={{ fontSize: 12, color: 'var(--text-muted)' }}>Vagas</div>
              </div>
              <div style={{ background: 'var(--bg)', borderRadius: 8, padding: '10px 14px' }}>
                <div style={{ fontSize: 20, fontWeight: 700, color: 'var(--warning)' }}>dia {u.diaVencimento}</div>
                <div style={{ fontSize: 12, color: 'var(--text-muted)' }}>Vencimento</div>
              </div>
            </div>
            {u.cnpj && <div style={{ fontSize: 12, color: 'var(--text-muted)', marginBottom: 14 }} className="mono">CNPJ: {u.cnpj}</div>}
            <div style={{ display: 'flex', gap: 8 }}>
              <Button small variant="secondary">Editar</Button>
              {u.ativa && <Button small variant="ghost" onClick={() => handleInativar(u.id)}>Inativar</Button>}
            </div>
          </Card>
        ))}
      </div>

      {modal && (
        <Modal title="Nova Unidade" onClose={() => setModal(false)}>
          <FormField label="Nome da Unidade">
            <input value={form.nome} onChange={e => setForm({ ...form, nome: e.target.value })} placeholder="Ex: Unidade Consolação" />
          </FormField>
          <FormField label="Número de Vagas">
            <input type="number" value={form.numeroVagas} onChange={e => setForm({ ...form, numeroVagas: +e.target.value })} />
          </FormField>
          <FormField label="Dia de Vencimento (mensalistas)">
            <input type="number" min={1} max={28} value={form.diaVencimento} onChange={e => setForm({ ...form, diaVencimento: +e.target.value })} />
          </FormField>
          <FormField label="CNPJ (opcional)">
            <input value={form.cnpj} onChange={e => setForm({ ...form, cnpj: e.target.value })} placeholder="00.000.000/0000-00" className="mono" />
          </FormField>
          <FormField label="CCM (opcional)">
            <input value={form.ccm} onChange={e => setForm({ ...form, ccm: e.target.value })} placeholder="CCM da prefeitura" />
          </FormField>
          <div style={{ display: 'flex', gap: 10, justifyContent: 'flex-end', marginTop: 8 }}>
            <Button variant="ghost" onClick={() => setModal(false)}>Cancelar</Button>
            <Button onClick={handleCriar} disabled={!form.nome || saving}>{saving ? 'Criando...' : 'Criar Unidade'}</Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default UnidadesPage;
