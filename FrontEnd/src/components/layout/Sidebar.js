import React from 'react';

const NAV_ITEMS = [
  { id: 'dashboard',    icon: '⊞',  label: 'Dashboard' },
  { id: 'movimentacao', icon: '🚗',  label: 'Movimentação' },
  { id: 'faturamento',  icon: '💰',  label: 'Faturamento' },
  { id: 'clientes',     icon: '👥',  label: 'Clientes' },
  { id: 'unidades',     icon: '🏢',  label: 'Unidades' },
  { id: 'funcionarios', icon: '👤',  label: 'Funcionários' },
  { id: 'ponto',        icon: '🕐',  label: 'Controle de Ponto' },
  { id: 'contas-pagar', icon: '📋',  label: 'Contas a Pagar' },
  { id: 'pedido-selo',  icon: '🏷',  label: 'Pedidos de Selo' },
];

function Sidebar({ currentPage, onNavigate, user, onLogout }) {
  return (
    <aside style={{
      width: 'var(--sidebar-w)',
      background: 'var(--pure-black)',
      display: 'flex',
      flexDirection: 'column',
      position: 'fixed',
      top: 0, bottom: 0, left: 0,
      zIndex: 100,
    }}>
      {/* Logo */}
      <div style={{
        padding: '22px 24px 20px',
        borderBottom: '1px solid var(--hairline-dark)',
      }}>
        <div style={{
          fontSize: 20, fontWeight: 600, letterSpacing: '-0.374px',
          color: '#ffffff',
        }}>
          Smart<span style={{ color: 'var(--action-blue)' }}>Park</span>
        </div>
        <div style={{
          fontSize: 11, color: 'rgba(255,255,255,0.4)', marginTop: 4,
          letterSpacing: '0.3px', textTransform: 'uppercase',
        }}>
          Sistema de Gestão
        </div>
      </div>

      {/* Nav */}
      <nav style={{ flex: 1, padding: '14px 12px', overflowY: 'auto' }}>
        {NAV_ITEMS.map(item => {
          const active = currentPage === item.id;
          return (
            <button
              key={item.id}
              onClick={() => onNavigate(item.id)}
              style={{
                display: 'flex', alignItems: 'center', gap: 10,
                width: '100%', padding: '9px 14px', borderRadius: 8,
                background: active ? 'rgba(255,255,255,0.10)' : 'transparent',
                border: 'none',
                color: active ? '#ffffff' : 'rgba(255,255,255,0.50)',
                fontWeight: active ? 600 : 400,
                fontSize: 14, letterSpacing: '-0.224px',
                textAlign: 'left', marginBottom: 2,
                transition: 'background 0.15s, color 0.15s',
                cursor: 'pointer',
              }}
              onMouseEnter={e => {
                if (!active) e.currentTarget.style.background = 'rgba(255,255,255,0.06)';
                if (!active) e.currentTarget.style.color = 'rgba(255,255,255,0.80)';
              }}
              onMouseLeave={e => {
                if (!active) e.currentTarget.style.background = 'transparent';
                if (!active) e.currentTarget.style.color = 'rgba(255,255,255,0.50)';
              }}
            >
              <span style={{ fontSize: 15, opacity: active ? 1 : 0.7 }}>{item.icon}</span>
              {item.label}
              {active && (
                <span style={{
                  marginLeft: 'auto', width: 5, height: 5,
                  borderRadius: '50%', background: 'var(--action-blue)',
                  flexShrink: 0,
                }} />
              )}
            </button>
          );
        })}
      </nav>

      {/* User footer */}
      <div style={{
        padding: '16px 24px',
        borderTop: '1px solid var(--hairline-dark)',
      }}>
        <div style={{ fontSize: 14, fontWeight: 600, color: '#ffffff', marginBottom: 2, letterSpacing: '-0.224px' }}>
          {user?.login || user?.nome || 'Usuário'}
        </div>
        <div style={{ fontSize: 11, color: 'rgba(255,255,255,0.4)', marginBottom: 12, letterSpacing: '0.2px' }}>
          Empresa #{user?.empresa_Id || '—'}
        </div>
        <button
          onClick={onLogout}
          style={{
            background: 'rgba(255,255,255,0.08)',
            border: '1px solid rgba(255,255,255,0.12)',
            borderRadius: 8,
            color: 'rgba(255,255,255,0.60)',
            fontSize: 12,
            padding: '6px 14px',
            cursor: 'pointer',
            transition: 'background 0.15s',
          }}
          onMouseEnter={e => e.currentTarget.style.background = 'rgba(255,255,255,0.14)'}
          onMouseLeave={e => e.currentTarget.style.background = 'rgba(255,255,255,0.08)'}
        >
          Sair →
        </button>
      </div>
    </aside>
  );
}

export default Sidebar;
