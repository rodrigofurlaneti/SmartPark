import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, Badge, PageHeader } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import funcionarioService from '../services/funcionarioService';
import { formatDate, formatCurrency } from '../utils/formatters';
import { ESCALAS } from '../utils/constants';
import { ToastContext } from '../components/layout/MainLayout';

const EMPRESA_ID = 1;
const ESCALA_COLOR = { Diurno: 'var(--warning)', Noturno: 'var(--action-blue)', '12x36': 'var(--success)' };

function FuncionariosPage() {
  const addToast = useContext(ToastContext);
  const { data, loading, refetch } = useApi(() => funcionarioService.listarAtivos(), []);
  const [modal,  setModal]  = useState(false);
  const [saving, setSaving] = useState(false);
  const [form,   setForm]   = useState({ pessoaId: '', salario: '', escala: 'Diurno', unidadeId: 1, empresaId: EMPRESA_ID });

  const handleCriar = async () => {
    if (!form.pessoaId || !form.salario) { addToast('Preencha todos os campos.', 'warning'); return; }
    setSaving(true);
    try {
      await funcionarioService.criar({
        pessoaId:  +form.pessoaId,
        salario:   +form.salario,
        escala:    form.escala,
        unidadeId: +form.unidadeId,
        empresaId: EMPRESA_ID,
      });
      addToast('Funcionário cadastrado!', 'success');
      setModal(false);
      setForm({ pessoaId: '', salario: '', escala: 'Diurno', unidadeId: 1, empresaId: EMPRESA_ID });
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao cadastrar.', 'error'); }
    finally { setSaving(false); }
  };

  const handleDesligar = async (id, codigo) => {
    if (!window.confirm(`Desligar funcionário ${codigo || id}?`)) return;
    try {
      await funcionarioService.desligar(id);
      addToast('Funcionário desligado.', 'success');
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro.', 'error'); }
  };

  const handleSalario = async (id) => {
    const v = window.prompt('Novo salário (R$):');
    if (!v || isNaN(+v)) return;
    try {
      await funcionarioService.alterarSalario(id, +v);
      addToast('Salário atualizado.', 'success');
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro.', 'error'); }
  };

  return (
    <div className="fade-in">
      <PageHeader
        title="Funcionários"
        subtitle={`${(data || []).length} ativos`}
        action={<Button onClick={() => setModal(true)}>＋ Novo Funcionário</Button>}
      />

      <Card>
        {loading ? <Spinner /> : (
          <Table
            columns={[
              {
                key: 'codigo', label: 'Código',
                render: v => <span className="mono" style={{ color: 'var(--action-blue)', fontWeight: 600 }}>{v}</span>,
              },
              { key: 'tipoEscala', label: 'Escala', render: v => <Badge color={ESCALA_COLOR[v] || 'var(--action-blue)'}>{v}</Badge> },
              { key: 'salario',    label: 'Salário', render: v => <span style={{ color: 'var(--success)', fontWeight: 600 }}>{formatCurrency(v)}</span> },
              { key: 'dataAdmissao', label: 'Admissão', render: v => formatDate(v), muted: true },
              { key: 'unidade_Id',   label: 'Unidade',  render: v => v ? `Unidade ${v}` : '—', muted: true },
            ]}
            data={data || []}
            onAction={row => (
              <div style={{ display: 'flex', gap: 8 }}>
                <Button small variant="ghost" onClick={() => handleSalario(row.id)}>Salário</Button>
                <Button small variant="ghost" onClick={() => handleDesligar(row.id, row.codigo)}>Desligar</Button>
              </div>
            )}
          />
        )}
      </Card>

      {modal && (
        <Modal title="Novo Funcionário" onClose={() => setModal(false)}>
          <FormField label="ID da Pessoa (sistema)">
            <input type="number" value={form.pessoaId}
              onChange={e => setForm({ ...form, pessoaId: e.target.value })}
              placeholder="ID cadastrado no sistema" autoFocus />
          </FormField>
          <FormField label="Salário (R$)">
            <input type="number" step="0.01" value={form.salario}
              onChange={e => setForm({ ...form, salario: e.target.value })} placeholder="2500,00" />
          </FormField>
          <FormField label="Escala de Trabalho">
            <select value={form.escala} onChange={e => setForm({ ...form, escala: e.target.value })}>
              {ESCALAS.map(e => <option key={e}>{e}</option>)}
            </select>
          </FormField>
          <FormField label="Unidade">
            <select value={form.unidadeId} onChange={e => setForm({ ...form, unidadeId: +e.target.value })}>
              <option value={1}>Unidade 1 — Centro</option>
              <option value={2}>Unidade 2 — Vila Madalena</option>
            </select>
          </FormField>
          <div style={{ display: 'flex', gap: 12, justifyContent: 'flex-end', marginTop: 24 }}>
            <Button variant="ghost" onClick={() => setModal(false)}>Cancelar</Button>
            <Button onClick={handleCriar} disabled={!form.pessoaId || !form.salario || saving}>
              {saving ? 'Salvando…' : 'Cadastrar'}
            </Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default FuncionariosPage;
