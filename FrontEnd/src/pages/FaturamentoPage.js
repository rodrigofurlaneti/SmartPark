import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, StatCard, PageHeader } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import faturamentoService from '../services/faturamentoService';
import { formatCurrency, formatDateTime } from '../utils/formatters';
import { ToastContext } from '../components/layout/MainLayout';

const UNIDADE_ID = 1;

function FaturamentoPage() {
  const addToast = useContext(ToastContext);
  const inicio = new Date(new Date().setDate(new Date().getDate() - 30)).toISOString();
  const fim    = new Date().toISOString();

  const { data, loading, refetch } = useApi(() => faturamentoService.listarPorPeriodo(UNIDADE_ID, inicio, fim), []);
  const [modalAbrir,   setModalAbrir]   = useState(false);
  const [modalFechar,  setModalFechar]  = useState(null);
  const [saving, setSaving] = useState(false);

  const [abrirForm, setAbrirForm] = useState({ numFechamento: '', numTerminal: 1, unidadeId: UNIDADE_ID, usuarioId: 1, saldoInicial: '' });
  const [fecharForm, setFecharForm] = useState({ valorTotal: '', valorDinheiro: '', valorCartaoDebito: '', valorCartaoCredito: '' });

  const totalGeral = (data || []).reduce((s, f) => s + f.valorTotal, 0);
  const totalDin   = (data || []).reduce((s, f) => s + f.valorDinheiro, 0);
  const totalDeb   = (data || []).reduce((s, f) => s + f.valorCartaoDebito, 0);
  const totalCred  = (data || []).reduce((s, f) => s + f.valorCartaoCredito, 0);

  const handleAbrirTurno = async () => {
    setSaving(true);
    try {
      await faturamentoService.abrirTurno({ ...abrirForm, numFechamento: +abrirForm.numFechamento, saldoInicial: +abrirForm.saldoInicial });
      addToast('Turno aberto com sucesso!', 'success');
      setModalAbrir(false);
      refetch();
    } catch { addToast('Erro ao abrir turno.', 'error'); }
    finally { setSaving(false); }
  };

  const handleFecharTurno = async () => {
    setSaving(true);
    try {
      await faturamentoService.fecharTurno({ faturamentoId: modalFechar.id, ...fecharForm,
        valorTotal: +fecharForm.valorTotal, valorDinheiro: +fecharForm.valorDinheiro,
        valorCartaoDebito: +fecharForm.valorCartaoDebito, valorCartaoCredito: +fecharForm.valorCartaoCredito });
      addToast('Turno fechado!', 'success');
      setModalFechar(null);
      refetch();
    } catch { addToast('Erro ao fechar turno.', 'error'); }
    finally { setSaving(false); }
  };

  const handleSangria = async (id) => {
    const v = window.prompt('Valor da sangria (R$):');
    if (!v) return;
    try { await faturamentoService.sangria(id, parseFloat(v)); addToast('Sangria registrada.', 'success'); refetch(); }
    catch { addToast('Erro ao registrar sangria.', 'error'); }
  };

  return (
    <div className="fade-in">
      <PageHeader
        title="Faturamento"
        subtitle="Controle de turnos e caixa"
        action={<Button onClick={() => setModalAbrir(true)}>＋ Abrir Turno</Button>}
      />

      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4,1fr)', gap: 16, marginBottom: 24 }}>
        <StatCard icon="💰" label="Total (30 dias)"  value={formatCurrency(totalGeral)} color="var(--success)" />
        <StatCard icon="💵" label="Dinheiro"          value={formatCurrency(totalDin)}   color="var(--warning)" />
        <StatCard icon="💳" label="Débito"            value={formatCurrency(totalDeb)}   color="var(--accent)" />
        <StatCard icon="💳" label="Crédito"           value={formatCurrency(totalCred)}  color="var(--accent-dim,#0099BB)" />
      </div>

      <Card>
        {loading ? <Spinner /> : (
          <Table
            columns={[
              { key: 'numFechamento', label: 'Fechamento', render: v => <span className="mono" style={{ color: 'var(--accent)' }}>#{v}</span> },
              { key: 'numTerminal',   label: 'Terminal' },
              { key: 'dataAbertura',  label: 'Abertura',   render: v => formatDateTime(v) },
              { key: 'dataFechamento',label: 'Fechamento', render: v => v ? formatDateTime(v) : '—' },
              { key: 'valorDinheiro', label: 'Dinheiro',   render: v => formatCurrency(v) },
              { key: 'valorCartaoDebito',  label: 'Débito',  render: v => formatCurrency(v) },
              { key: 'valorCartaoCredito', label: 'Crédito', render: v => formatCurrency(v) },
              { key: 'valorTotal',    label: 'Total',      render: v => <strong style={{ color: 'var(--success)' }}>{formatCurrency(v)}</strong> },
            ]}
            data={data || []}
            onAction={row => (
              <div style={{ display: 'flex', gap: 8 }}>
                {!row.dataFechamento && <Button small variant="secondary" onClick={() => setModalFechar(row)}>Fechar</Button>}
                <Button small variant="ghost" onClick={() => handleSangria(row.id)}>Sangria</Button>
              </div>
            )}
          />
        )}
      </Card>

      {modalAbrir && (
        <Modal title="Abrir Turno" onClose={() => setModalAbrir(false)}>
          <FormField label="Número do Fechamento">
            <input type="number" value={abrirForm.numFechamento} onChange={e => setAbrirForm({ ...abrirForm, numFechamento: e.target.value })} placeholder="Ex: 100" />
          </FormField>
          <FormField label="Terminal">
            <select value={abrirForm.numTerminal} onChange={e => setAbrirForm({ ...abrirForm, numTerminal: +e.target.value })}>
              <option value={1}>Terminal 1</option>
              <option value={2}>Terminal 2</option>
            </select>
          </FormField>
          <FormField label="Saldo Inicial (R$)">
            <input type="number" step="0.01" value={abrirForm.saldoInicial} onChange={e => setAbrirForm({ ...abrirForm, saldoInicial: e.target.value })} placeholder="0.00" />
          </FormField>
          <div style={{ display: 'flex', gap: 10, justifyContent: 'flex-end', marginTop: 8 }}>
            <Button variant="ghost" onClick={() => setModalAbrir(false)}>Cancelar</Button>
            <Button onClick={handleAbrirTurno} disabled={saving}>{saving ? 'Abrindo...' : 'Abrir Turno'}</Button>
          </div>
        </Modal>
      )}

      {modalFechar && (
        <Modal title={`Fechar Turno #${modalFechar.numFechamento}`} onClose={() => setModalFechar(null)}>
          {['valorTotal','valorDinheiro','valorCartaoDebito','valorCartaoCredito'].map(k => (
            <FormField key={k} label={k.replace('valor','').replace(/([A-Z])/g,' $1').trim() + ' (R$)'}>
              <input type="number" step="0.01" value={fecharForm[k]} onChange={e => setFecharForm({ ...fecharForm, [k]: e.target.value })} placeholder="0.00" />
            </FormField>
          ))}
          <div style={{ display: 'flex', gap: 10, justifyContent: 'flex-end', marginTop: 8 }}>
            <Button variant="ghost" onClick={() => setModalFechar(null)}>Cancelar</Button>
            <Button variant="success" onClick={handleFecharTurno} disabled={saving}>{saving ? 'Fechando...' : 'Fechar Turno'}</Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default FaturamentoPage;
