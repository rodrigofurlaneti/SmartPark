import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, Badge, PageHeader } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import contasAPagarService from '../services/contasAPagarService';
import { formatDate, formatCurrency } from '../utils/formatters';
import { ToastContext } from '../components/layout/MainLayout';

function ContasPagarPage() {
  const addToast = useContext(ToastContext);
  const { data, loading, refetch } = useApi(() => contasAPagarService.listarEmAberto(), []);
  const [modal, setModal] = useState(false);
  const [saving, setSaving] = useState(false);
  const [form, setForm] = useState({ numeroDocumento: '', dataVencimento: '', valorTotal: '', unidadeId: 1 });

  const isVencida = (d) => new Date(d) < new Date();
  const totalEmAberto = (data || []).reduce((s, c) => s + c.valorTotal, 0);
  const vencidas      = (data || []).filter(c => isVencida(c.dataVencimento));

  const handleCriar = async () => {
    setSaving(true);
    try {
      await contasAPagarService.criar({ ...form, valorTotal: +form.valorTotal, unidadeId: +form.unidadeId });
      addToast('Conta cadastrada!', 'success');
      setModal(false);
      setForm({ numeroDocumento: '', dataVencimento: '', valorTotal: '', unidadeId: 1 });
      refetch();
    } catch { addToast('Erro ao cadastrar.', 'error'); }
    finally { setSaving(false); }
  };

  const handlePagar = async (id, doc) => {
    if (!window.confirm(`Marcar ${doc} como paga?`)) return;
    try { await contasAPagarService.pagar(id); addToast('Conta marcada como paga!', 'success'); refetch(); }
    catch { addToast('Erro ao pagar.', 'error'); }
  };

  return (
    <div className="fade-in">
      <PageHeader
        title="Contas a Pagar"
        subtitle={`${(data || []).length} em aberto · ${vencidas.length} vencidas`}
        action={<Button onClick={() => setModal(true)}>＋ Nova Conta</Button>}
      />

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16, marginBottom: 24 }}>
        <div style={{ background: 'var(--surface)', border: '1px solid var(--border)', borderRadius: 12, padding: 20 }}>
          <div style={{ fontSize: 12, color: 'var(--text-muted)', marginBottom: 6, textTransform: 'uppercase', letterSpacing: 0.5 }}>Total em aberto</div>
          <div style={{ fontSize: 26, fontWeight: 700, color: 'var(--warning)' }}>{formatCurrency(totalEmAberto)}</div>
        </div>
        <div style={{ background: 'var(--surface)', border: '1px solid var(--border)', borderRadius: 12, padding: 20 }}>
          <div style={{ fontSize: 12, color: 'var(--text-muted)', marginBottom: 6, textTransform: 'uppercase', letterSpacing: 0.5 }}>Total vencido</div>
          <div style={{ fontSize: 26, fontWeight: 700, color: 'var(--danger)' }}>{formatCurrency(vencidas.reduce((s, c) => s + c.valorTotal, 0))}</div>
        </div>
      </div>

      <Card>
        {loading ? <Spinner /> : (
          <Table
            columns={[
              { key: 'numeroDocumento', label: 'Documento', render: v => <span className="mono">{v}</span> },
              { key: 'dataVencimento',  label: 'Vencimento', render: (v, row) => (
                <span style={{ color: isVencida(v) ? 'var(--danger)' : 'var(--text)' }}>
                  {formatDate(v)} {isVencida(v) && '⚠'}
                </span>
              )},
              { key: 'valorTotal', label: 'Valor', render: v => <strong style={{ color: 'var(--warning)' }}>{formatCurrency(v)}</strong> },
              { key: 'status', label: 'Status', render: (v, row) => (
                <Badge color={isVencida(row.dataVencimento) ? 'var(--danger)' : 'var(--warning)'}>
                  {isVencida(row.dataVencimento) ? 'Vencido' : 'Em Aberto'}
                </Badge>
              )},
              { key: 'unidadeId', label: 'Unidade', render: v => v ? `Unidade ${v}` : '—', muted: true },
            ]}
            data={data || []}
            onAction={row => (
              <Button small variant="success" onClick={() => handlePagar(row.id, row.numeroDocumento)}>Pagar</Button>
            )}
          />
        )}
      </Card>

      {modal && (
        <Modal title="Nova Conta a Pagar" onClose={() => setModal(false)}>
          <FormField label="Número do Documento / NF">
            <input value={form.numeroDocumento} onChange={e => setForm({ ...form, numeroDocumento: e.target.value })} placeholder="NF-001234" className="mono" />
          </FormField>
          <FormField label="Data de Vencimento">
            <input type="date" value={form.dataVencimento} onChange={e => setForm({ ...form, dataVencimento: e.target.value })} />
          </FormField>
          <FormField label="Valor Total (R$)">
            <input type="number" step="0.01" value={form.valorTotal} onChange={e => setForm({ ...form, valorTotal: e.target.value })} placeholder="0.00" />
          </FormField>
          <FormField label="Unidade">
            <select value={form.unidadeId} onChange={e => setForm({ ...form, unidadeId: e.target.value })}>
              <option value={1}>Centro</option>
              <option value={2}>Vila Madalena</option>
            </select>
          </FormField>
          <div style={{ display: 'flex', gap: 10, justifyContent: 'flex-end', marginTop: 8 }}>
            <Button variant="ghost" onClick={() => setModal(false)}>Cancelar</Button>
            <Button onClick={handleCriar} disabled={!form.numeroDocumento || !form.dataVencimento || !form.valorTotal || saving}>
              {saving ? 'Salvando...' : 'Cadastrar'}
            </Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default ContasPagarPage;
