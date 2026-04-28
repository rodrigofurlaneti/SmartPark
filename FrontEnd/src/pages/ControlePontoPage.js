import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, PageHeader, Badge } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import controlePontoService from '../services/controlePontoService';
import { formatDateTime } from '../utils/formatters';
import { ToastContext } from '../components/layout/MainLayout';

const EMPRESA_ID = 1;
const TIPO_COLOR = { Entrada: 'var(--success)', 'Saída': 'var(--danger)', 'Intervalo Início': 'var(--warning)', 'Intervalo Fim': 'var(--action-blue)' };

function ControlePontoPage() {
  const addToast = useContext(ToastContext);
  const [funcId, setFuncId] = useState('');
  const { data, loading, refetch } = useApi(
    () => funcId
      ? controlePontoService.listarPorFuncionario(+funcId)
      : controlePontoService.listarTodos(),
    [funcId]
  );

  const [modal,  setModal]  = useState(false);
  const [saving, setSaving] = useState(false);
  const [form,   setForm]   = useState({ funcionarioId: '', tipo: 'Entrada', empresaId: EMPRESA_ID });

  const handleRegistrar = async () => {
    if (!form.funcionarioId) { addToast('Informe o ID do funcionário.', 'warning'); return; }
    setSaving(true);
    try {
      await controlePontoService.registrarPonto({ ...form, funcionarioId: +form.funcionarioId });
      addToast('Ponto registrado!', 'success');
      setModal(false);
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao registrar ponto.', 'error'); }
    finally { setSaving(false); }
  };

  return (
    <div className="fade-in">
      <PageHeader
        title="Controle de Ponto"
        subtitle="Registro de jornada dos funcionários"
        action={<Button onClick={() => setModal(true)}>＋ Registrar Ponto</Button>}
      />

      {/* Filtro por funcionário */}
      <div style={{ display: 'flex', gap: 12, alignItems: 'center', marginBottom: 20 }}>
        <div style={{ flex: '0 0 300px' }}>
          <input
            type="number"
            value={funcId}
            onChange={e => setFuncId(e.target.value)}
            placeholder="Filtrar por ID do funcionário…"
          />
        </div>
        {funcId && (
          <Button small variant="ghost" onClick={() => setFuncId('')}>
            Limpar filtro
          </Button>
        )}
      </div>

      <Card>
        {loading ? <Spinner /> : (
          <Table
            columns={[
              {
                key: 'id', label: 'ID',
                render: v => <span className="mono" style={{ color: 'var(--action-blue)', fontWeight: 600 }}>{v}</span>,
              },
              { key: 'dataHora', label: 'Data / Hora', render: v => formatDateTime(v) },
              {
                key: 'tipo', label: 'Tipo',
                render: v => <Badge color={TIPO_COLOR[v] || 'var(--action-blue)'}>{v}</Badge>,
              },
              { key: 'funcionario_Id', label: 'Func. ID', muted: true },
            ]}
            data={data || []}
          />
        )}
      </Card>

      {modal && (
        <Modal title="Registrar Ponto" onClose={() => setModal(false)}>
          <FormField label="ID do Funcionário">
            <input
              type="number"
              value={form.funcionarioId}
              onChange={e => setForm({ ...form, funcionarioId: e.target.value })}
              placeholder="ID do funcionário"
              autoFocus
            />
          </FormField>
          <FormField label="Tipo de Registro">
            <select value={form.tipo} onChange={e => setForm({ ...form, tipo: e.target.value })}>
              <option>Entrada</option>
              <option>Saída</option>
              <option>Intervalo Início</option>
              <option>Intervalo Fim</option>
            </select>
          </FormField>
          <div style={{ display: 'flex', gap: 12, justifyContent: 'flex-end', marginTop: 24 }}>
            <Button variant="ghost" onClick={() => setModal(false)}>Cancelar</Button>
            <Button onClick={handleRegistrar} disabled={!form.funcionarioId || saving}>
              {saving ? 'Registrando…' : 'Registrar'}
            </Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default ControlePontoPage;
