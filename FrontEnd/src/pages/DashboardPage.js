import React from 'react';
import { StatCard, Card, Badge } from '../components/ui';
import { Spinner } from '../components/ui';
import useApi from '../hooks/useApi';
import unidadeService     from '../services/unidadeService';
import movimentacaoService from '../services/movimentacaoService';
import contasAPagarService from '../services/contasAPagarService';
import faturamentoService  from '../services/faturamentoService';
import { formatCurrency, formatDuration } from '../utils/formatters';

function DashboardPage() {
  const inicioMes = new Date(new Date().getFullYear(), new Date().getMonth(), 1).toISOString();
  const fim       = new Date().toISOString();

  const { data: unidades, loading: l1 } = useApi(() => unidadeService.listarAtivas(),         []);
  const { data: movAberta, loading: l2 } = useApi(() => movimentacaoService.listarAbertas(1),  []);
  const { data: contas,    loading: l3 } = useApi(() => contasAPagarService.listarEmAberto(),  []);
  const { data: faturas,   loading: l4 } = useApi(() => faturamentoService.listarPorPeriodo(1, inicioMes, fim), []);

  const loading = l1 || l2 || l3 || l4;

  const totalFat = (faturas  || []).reduce((s, f) => s + (f.valorTotal || 0), 0);
  const vencidas = (contas   || []).filter(c => new Date(c.dataVencimento) < new Date());

  if (loading) return <Spinner />;

  return (
    <div className="fade-in">
      {/* Header */}
      <div style={{ marginBottom: 36 }}>
        <h1 style={{ fontSize: 40, fontWeight: 600, letterSpacing: '-0.28px', color: 'var(--ink)' }}>
          Dashboard
        </h1>
        <p style={{ color: 'var(--ink-48)', fontSize: 14, marginTop: 6, letterSpacing: '-0.224px' }}>
          {new Date().toLocaleDateString('pt-BR', {
            weekday: 'long', day: 'numeric', month: 'long', year: 'numeric',
          })}
        </p>
      </div>

      {/* KPIs */}
      <div style={{
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))',
        gap: 16, marginBottom: 32,
      }}>
        <StatCard
          icon="🏢" label="Unidades ativas"
          value={(unidades || []).length}
          color="var(--action-blue)"
        />
        <StatCard
          icon="🚗" label="Veículos no pátio"
          value={(movAberta || []).length}
          sub="movimentações abertas"
          color="var(--success)"
        />
        <StatCard
          icon="💰" label="Faturamento (mês)"
          value={formatCurrency(totalFat)}
          color="var(--warning)"
        />
        <StatCard
          icon="⚠️" label="Contas vencidas"
          value={vencidas.length}
          sub={formatCurrency(vencidas.reduce((s, c) => s + (c.valorTotal || 0), 0))}
          color="var(--danger)"
        />
      </div>

      {/* Detail cards */}
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
        {/* Pátio */}
        <Card>
          <h3 style={{ fontSize: 17, fontWeight: 600, marginBottom: 20, color: 'var(--ink)', letterSpacing: '-0.374px' }}>
            Veículos no pátio agora
          </h3>
          {(movAberta || []).length === 0 ? (
            <p style={{ color: 'var(--ink-48)', fontSize: 14 }}>Pátio vazio.</p>
          ) : (movAberta || []).map(m => (
            <div key={m.id} style={{
              display: 'flex', justifyContent: 'space-between', alignItems: 'center',
              padding: '11px 0',
              borderBottom: '1px solid var(--hairline)',
            }}>
              <div>
                <span className="mono" style={{ fontWeight: 600, color: 'var(--ink)', fontSize: 14 }}>{m.placa}</span>
                <span style={{ fontSize: 12, color: 'var(--ink-48)', marginLeft: 10 }}>#{m.ticket}</span>
              </div>
              <div style={{ display: 'flex', gap: 8, alignItems: 'center' }}>
                <Badge color={m.isMensalista ? 'var(--success)' : 'var(--action-blue)'}>
                  {m.isMensalista ? 'Mensalista' : 'Avulso'}
                </Badge>
                <span style={{ fontSize: 12, color: 'var(--ink-48)' }}>{formatDuration(m.dataEntrada)}</span>
              </div>
            </div>
          ))}
        </Card>

        {/* Últimos faturamentos */}
        <Card>
          <h3 style={{ fontSize: 17, fontWeight: 600, marginBottom: 20, color: 'var(--ink)', letterSpacing: '-0.374px' }}>
            Últimos faturamentos
          </h3>
          {(faturas || []).length === 0 ? (
            <p style={{ color: 'var(--ink-48)', fontSize: 14 }}>Nenhum faturamento no período.</p>
          ) : (faturas || []).slice(0, 5).map(f => (
            <div key={f.id} style={{
              padding: '11px 0',
              borderBottom: '1px solid var(--hairline)',
            }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <span style={{ fontSize: 14, color: 'var(--ink)', letterSpacing: '-0.224px' }}>
                  Terminal {f.numTerminal} — Fech. #{f.numFechamento}
                </span>
                <strong style={{ color: 'var(--success)', fontSize: 14 }}>
                  {formatCurrency(f.valorTotal)}
                </strong>
              </div>
              <div style={{ fontSize: 12, color: 'var(--ink-48)', marginTop: 4 }}>
                Dinheiro {formatCurrency(f.valorDinheiro)} · Cartão {formatCurrency((f.valorCartaoDebito || 0) + (f.valorCartaoCredito || 0))}
              </div>
            </div>
          ))}
        </Card>
      </div>
    </div>
  );
}

export default DashboardPage;
