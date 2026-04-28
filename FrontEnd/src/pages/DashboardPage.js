import React from 'react';
import { StatCard, Card, Badge } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import unidadeService from '../services/unidadeService';
import movimentacaoService from '../services/movimentacaoService';
import contasAPagarService from '../services/contasAPagarService';
import faturamentoService from '../services/faturamentoService';
import { formatCurrency, formatDuration } from '../utils/formatters';

function DashboardPage() {
  const inicioMes = new Date(new Date().getFullYear(), new Date().getMonth(), 1).toISOString();

  const { data: unidades, loading: l1 } = useApi(() => unidadeService.listarAtivas(), []);
  const { data: movAberta, loading: l2 } = useApi(() => movimentacaoService.listarAbertas(1), []);
  const { data: contas,    loading: l3 } = useApi(() => contasAPagarService.listarEmAberto(), []);
  const { data: faturas,   loading: l4 } = useApi(() => faturamentoService.listarPorPeriodo(1, inicioMes, new Date().toISOString()), []);

  const loading = l1 || l2 || l3 || l4;
  const totalFat = (faturas || []).reduce((s, f) => s + f.valorTotal, 0);
  const vencidas = (contas   || []).filter(c => new Date(c.dataVencimento) < new Date());

  if (loading) return <Spinner />;

  return (
    <div className="fade-in">
      <div style={{ marginBottom: 28 }}>
        <h1 style={{ fontSize: 26, fontWeight: 700, letterSpacing: -0.5 }}>Dashboard</h1>
        <p style={{ color: 'var(--text-muted)', fontSize: 13, marginTop: 4 }}>
          {new Date().toLocaleDateString('pt-BR', { weekday: 'long', day: 'numeric', month: 'long', year: 'numeric' })}
        </p>
      </div>

      {/* KPIs */}
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: 16, marginBottom: 28 }}>
        <StatCard icon="🏢" label="Unidades ativas" value={(unidades || []).length} color="var(--accent)" />
        <StatCard icon="🚗" label="Veículos no pátio" value={(movAberta || []).length} sub="movimentações abertas" color="var(--success)" />
        <StatCard icon="💰" label="Faturamento (mês)" value={formatCurrency(totalFat)} color="var(--warning)" />
        <StatCard icon="⚠️" label="Contas vencidas" value={vencidas.length} sub={formatCurrency(vencidas.reduce((s, c) => s + c.valorTotal, 0))} color="var(--danger)" />
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
        {/* Pátio */}
        <Card>
          <h3 style={{ fontSize: 15, fontWeight: 600, marginBottom: 16, color: 'var(--text-dim)' }}>
            🚗 Veículos no pátio agora
          </h3>
          {(movAberta || []).length === 0 ? (
            <p style={{ color: 'var(--text-muted)', fontSize: 13 }}>Pátio vazio.</p>
          ) : (movAberta || []).map(m => (
            <div key={m.id} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '10px 0', borderBottom: '1px solid color-mix(in srgb, var(--border) 30%, transparent)' }}>
              <div>
                <span className="mono" style={{ fontWeight: 600, color: 'var(--accent)' }}>{m.placa}</span>
                <span style={{ fontSize: 12, color: 'var(--text-muted)', marginLeft: 10 }}>#{m.ticket}</span>
              </div>
              <div style={{ display: 'flex', gap: 8, alignItems: 'center' }}>
                <Badge color={m.tipoCliente === 'Mensalista' ? 'var(--success)' : m.tipoCliente === 'Convênio' ? 'var(--warning)' : 'var(--accent)'}>
                  {m.tipoCliente || 'Avulso'}
                </Badge>
                <span style={{ fontSize: 12, color: 'var(--text-muted)' }}>{formatDuration(m.dataEntrada)}</span>
              </div>
            </div>
          ))}
        </Card>

        {/* Últimos faturamentos */}
        <Card>
          <h3 style={{ fontSize: 15, fontWeight: 600, marginBottom: 16, color: 'var(--text-dim)' }}>
            💰 Últimos faturamentos
          </h3>
          {(faturas || []).slice(0, 5).map(f => (
            <div key={f.id} style={{ padding: '10px 0', borderBottom: '1px solid color-mix(in srgb, var(--border) 30%, transparent)' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                <span style={{ fontSize: 13 }}>Terminal {f.numTerminal} — Fech. #{f.numFechamento}</span>
                <strong style={{ color: 'var(--success)' }}>{formatCurrency(f.valorTotal)}</strong>
              </div>
              <div style={{ fontSize: 12, color: 'var(--text-muted)', marginTop: 3 }}>
                💵 {formatCurrency(f.valorDinheiro)} · 💳 {formatCurrency(f.valorCartaoDebito + f.valorCartaoCredito)}
              </div>
            </div>
          ))}
        </Card>
      </div>
    </div>
  );
}

export default DashboardPage;
