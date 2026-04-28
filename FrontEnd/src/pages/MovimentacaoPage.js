import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, Badge, PageHeader } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import movimentacaoService from '../services/movimentacaoService';
import { FORMAS_PAGAMENTO } from '../utils/constants';
import { formatTime, formatDuration } from '../utils/formatters';
import { ToastContext } from '../components/layout/MainLayout';

const EMPRESA_ID = 1;
const UNIDADE_ID = 1;

function MovimentacaoPage() {
  const addToast = useContext(ToastContext);
  const { data, loading, refetch } = useApi(
    () => movimentacaoService.listarAbertas(UNIDADE_ID), []
  );

  const [modalEntrada, setModalEntrada] = useState(false);
  const [modalSaida,   setModalSaida]   = useState(null);
  const [saving, setSaving] = useState(false);

  const [entradaForm, setEntradaForm] = useState({
    placa: '', unidadeId: UNIDADE_ID, empresaId: EMPRESA_ID,
    clienteId: undefined, numeroContrato: undefined,
  });
  const [saidaForm, setSaidaForm] = useState({
    valorCobrado: '', formaPagamento: 'Dinheiro', cpfParaNF: '',
  });

  const handleEntrada = async () => {
    if (!entradaForm.placa.trim()) { addToast('Informe a placa do veículo.', 'warning'); return; }
    setSaving(true);
    try {
      await movimentacaoService.registrarEntrada(entradaForm);
      addToast(`Entrada registrada — ${entradaForm.placa}`, 'success');
      setModalEntrada(false);
      setEntradaForm({ placa: '', unidadeId: UNIDADE_ID, empresaId: EMPRESA_ID });
      refetch();
    } catch (e) {
      addToast(e.response?.data?.erro || 'Erro ao registrar entrada.', 'error');
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
    } catch (e) {
      addToast(e.response?.data?.erro || 'Erro ao registrar saída.', 'error');
    } finally { setSaving(false); }
  };

  const cols = [
    {
      key: 'placa', label: 'Placa',
      render: v => <span className="mono" style={{ fontWeight: 600, fontSize: 13, color: 'var(--ink)' }}>{v}</span>,
    },
    {
      key: 'isMensalista', label: 'Tipo',
      render: (v, row) => (
        <Badge color={v ? 'var(--success)' : row.clienteId ? 'var(--warning)' : 'var(--action-blue)'}>
          {v ? 'Mensalista' : row.clienteId ? 'Convênio' : 'Avulso'}
        </Badge>
      ),
    },
    { key: 'dataEntrada', label: 'Entrada',  render: v => formatTime(v) },
    { key: 'dataEntrada', label: 'Tempo',    render: v => formatDuration(v) },
  ];

  return (
    <div className="fade-in">
      <PageHeader
        title="Movimentação"
        subtitle={`${(data || []).length} veículos no pátio`}
        action={
          <Button onClick={() => setModalEntrada(true)}>＋ Registrar Entrada</Button>
        }
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
              onChange={e => setEntradaForm({ ...entradaForm, placa: e.target.value.toUpperCase().replace(/[^A-Z0-9]/g, '') })}
              placeholder="ABC1234"
              className="mono"
              maxLength={7}
              autoFocus
            />
          </FormField>
          <FormField label="ID do Cliente (opcional)">
            <input
              type="number"
              placeholder="Deixe vazio para avulso"
              onChange={e => setEntradaForm({
                ...entradaForm,
                clienteId: e.target.value ? +e.target.value : undefined,
              })}
            />
          </FormField>
          <FormField label="Nº do Contrato (opcional)">
            <input
              placeholder="Apenas para mensalistas"
              onChange={e => setEntradaForm({
                ...entradaForm,
                numeroContrato: e.target.value || undefined,
              })}
            />
          </FormField>
          <div style={{ display: 'flex', gap: 12, justifyContent: 'flex-end', marginTop: 24 }}>
            <Button variant="ghost" onClick={() => setModalEntrada(false)}>Cancelar</Button>
            <Button onClick={handleEntrada} disabled={!entradaForm.placa || saving}>
              {saving ? 'Registrando…' : 'Confirmar Entrada'}
            </Button>
          </div>
        </Modal>
      )}

      {/* Modal Saída */}
      {modalSaida && (
        <Modal title={`Registrar Saída — ${modalSaida.placa}`} onClose={() => setModalSaida(null)}>
          <div style={{
            background: 'var(--parchment)', borderRadius: 12,
            padding: '14px 18px', marginBottom: 24,
            fontSize: 14, color: 'var(--ink-48)',
            letterSpacing: '-0.224px',
          }}>
            Entrada: <strong style={{ color: 'var(--ink)' }}>{formatTime(modalSaida.dataEntrada)}</strong>
            &nbsp;·&nbsp;
            Tempo: <strong style={{ color: 'var(--action-blue)' }}>{formatDuration(modalSaida.dataEntrada)}</strong>
          </div>
          <FormField label="Valor Cobrado (R$)">
            <input
              type="number" step="0.01" min="0"
              value={saidaForm.valorCobrado}
              onChange={e => setSaidaForm({ ...saidaForm, valorCobrado: e.target.value })}
              placeholder="0,00"
              autoFocus
            />
          </FormField>
          <FormField label="Forma de Pagamento">
            <select
              value={saidaForm.formaPagamento}
              onChange={e => setSaidaForm({ ...saidaForm, formaPagamento: e.target.value })}
            >
              {FORMAS_PAGAMENTO.map(f => <option key={f}>{f}</option>)}
            </select>
          </FormField>
          <FormField label="CPF para NF-e (opcional)">
            <input
              value={saidaForm.cpfParaNF}
              onChange={e => setSaidaForm({ ...saidaForm, cpfParaNF: e.target.value })}
              placeholder="000.000.000-00"
              className="mono"
            />
          </FormField>
          <div style={{ display: 'flex', gap: 12, justifyContent: 'flex-end', marginTop: 24 }}>
            <Button variant="ghost" onClick={() => setModalSaida(null)}>Cancelar</Button>
            <Button variant="success" onClick={handleSaida} disabled={!saidaForm.valorCobrado || saving}>
              {saving ? 'Processando…' : 'Confirmar Saída'}
            </Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default MovimentacaoPage;
