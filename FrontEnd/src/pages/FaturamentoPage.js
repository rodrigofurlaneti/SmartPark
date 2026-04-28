import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, StatCard, PageHeader } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import faturamentoService from '../services/faturamentoService';
import { formatCurrency, formatDateTime } from '../utils/formatters';
import { ToastContext } from '../components/layout/MainLayout';

const EMPRESA_ID  = 1;
const UNIDADE_ID  = 1;
const USUARIO_ID  = 1; // idealmente viria do contexto do user logado

function FaturamentoPage() {
  const addToast = useContext(ToastContext);
  const inicio   = new Date(new Date().setDate(new Date().getDate() - 30)).toISOString();
  const fim      = new Date().toISOString();

  const { data, loading, refetch } = useApi(
    () => faturamentoService.listarPorPeriodo(UNIDADE_ID, inicio, fim), []
  );

  const [modalAbrir,  setModalAbrir]  = useState(false);
  const [modalFechar, setModalFechar] = useState(null);
  const [saving, setSaving] = useState(false);

  const [abrirForm, setAbrirForm] = useState({
    numFechamento: '', numTerminal: 1,
    unidadeId: UNIDADE_ID, usuarioId: USUARIO_ID,
    empresaId: EMPRESA_ID, saldoInicial: '',
  });

  const [fecharForm, setFecharForm] = useState({
    valorTotal: '', valorDinheiro: '',
    valorCartaoDebito: '', valorCartaoCredito: '',
    valorRotativo: '', valorMensalidade: '',
    valorSemParar: '', valorSeloDesconto: '',
    ticketFinal: '',
  });

  const totalGeral = (data || []).reduce((s, f) => s + (f.valorTotal || 0), 0);
  const totalDin   = (data || []).reduce((s, f) => s + (f.valorDinheiro || 0), 0);
  const totalDeb   = (data || []).reduce((s, f) => s + (f.valorCartaoDebito || 0), 0);
  const totalCred  = (data || []).reduce((s, f) => s + (f.valorCartaoCredito || 0), 0);

  const handleAbrirTurno = async () => {
    if (!abrirForm.numFechamento) { addToast('Informe o número do fechamento.', 'warning'); return; }
    setSaving(true);
    try {
      await faturamentoService.abrirTurno({
        ...abrirForm,
        numFechamento: +abrirForm.numFechamento,
        saldoInicial:  +abrirForm.saldoInicial,
      });
      addToast('Turno aberto com sucesso!', 'success');
      setModalAbrir(false);
      setAbrirForm({ numFechamento: '', numTerminal: 1, unidadeId: UNIDADE_ID, usuarioId: USUARIO_ID, empresaId: EMPRESA_ID, saldoInicial: '' });
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao abrir turno.', 'error'); }
    finally { setSaving(false); }
  };

  const handleFecharTurno = async () => {
    setSaving(true);
    try {
      await faturamentoService.fecharTurno({
        faturamentoId:      modalFechar.id,
        valorTotal:         +fecharForm.valorTotal,
        valorDinheiro:      +fecharForm.valorDinheiro,
        valorCartaoDebito:  +fecharForm.valorCartaoDebito,
        valorCartaoCredito: +fecharForm.valorCartaoCredito,
        valorRotativo:      fecharForm.valorRotativo    ? +fecharForm.valorRotativo    : null,
        valorMensalidade:   fecharForm.valorMensalidade ? +fecharForm.valorMensalidade : null,
        valorSemParar:      fecharForm.valorSemParar    ? +fecharForm.valorSemParar    : null,
        valorSeloDesconto:  fecharForm.valorSeloDesconto? +fecharForm.valorSeloDesconto: null,
        ticketFinal:        fecharForm.ticketFinal      ? +fecharForm.ticketFinal      : null,
      });
      addToast('Turno fechado!', 'success');
      setModalFechar(null);
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao fechar turno.', 'error'); }
    finally { setSaving(false); }
  };

  const CAMPOS_FECHAR = [
    { key: 'valorTotal',         label: 'Valor Total (R$)',        required: true },
    { key: 'valorDinheiro',      label: 'Dinheiro (R$)',           required: true },
    { key: 'valorCartaoDebito',  label: 'Cartão Débito (R$)',      required: true },
    { key: 'valorCartaoCredito', label: 'Cartão Crédito (R$)',     required: true },
    { key: 'valorRotativo',      label: 'Rotativo (R$)',           required: false },
    { key: 'valorMensalidade',   label: 'Mensalidade (R$)',        required: false },
    { key: 'valorSemParar',      label: 'Sem Parar (R$)',          required: false },
    { key: 'valorSeloDesconto',  label: 'Selo Desconto (R$)',      required: false },
    { key: 'ticketFinal',        label: 'Ticket Final (nº)',       required: false },
  ];

  return (
    <div className="fade-in">
      <PageHeader
        title="Faturamento"
        subtitle="Controle de turnos e caixa"
        action={<Button onClick={() => setModalAbrir(true)}>＋ Abrir Turno</Button>}
      />

      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4,1fr)', gap: 16, marginBottom: 28 }}>
        <StatCard icon="💰" label="Total (30 dias)"  value={formatCurrency(totalGeral)} color="var(--success)" />
        <StatCard icon="💵" label="Dinheiro"          value={formatCurrency(totalDin)}   color="var(--warning)" />
        <StatCard icon="💳" label="Débito"            value={formatCurrency(totalDeb)}   color="var(--action-blue)" />
        <StatCard icon="💳" label="Crédito"           value={formatCurrency(totalCred)}  color="var(--action-blue)" />
      </div>

      <Card>
        {loading ? <Spinner /> : (
          <Table
            columns={[
              { key: 'numFechamento', label: 'Fechamento', render: v => <span className="mono" style={{ color: 'var(--action-blue)', fontWeight: 600 }}>#{v}</span> },
              { key: 'numTerminal',   label: 'Terminal',   render: v => `Terminal ${v}` },
              { key: 'dataAbertura',  label: 'Abertura',   render: v => formatDateTime(v) },
              { key: 'dataFechamento',label: 'Fechamento', render: v => v ? formatDateTime(v) : '—' },
              { key: 'valorDinheiro', label: 'Dinheiro',   render: v => formatCurrency(v) },
              { key: 'valorCartaoDebito',  label: 'Débito',  render: v => formatCurrency(v) },
              { key: 'valorCartaoCredito', label: 'Crédito', render: v => formatCurrency(v) },
              { key: 'valorTotal',    label: 'Total',      render: v => <strong style={{ color: 'var(--success)' }}>{formatCurrency(v)}</strong> },
            ]}
            data={data || []}
            onAction={row => !row.dataFechamento ? (
              <Button small variant="secondary" onClick={() => setModalFechar(row)}>
                Fechar Turno
              </Button>
            ) : null}
          />
        )}
      </Card>

      {/* Modal Abrir */}
      {modalAbrir && (
        <Modal title="Abrir Turno" onClose={() => setModalAbrir(false)}>
          <FormField label="Número do Fechamento">
            <input type="number" value={abrirForm.numFechamento}
              onChange={e => setAbrirForm({ ...abrirForm, numFechamento: e.target.value })}
              placeholder="Ex: 100" autoFocus />
          </FormField>
          <FormField label="Terminal">
            <select value={abrirForm.numTerminal}
              onChange={e => setAbrirForm({ ...abrirForm, numTerminal: +e.target.value })}>
              <option value={1}>Terminal 1</option>
              <option value={2}>Terminal 2</option>
              <option value={3}>Terminal 3</option>
            </select>
          </FormField>
          <FormField label="Saldo Inicial (R$)">
            <input type="number" step="0.01" value={abrirForm.saldoInicial}
              onChange={e => setAbrirForm({ ...abrirForm, saldoInicial: e.target.value })}
              placeholder="0,00" />
          </FormField>
          <div style={{ display: 'flex', gap: 12, justifyContent: 'flex-end', marginTop: 24 }}>
            <Button variant="ghost" onClick={() => setModalAbrir(false)}>Cancelar</Button>
            <Button onClick={handleAbrirTurno} disabled={saving}>
              {saving ? 'Abrindo…' : 'Abrir Turno'}
            </Button>
          </div>
        </Modal>
      )}

      {/* Modal Fechar */}
      {modalFechar && (
        <Modal title={`Fechar Turno #${modalFechar.numFechamento}`} onClose={() => setModalFechar(null)} width={560}>
          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '0 16px' }}>
            {CAMPOS_FECHAR.map(c => (
              <FormField key={c.key} label={c.label + (c.required ? '' : ' (opcional)')}>
                <input type="number" step="0.01" value={fecharForm[c.key]}
                  onChange={e => setFecharForm({ ...fecharForm, [c.key]: e.target.value })}
                  placeholder="0,00" />
              </FormField>
            ))}
          </div>
          <div style={{ display: 'flex', gap: 12, justifyContent: 'flex-end', marginTop: 24 }}>
            <Button variant="ghost" onClick={() => setModalFechar(null)}>Cancelar</Button>
            <Button variant="success" onClick={handleFecharTurno} disabled={saving}>
              {saving ? 'Fechando…' : 'Fechar Turno'}
            </Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default FaturamentoPage;
