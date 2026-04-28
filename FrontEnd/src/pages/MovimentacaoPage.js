import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, Badge, PageHeader } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import movimentacaoService from '../services/movimentacaoService';
import { FORMAS_PAGAMENTO } from '../utils/constants';
import { formatTime, formatDuration } from '../utils/formatters';
import { ToastContext } from '../components/layout/MainLayout';

const UNIDADE_ID = 1; // TODO: vir do contexto do usuário logado

function MovimentacaoPage() {
  const addToast = useContext(ToastContext);
  const { data, loading, refetch } = useApi(() => movimentacaoService.listarAbertas(UNIDADE_ID), []);

  const [modalEntrada, setModalEntrada] = useState(false);
  const [modalSaida,   setModalSaida]   = useState(null);
  const [saving, setSaving] = useState(false);

  const [entradaForm, setEntradaForm] = useState({ placa: '', unidadeId: UNIDADE_ID });
  const [saidaForm,   setSaidaForm]   = useState({ valorCobrado: '', formaPagamento: 'Dinheiro', cpfParaNF: '' });

  const handleEntrada = async () => {
    setSaving(true);
    try {
      await movimentacaoService.registrarEntrada({ ...entradaForm });
      addToast(`Entrada registrada — ${entradaForm.placa}`, 'success');
      setModalEntrada(false);
      setEntradaForm({ placa: '', unidadeId: UNIDADE_ID });
      refetch();
    } catch {
      addToast('Erro ao registrar entrada.', 'error');
    } finally { setSaving(false); }
  };

  const handleSaida = async () => {
    setSaving(true);
    try {
      await movimentacaoService.registrarSaida({
        movimentacaoId: modalSaida.id,
        valorCobrado:   parseFloat(saidaForm.valorCobrado),
        formaPagamento: saidaForm.formaPagamento,
        cpfParaNF:      saidaForm.cpfParaNF || undefined,
      });
      addToast(`Saída registrada — ${modalSaida.placa}`, 'success');
      setModalSaida(null);
      setSaidaForm({ valorCobrado: '', formaPagamento: 'Dinheiro', cpfParaNF: '' });
      refetch();
    } catch {
      addToast('Erro ao registrar saída.', 'error');
    } finally { setSaving(false); }
  };

  const cols = [
    { key: 'ticket',     label: 'Ticket',   render: v => <span className="mono" style={{ color: 'var(--accent)', fontWeight: 600 }}>{v}</span> },
    { key: 'placa',      label: 'Placa',    render: v => <span className="mono" style={{ fontWeight: 600 }}>{v}</span> },
    { key: 'tipoCliente',label: 'Tipo',     render: v => <Badge color={v === 'Mensalista' ? 'var(--success)' : v === 'Convênio' ? 'var(--warning)' : 'var(--accent)'}>{v || 'Avulso'}</Badge> },
    { key: 'dataEntrada',label: 'Entrada',  render: v => formatTime(v) },
    { key: 'dataEntrada',label: 'Tempo',    render: v => formatDuration(v) },
  ];

  return (
    <div className="fade-in">
      <PageHeader
        title="Movimentação"
        subtitle={`${(data || []).length} veículos no pátio`}
        action={<Button onClick={() => setModalEntrada(true)}>＋ Registrar Entrada</Button>}
      />

      <Card>
        {loading ? <Spinner /> : (
          <Table
            columns={cols}
            data={data || []}
            onAction={row => (
              <Button small variant="secondary" onClick={() => setModalSaida(row)}>
                Registrar Saída
              </Button>
            )}
          />
        )}
      </Card>

      {/* Modal Entrada */}
      {modalEntrada && (
        <Modal title="Registrar Entrada de Veículo" onClose={() => setModalEntrada(false)}>
          <FormField label="Placa do Veículo">
            <input
              value={entradaForm.placa}
              onChange={e => setEntradaForm({ ...entradaForm, placa: e.target.value.toUpperCase() })}
              placeholder="ABC-1234"
              className="mono"
              maxLength={8}
            />
          </FormField>
          <FormField label="Unidade">
            <select value={entradaForm.unidadeId} onChange={e => setEntradaForm({ ...entradaForm, unidadeId: +e.target.value })}>
              <option value={1}>SP-001 — Centro</option>
              <option value={2}>SP-002 — Vila Madalena</option>
            </select>
          </FormField>
          <FormField label="ID do Cliente (opcional)">
            <input type="number" placeholder="Deixe vazio para avulso"
              onChange={e => setEntradaForm({ ...entradaForm, clienteId: e.target.value ? +e.target.value : undefined })} />
          </FormField>
          <div style={{ display: 'flex', gap: 10, justifyContent: 'flex-end', marginTop: 8 }}>
            <Button variant="ghost" onClick={() => setModalEntrada(false)}>Cancelar</Button>
            <Button onClick={handleEntrada} disabled={!entradaForm.placa || saving}>
              {saving ? 'Salvando...' : 'Confirmar Entrada'}
            </Button>
          </div>
        </Modal>
      )}

      {/* Modal Saída */}
      {modalSaida && (
        <Modal title={`Registrar Saída — ${modalSaida.placa}`} onClose={() => setModalSaida(null)}>
          <div style={{ background: 'var(--bg)', borderRadius: 8, padding: 14, marginBottom: 20, fontSize: 13, color: 'var(--text-muted)' }}>
            Ticket <strong style={{ color: 'var(--accent)' }}>{modalSaida.ticket}</strong> &nbsp;·&nbsp;
            Entrada: {formatTime(modalSaida.dataEntrada)} &nbsp;·&nbsp;
            Tempo: {formatDuration(modalSaida.dataEntrada)}
          </div>
          <FormField label="Valor Cobrado (R$)">
            <input type="number" step="0.01" min="0"
              value={saidaForm.valorCobrado}
              onChange={e => setSaidaForm({ ...saidaForm, valorCobrado: e.target.value })}
              placeholder="0.00" />
          </FormField>
          <FormField label="Forma de Pagamento">
            <select value={saidaForm.formaPagamento} onChange={e => setSaidaForm({ ...saidaForm, formaPagamento: e.target.value })}>
              {FORMAS_PAGAMENTO.map(f => <option key={f}>{f}</option>)}
            </select>
          </FormField>
          <FormField label="CPF para NF-e (opcional)">
            <input value={saidaForm.cpfParaNF}
              onChange={e => setSaidaForm({ ...saidaForm, cpfParaNF: e.target.value })}
              placeholder="000.000.000-00" className="mono" />
          </FormField>
          <div style={{ display: 'flex', gap: 10, justifyContent: 'flex-end', marginTop: 8 }}>
            <Button variant="ghost" onClick={() => setModalSaida(null)}>Cancelar</Button>
            <Button variant="success" onClick={handleSaida} disabled={saving}>
              {saving ? 'Salvando...' : 'Confirmar Saída'}
            </Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default MovimentacaoPage;
