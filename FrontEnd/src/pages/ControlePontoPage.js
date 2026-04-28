import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, PageHeader } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import controlePontoService from '../services/controlePontoService';
import { formatDateTime } from '../utils/formatters';
import { ToastContext } from '../components/layout/MainLayout';

function ControlePontoPage() {
  const addToast = useContext(ToastContext);
  const [funcId, setFuncId] = useState(1);
  const { data, loading, refetch } = useApi(() => controlePontoService.listarPorFuncionario(funcId), [funcId]);
  const [modal, setModal]   = useState(false);
  const [saving, setSaving] = useState(false);
  const [form, setForm]     = useState({ funcionarioId: 1, tipo: 'Entrada' });

  const handleRegistrar = async () => {
    setSaving(true);
    try {
      await controlePontoService.registrarPonto(form);
      addToast('Ponto registrado!', 'success');
      setModal(false);
      refetch();
    } catch { addToast('Erro ao registrar ponto.', 'error'); }
    finally { setSaving(false); }
  };

  return (
    <div className="fade-in">
      <PageHeader
        title="Controle de Ponto"
        subtitle="Registro de jornada dos funcionários"
        action={<Button onClick={() => setModal(true)}>＋ Registrar Ponto</Button>}
      />

      <div style={{ display: 'flex', gap: 12, marginBottom: 16, alignItems: 'center' }}>
        <label style={{ color: 'var(--text-muted)', fontSize: 13 }}>Funcionário ID:</label>
        <input type="number" value={funcId} onChange={e => setFuncId(+e.target.value)}
          style={{ width: 100 }} />
      </div>

      <Card>
        {loading ? <Spinner /> : (
          <Table
            columns={[
              { key: 'id',            label: 'ID',         render: v => <span className="mono" style={{ color: 'var(--accent)' }}>{v}</span> },
              { key: 'dataHora',      label: 'Data/Hora',  render: v => formatDateTime(v) },
              { key: 'tipo',          label: 'Tipo' },
              { key: 'funcionarioId', label: 'Func. ID',   muted: true },
            ]}
            data={data || []}
          />
        )}
      </Card>

      {modal && (
        <Modal title="Registrar Ponto" onClose={() => setModal(false)}>
          <FormField label="ID do Funcionário">
            <input type="number" value={form.funcionarioId} onChange={e => setForm({ ...form, funcionarioId: +e.target.value })} />
          </FormField>
          <FormField label="Tipo">
            <select value={form.tipo} onChange={e => setForm({ ...form, tipo: e.target.value })}>
              <option>Entrada</option>
              <option>Saída</option>
              <option>Intervalo Início</option>
              <option>Intervalo Fim</option>
            </select>
          </FormField>
          <div style={{ display: 'flex', gap: 10, justifyContent: 'flex-end', marginTop: 8 }}>
            <Button variant="ghost" onClick={() => setModal(false)}>Cancelar</Button>
            <Button onClick={handleRegistrar} disabled={saving}>{saving ? 'Registrando...' : 'Registrar'}</Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default ControlePontoPage;
