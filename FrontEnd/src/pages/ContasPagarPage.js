import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, Badge, PageHeader, StatCard } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import contasAPagarService from '../services/contasAPagarService';
import { formatDate, formatCurrency } from '../utils/formatters';
import { ToastContext } from '../components/layout/MainLayout';

const EMPRESA_ID = 1;

function ContasPagarPage() {
  const addToast = useContext(ToastContext);
  const { data, loading, refetch } = useApi(() => contasAPagarService.listarEmAberto(), []);
  const [modal,  setModal]  = useState(false);
  const [saving, setSaving] = useState(false);
  const [form,   setForm]   = useState({
    numeroDocumento: '', dataVencimento: '', valorTotal: '',
    empresaId: EMPRESA_ID, unidadeId: '',
  });

  const isVencida      = (d) => new Date(d) < new Date();
  const totalEmAberto  = (data || []).reduce((s, c) => s + (c.valorTotal || 0), 0);
  const vencidas       = (data || []).filter(c => isVencida(c.dataVencimento));

  const handleCriar = async () => {
    if (!form.numeroDocumento || !form.dataVencimento || !form.valorTotal) {
      addToast('Preencha todos os campos obrigatórios.', 'warning'); return;
    }
    setSaving(true);
    try {
      await contasAPagarService.criar({
        ...form,
        valorTotal: +form.valorTotal,
        unidadeId:  form.unidadeId ? +form.unidadeId : null,
      });
      addToast('Conta cadastrada!', 'success');
      setModal(false);
      setForm({ numeroDocumento: '', dataVencimento: '', valorTotal: '', empresaId: EMPRESA_ID, unidadeId: '' });
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao cadastrar.', 'error'); }
    finally { setSaving(false); }
  };

  const handlePagar = async (id, doc) => {
    if (!window.confirm(`Marcar "${doc}" como paga?`)) return;
    try {
      await contasAPagarService.pagar(id);
      addToast('Conta marcada como paga!', 'success');
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao registrar pagamento.', 'error'); }
  };

  return (
    <div className="fade-in">
      <PageHeader
        title="Contas a Pagar"
        subtitle={`${(data || []).length} em aberto · ${vencidas.length} vencidas`}
        action={<Button onClick={() => setModal(true)}>＋ Nova Conta</Button>}
      />

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16, marginBottom: 28 }}>
        <StatCard
          icon="📋" label="Total em aberto"
          value={formatCurrency(totalEmAberto)} color="var(--warning)"
        />
        <StatCard
          icon="⚠️" label="Total vencido"
          value={formatCurrency(vencidas.reduce((s, c) => s + (c.valorTotal || 0), 0))}
          color="var(--danger)"
        />
      </div>

      <Card>
        {loading ? <Spinner /> : (
          <Table
            columns={[
              { key: 'numeroDocumento', label: 'Documento', render: v => <span className="mono">{v}</span> },
              {
                key: 'dataVencimento', label: 'Vencimento',
                render: (v) => (
                  <span style={{ color: isVencida(v) ? 'var(--danger)' : 'var(--ink)' }}>
                    {formatDate(v)} {isVencida(v) ? '⚠' : ''}
                  </span>
                ),
              },
              {
                key: 'valorTotal', label: 'Valor',
                render: v => <strong style={{ color: 'var(--warning)' }}>{formatCurrency(v)}</strong>,
              },
              {
                key: 'dataVencimento', label: 'Status',
                render: v => (
                  <Badge color={isVencida(v) ? 'var(--danger)' : 'var(--warning)'}>
                    {isVencida(v) ? 'Vencida' : 'Em Aberto'}
                  </Badge>
                ),
              },
              { key: 'unidade_Id', label: 'Unidade', render: v => v ? `Unidade ${v}` : '—', muted: true },
            ]}
            data={data || []}
            onAction={row => (
              <Button small variant="success" onClick={() => handlePagar(row.id, row.numeroDocumento)}>
                Pagar
              </Button>
            )}
          />
        )}
      </Card>

      {modal && (
        <Modal title="Nova Conta a Pagar" onClose={() => setModal(false)}>
          <FormField label="Número do Documento / NF">
            <input
              value={form.numeroDocumento}
              onChange={e => setForm({ ...form, numeroDocumento: e.target.value })}
              placeholder="NF-001234"
              className="mono"
              autoFocus
            />
          </FormField>
          <FormField label="Data de Vencimento">
            <input
              type="date"
              value={form.dataVencimento}
              onChange={e => setForm({ ...form, dataVencimento: e.target.value })}
            />
          </FormField>
          <FormField label="Valor Total (R$)">
            <input
              type="number" step="0.01"
              value={form.valorTotal}
              onChange={e => setForm({ ...form, valorTotal: e.target.value })}
              placeholder="0,00"
            />
          </FormField>
          <FormField label="Unidade (opcional)">
            <select value={form.unidadeId} onChange={e => setForm({ ...form, unidadeId: e.target.value })}>
              <option value="">Todas as unidades</option>
              <option value="1">Unidade 1 — Centro</option>
              <option value="2">Unidade 2 — Vila Madalena</option>
            </select>
          </FormField>
          <div style={{ display: 'flex', gap: 12, justifyContent: 'flex-end', marginTop: 24 }}>
            <Button variant="ghost" onClick={() => setModal(false)}>Cancelar</Button>
            <Button
              onClick={handleCriar}
              disabled={!form.numeroDocumento || !form.dataVencimento || !form.valorTotal || saving}
            >
              {saving ? 'Salvando…' : 'Cadastrar'}
            </Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default ContasPagarPage;
