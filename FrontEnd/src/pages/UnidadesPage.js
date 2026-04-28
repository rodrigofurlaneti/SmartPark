import React, { useState, useContext } from 'react';
import { Card, Button, Badge, Modal, FormField, PageHeader, StatCard } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import unidadeService from '../services/unidadeService';
import { ToastContext } from '../components/layout/MainLayout';

const EMPRESA_ID = 1;

function UnidadesPage() {
  const addToast = useContext(ToastContext);
  const { data, loading, refetch } = useApi(() => unidadeService.listarTodas(), []);
  const [modal,  setModal]  = useState(false);
  const [saving, setSaving] = useState(false);
  const [form,   setForm]   = useState({
    nome: '', numeroVagas: 50, diaVencimento: 10,
    empresaId: EMPRESA_ID, cnpj: '', ccm: '',
  });

  const handleCriar = async () => {
    if (!form.nome) { addToast('Informe o nome da unidade.', 'warning'); return; }
    setSaving(true);
    try {
      await unidadeService.criar(form);
      addToast('Unidade criada com sucesso!', 'success');
      setModal(false);
      setForm({ nome: '', numeroVagas: 50, diaVencimento: 10, empresaId: EMPRESA_ID, cnpj: '', ccm: '' });
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao criar unidade.', 'error'); }
    finally { setSaving(false); }
  };

  const handleInativar = async (id, nome) => {
    if (!window.confirm(`Inativar a unidade "${nome}"?`)) return;
    try {
      await unidadeService.inativar(id);
      addToast('Unidade inativada.', 'success');
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao inativar.', 'error'); }
  };

  if (loading) return <Spinner />;

  const ativas = (data || []).filter(u => u.ativa).length;

  return (
    <div className="fade-in">
      <PageHeader
        title="Unidades"
        subtitle={`${ativas} ativas de ${(data || []).length}`}
        action={<Button onClick={() => setModal(true)}>＋ Nova Unidade</Button>}
      />

      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: 16 }}>
        {(data || []).map(u => (
          <Card
            key={u.id}
            style={{ cursor: 'default' }}
            onMouseEnter={e => e.currentTarget.style.borderColor = 'var(--action-blue)'}
            onMouseLeave={e => e.currentTarget.style.borderColor = 'var(--hairline)'}
          >
            {/* Header */}
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: 20 }}>
              <div>
                {u.codigo && (
                  <div style={{ fontSize: 11, color: 'var(--ink-48)', letterSpacing: '1px', textTransform: 'uppercase', marginBottom: 4 }} className="mono">
                    {u.codigo}
                  </div>
                )}
                <h3 style={{ fontSize: 19, fontWeight: 600, letterSpacing: '-0.374px', color: 'var(--ink)', lineHeight: 1.2 }}>
                  {u.nome}
                </h3>
              </div>
              <Badge color={u.ativa ? 'var(--success)' : 'var(--danger)'}>
                {u.ativa ? 'Ativa' : 'Inativa'}
              </Badge>
            </div>

            {/* Stats */}
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 10, marginBottom: 20 }}>
              <div style={{ background: 'var(--parchment)', borderRadius: 12, padding: '12px 16px' }}>
                <div style={{ fontSize: 24, fontWeight: 600, color: 'var(--action-blue)', letterSpacing: '-0.374px' }}>
                  {u.numeroVaga || u.numeroVagas || '—'}
                </div>
                <div style={{ fontSize: 12, color: 'var(--ink-48)', marginTop: 2 }}>Vagas</div>
              </div>
              <div style={{ background: 'var(--parchment)', borderRadius: 12, padding: '12px 16px' }}>
                <div style={{ fontSize: 20, fontWeight: 600, color: 'var(--warning)', letterSpacing: '-0.374px' }}>
                  dia {u.diaVencimento || '—'}
                </div>
                <div style={{ fontSize: 12, color: 'var(--ink-48)', marginTop: 2 }}>Vencimento</div>
              </div>
            </div>

            {u.cnpj && (
              <div style={{ fontSize: 12, color: 'var(--ink-48)', marginBottom: 16, letterSpacing: '0.2px' }} className="mono">
                CNPJ: {u.cnpj}
              </div>
            )}

            <div style={{ display: 'flex', gap: 8 }}>
              {u.ativa && (
                <Button small variant="ghost" onClick={() => handleInativar(u.id, u.nome)}>
                  Inativar
                </Button>
              )}
            </div>
          </Card>
        ))}
      </div>

      {modal && (
        <Modal title="Nova Unidade" onClose={() => setModal(false)}>
          <FormField label="Nome da Unidade">
            <input value={form.nome} onChange={e => setForm({ ...form, nome: e.target.value })}
              placeholder="Ex: Unidade Consolação" autoFocus />
          </FormField>
          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
            <FormField label="Número de Vagas">
              <input type="number" value={form.numeroVagas}
                onChange={e => setForm({ ...form, numeroVagas: +e.target.value })} />
            </FormField>
            <FormField label="Dia Vencimento (1–28)">
              <input type="number" min={1} max={28} value={form.diaVencimento}
                onChange={e => setForm({ ...form, diaVencimento: +e.target.value })} />
            </FormField>
          </div>
          <FormField label="CNPJ (opcional)">
            <input value={form.cnpj} onChange={e => setForm({ ...form, cnpj: e.target.value })}
              placeholder="00.000.000/0000-00" className="mono" />
          </FormField>
          <FormField label="CCM (opcional)">
            <input value={form.ccm} onChange={e => setForm({ ...form, ccm: e.target.value })}
              placeholder="CCM da prefeitura" />
          </FormField>
          <div style={{ display: 'flex', gap: 12, justifyContent: 'flex-end', marginTop: 24 }}>
            <Button variant="ghost" onClick={() => setModal(false)}>Cancelar</Button>
            <Button onClick={handleCriar} disabled={!form.nome || saving}>
              {saving ? 'Criando…' : 'Criar Unidade'}
            </Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default UnidadesPage;
