import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, Badge, PageHeader, StatCard } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import pedidoSeloService from '../services/pedidoSeloService';
import { formatDate, formatCurrency } from '../utils/formatters';
import { ToastContext } from '../components/layout/MainLayout';

const EMPRESA_ID = 1;

const STATUS_COLOR = {
  Pendente:   'var(--warning)',
  Aprovado:   'var(--success)',
  Entregue:   'var(--action-blue)',
  Cancelado:  'var(--danger)',
};

function PedidoSeloPage() {
  const addToast = useContext(ToastContext);
  const { data, loading, refetch } = useApi(() => pedidoSeloService.listarTodos(), []);
  const [modal,  setModal]  = useState(false);
  const [saving, setSaving] = useState(false);
  const [form,   setForm]   = useState({
    clienteId: '', quantidadeSelo: '', valorTotal: '', empresaId: EMPRESA_ID,
  });

  const totalPedidos   = (data || []).length;
  const totalSelosEmit = (data || []).reduce((s, p) => s + (p.quantidadeSelo || 0), 0);
  const totalValor     = (data || []).reduce((s, p) => s + (p.valorTotal || 0), 0);
  const pendentes      = (data || []).filter(p => (p.status || '').toLowerCase() === 'pendente').length;

  const handleCriar = async () => {
    if (!form.clienteId || !form.quantidadeSelo || !form.valorTotal) {
      addToast('Preencha todos os campos obrigatórios.', 'warning'); return;
    }
    setSaving(true);
    try {
      await pedidoSeloService.criar({
        clienteId:      +form.clienteId,
        quantidadeSelo: +form.quantidadeSelo,
        valorTotal:     +form.valorTotal,
        empresaId:      EMPRESA_ID,
      });
      addToast('Pedido de selo criado!', 'success');
      setModal(false);
      setForm({ clienteId: '', quantidadeSelo: '', valorTotal: '', empresaId: EMPRESA_ID });
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao criar pedido.', 'error'); }
    finally { setSaving(false); }
  };

  const handleCancelar = async (id) => {
    if (!window.confirm('Cancelar este pedido de selo?')) return;
    try {
      await pedidoSeloService.cancelar(id);
      addToast('Pedido cancelado.', 'success');
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao cancelar.', 'error'); }
  };

  return (
    <div className="fade-in">
      <PageHeader
        title="Pedidos de Selo"
        subtitle={`${totalPedidos} pedidos · ${pendentes} pendentes`}
        action={<Button onClick={() => setModal(true)}>＋ Novo Pedido</Button>}
      />

      {/* KPIs */}
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: 16, marginBottom: 28 }}>
        <StatCard icon="🏷" label="Total de pedidos"   value={totalPedidos}  color="var(--action-blue)" />
        <StatCard icon="📦" label="Selos emitidos"      value={totalSelosEmit} color="var(--success)" />
        <StatCard icon="💰" label="Valor total"         value={formatCurrency(totalValor)} color="var(--warning)" />
      </div>

      <Card>
        {loading ? <Spinner /> : (
          <Table
            columns={[
              {
                key: 'id', label: 'Pedido',
                render: v => <span className="mono" style={{ color: 'var(--action-blue)', fontWeight: 600 }}>#{v}</span>,
              },
              { key: 'cliente_Id',     label: 'Cliente ID',    render: v => `#${v}`, muted: true },
              { key: 'quantidadeSelo', label: 'Qtd. Selos',    render: v => <strong>{v}</strong> },
              {
                key: 'valorTotal', label: 'Valor',
                render: v => <span style={{ color: 'var(--warning)', fontWeight: 600 }}>{formatCurrency(v)}</span>,
              },
              {
                key: 'status', label: 'Status',
                render: v => (
                  <Badge color={STATUS_COLOR[v] || 'var(--action-blue)'}>{v || 'Pendente'}</Badge>
                ),
              },
              { key: 'dataInsercao', label: 'Data', render: v => formatDate(v), muted: true },
            ]}
            data={data || []}
            onAction={row => {
              const status = (row.status || '').toLowerCase();
              if (status === 'cancelado' || status === 'entregue') return null;
              return (
                <Button small variant="ghost" onClick={() => handleCancelar(row.id)}>
                  Cancelar
                </Button>
              );
            }}
          />
        )}
      </Card>

      {modal && (
        <Modal title="Novo Pedido de Selo" onClose={() => setModal(false)}>
          <FormField label="ID do Cliente">
            <input
              type="number"
              value={form.clienteId}
              onChange={e => setForm({ ...form, clienteId: e.target.value })}
              placeholder="ID do cliente mensalista"
              autoFocus
            />
          </FormField>
          <FormField label="Quantidade de Selos">
            <input
              type="number"
              min={1}
              value={form.quantidadeSelo}
              onChange={e => setForm({ ...form, quantidadeSelo: e.target.value })}
              placeholder="Ex: 10"
            />
          </FormField>
          <FormField label="Valor Total (R$)">
            <input
              type="number"
              step="0.01"
              value={form.valorTotal}
              onChange={e => setForm({ ...form, valorTotal: e.target.value })}
              placeholder="0,00"
            />
          </FormField>
          <div style={{ display: 'flex', gap: 12, justifyContent: 'flex-end', marginTop: 24 }}>
            <Button variant="ghost" onClick={() => setModal(false)}>Cancelar</Button>
            <Button
              onClick={handleCriar}
              disabled={!form.clienteId || !form.quantidadeSelo || !form.valorTotal || saving}
            >
              {saving ? 'Criando…' : 'Criar Pedido'}
            </Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default PedidoSeloPage;
