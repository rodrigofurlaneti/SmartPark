import React, { useState, useContext } from 'react';
import { Card, Button, Table, Modal, FormField, Badge, PageHeader } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import clienteService from '../services/clienteService';
import { formatDate, formatDoc } from '../utils/formatters';
import { ToastContext } from '../components/layout/MainLayout';

function ClientesPage() {
    const addToast = useContext(ToastContext);
    const { data, loading, refetch } = useApi(() => clienteService.listarTodos(), []);
    const [modal, setModal] = useState(false);
    const [search, setSearch] = useState('');
    const [saving, setSaving] = useState(false);
    const [form, setForm] = useState({ nome: '', documento: '', isMensalista: false });

    // ✅ Corrigido — null-safe com || ''
    const filtered = (data || []).filter(c =>
        (c.nome || '').toLowerCase().includes(search.toLowerCase()) ||
        (c.documentoNumero || '').includes(search)
    );

    const handleCriar = async () => {
        setSaving(true);
        try {
            await clienteService.criar(form);
            addToast('Cliente cadastrado com sucesso!', 'success');
            setModal(false);
            setForm({ nome: '', documento: '', isMensalista: false });
            refetch();
        } catch { addToast('Erro ao cadastrar cliente.', 'error'); }
        finally { setSaving(false); }
    };

    const handleInativar = async (id, nome) => {
        if (!window.confirm(`Inativar ${nome}?`)) return;
        try {
            await clienteService.inativar(id);
            addToast('Cliente inativado.', 'success');
            refetch();
        } catch { addToast('Erro ao inativar.', 'error'); }
    };

    const ativos = (data || []).filter(c => c.ativo).length;
    const mensalist = (data || []).filter(c => c.isMensalista).length;

    return (
        <div className="fade-in">
            <PageHeader
                title="Clientes"
                subtitle={`${ativos} ativos · ${mensalist} mensalistas`}
                action={<Button onClick={() => setModal(true)}>＋ Novo Cliente</Button>}
            />

            <div style={{ marginBottom: 16 }}>
                <input
                    value={search}
                    onChange={e => setSearch(e.target.value)}
                    placeholder="🔍  Buscar por nome ou documento…"
                    style={{ maxWidth: 380 }}
                />
            </div>

            <Card>
                {loading ? <Spinner /> : (
                    <Table
                        columns={[
                            { key: 'nome', label: 'Nome' },
                            {
                                key: 'documentoNumero', label: 'Documento',
                                render: v => <span className="mono">{formatDoc(v)}</span>
                            },
                            {
                                key: 'isMensalista', label: 'Tipo',
                                render: v => <Badge color={v ? 'var(--success)' : 'var(--accent)'}>{v ? 'Mensalista' : 'Avulso'}</Badge>
                            },
                            {
                                key: 'ativo', label: 'Status',
                                render: v => <Badge color={v ? 'var(--success)' : 'var(--danger)'}>{v ? 'Ativo' : 'Inativo'}</Badge>
                            },
                            {
                                key: 'dataInsercao', label: 'Cadastro',
                                render: v => formatDate(v), muted: true
                            },
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
                        />
                    </FormField>
                    <FormField label="CPF / CNPJ">
                        <input
                            value={form.documento}
                            onChange={e => setForm({ ...form, documento: e.target.value })}
                            placeholder="000.000.000-00"
                            className="mono"
                        />
                    </FormField>
                    <FormField label="Tipo">
                        <select
                            value={form.isMensalista}
                            onChange={e => setForm({ ...form, isMensalista: e.target.value === 'true' })}
                        >
                            <option value={false}>Avulso</option>
                            <option value={true}>Mensalista</option>
                        </select>
                    </FormField>
                    <div style={{ display: 'flex', gap: 10, justifyContent: 'flex-end', marginTop: 8 }}>
                        <Button variant="ghost" onClick={() => setModal(false)}>Cancelar</Button>
                        <Button onClick={handleCriar} disabled={!form.nome || !form.documento || saving}>
                            {saving ? 'Salvando...' : 'Cadastrar'}
                        </Button>
                    </div>
                </Modal>
            )}
        </div>
    );
}

export default ClientesPage;