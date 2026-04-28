import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, Badge, PageHeader } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import clienteService from '../services/clienteService';
import { formatDate, formatDoc } from '../utils/formatters';
import { ToastContext } from '../components/layout/MainLayout';

const EMPRESA_ID = 1;

function ClientesPage() {
  const addToast = useContext(ToastContext);
  const { data, loading, refetch } = useApi(() => clienteService.listarTodos(), []);
  const [modal,  setModal]  = useState(false);
  const [search, setSearch] = useState('');
  const [saving, setSaving] = useState(false);
  const [form,   setForm]   = useState({ nome: '', documento: '', isMensalista: false, empresaId: EMPRESA_ID });

  const filtered = (data || []).filter(c =>
    (c.nome           || '').toLowerCase().includes(search.toLowerCase()) ||
    (c.documentoNumero|| '').includes(search.replace(/\D/g, ''))
  );

  const handleCriar = async () => {
    if (!form.nome || !form.documento) { addToast('Nome e documento são obrigatórios.', 'warning'); return; }
    setSaving(true);
    try {
      await clienteService.criar(form);
      addToast('Cliente cadastrado com sucesso!', 'success');
      setModal(false);
      setForm({ nome: '', documento: '', isMensalista: false, empresaId: EMPRESA_ID });
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao cadastrar cliente.', 'error'); }
    finally { setSaving(false); }
  };

  const handleInativar = async (id, nome) => {
    if (!window.confirm(`Inativar o cliente "${nome}"?`)) return;
    try {
      await clienteService.inativar(id);
      addToast('Cliente inativado.', 'success');
      refetch();
    } catch (e) { addToast(e.response?.data?.erro || 'Erro ao inativar.', 'error'); }
  };

  const ativos      = (data || []).filter(c => c.ativo).length;
  const mensalist   = (data || []).filter(c => c.isMensalista).length;

  return (
    <div className="fade-in">
      <PageHeader
        title="Clientes"
        subtitle={`${ativos} ativos · ${mensalist} mensalistas`}
        action={<Button onClick={() => setModal(true)}>＋ Novo Cliente</Button>}
      />

      <div style={{ marginBottom: 20 }}>
        <input
          value={search}
          onChange={e => setSearch(e.target.value)}
          placeholder="Buscar por nome ou documento…"
          style={{ maxWidth: 400 }}
        />
      </div>

      <Card>
        {loading ? <Spinner /> : (
          <Table
            columns={[
              { key: 'nome', label: 'Nome' },
              {
                key: 'documentoNumero', label: 'Documento',
                render: v => <span className="mono">{formatDoc(v)}</span>,
              },
              {
                key: 'isMensalista', label: 'Tipo',
                render: v => <Badge color={v ? 'var(--success)' : 'var(--action-blue)'}>{v ? 'Mensalista' : 'Avulso'}</Badge>,
              },
              {
                key: 'ativo', label: 'Status',
                render: v => <Badge color={v ? 'var(--success)' : 'var(--danger)'}>{v ? 'Ativo' : 'Inativo'}</Badge>,
              },
              { key: 'dataInsercao', label: 'Cadastro', render: v => formatDate(v), muted: true },
            ]}
            data={filtered}
            onAction={row => row.ativo ? (
              <Button small variant="ghost" onClick={() => handleInativar(row.id, row.nome)}>
                Inativar
              </Button>
            ) : null}
          />
        )}
      </Card>

      {modal && (
        <Modal title="Novo Cliente" onClose={() => setModal(false)}>
          <FormField label="Nome completo / Razão Social">
            <input
              value={form.nome}
              onChange={e => setForm({ ...form, nome: e.target.value })}
              placeholder="Nome do cliente"
              autoFocus
            />
          </FormField>
          <FormField label="CPF (11 dígitos) ou CNPJ (14 dígitos)">
            <input
              value={form.documento}
              onChange={e => setForm({ ...form, documento: e.target.value.replace(/\D/g, '') })}
              placeholder="Apenas números"
              className="mono"
              maxLength={14}
            />
          </FormField>
          <FormField label="Tipo de cliente">
            <select
              value={form.isMensalista}
              onChange={e => setForm({ ...form, isMensalista: e.target.value === 'true' })}
            >
              <option value={false}>Avulso</option>
              <option value={true}>Mensalista</option>
            </select>
          </FormField>
          <div style={{ display: 'flex', gap: 12, justifyContent: 'flex-end', marginTop: 24 }}>
            <Button variant="ghost" onClick={() => setModal(false)}>Cancelar</Button>
            <Button
              onClick={handleCriar}
              disabled={!form.nome || !form.documento || saving}
            >
              {saving ? 'Salvando…' : 'Cadastrar'}
            </Button>
          </div>
        </Modal>
      )}
    </div>
  );
}

export default ClientesPage;
