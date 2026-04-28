import React from 'react';

const NAV_ITEMS = [
  { id: 'dashboard',    icon: '⬡', label: 'Dashboard' },
  { id: 'movimentacao', icon: '🚗', label: 'Movimentação' },
  { id: 'faturamento',  icon: '💰', label: 'Faturamento' },
  { id: 'clientes',     icon: '👥', label: 'Clientes' },
  { id: 'unidades',     icon: '🏢', label: 'Unidades' },
  { id: 'funcionarios', icon: '👤', label: 'Funcionários' },
  { id: 'ponto',        icon: '🕐', label: 'Controle de Ponto' },
  { id: 'contas-pagar', icon: '📋', label: 'Contas a Pagar' },
];

function Sidebar({ currentPage, onNavigate, user, onLogout }) {
  return (
    <aside style={{
      width: 220, background: 'var(--surface)', borderRight: '1px solid var(--border)',
      display: 'flex', flexDirection: 'column', position: 'fixed',
      top: 0, bottom: 0, left: 0, zIndex: 100,
    }}>
      {/* Logo */}
      <div style={{ padding: '22px 20px', borderBottom: '1px solid var(--border)' }}>
        <div style={{ fontSize: 20, fontWeight: 800, letterSpacing: -0.5 }}>
          🅿 Smart<span style={{ color: 'var(--accent)' }}>Park</span>
        </div>
        <div style={{ fontSize: 11, color: 'var(--text-muted)', marginTop: 4, letterSpacing: 0.3 }}>
          Sistema de Gestão
        </div>
      </div>

      {/* Nav */}
      <nav style={{ flex: 1, padding: '12px 10px', overflowY: 'auto' }}>
        {NAV_ITEMS.map(item => {
          const active = currentPage === item.id;
          return (
            <button
              key={item.id}
              onClick={() => onNavigate(item.id)}
              style={{
                display: 'flex', alignItems: 'center', gap: 10,
                width: '100%', padding: '10px 12px', borderRadius: 8,
                background: active ? 'color-mix(in srgb, var(--accent) 12%, transparent)' : 'transparent',
                border: active ? '1px solid color-mix(in srgb, var(--accent) 25%, transparent)' : '1px solid transparent',
                color: active ? 'var(--accent)' : 'var(--text-muted)',
                fontWeight: active ? 600 : 400,
                fontSize: 14, textAlign: 'left', marginBottom: 2,
                transition: 'all 0.15s', cursor: 'pointer',
              }}
            >
              <span style={{ fontSize: 15 }}>{item.icon}</span>
              {item.label}
            </button>
          );
        })}
      </nav>

      {/* User footer */}
      <div style={{ padding: '14px 20px', borderTop: '1px solid var(--border)' }}>
        <div style={{ fontSize: 13, fontWeight: 600, marginBottom: 2, color: 'var(--text)' }}>
          {user?.nome || 'Usuário'}
        </div>
        <div style={{ fontSize: 11, color: 'var(--text-muted)', marginBottom: 8 }}>
          {user?.perfil || 'Operador'}
        </div>
        <button
          onClick={onLogout}
          style={{ background: 'none', border: 'none', color: 'var(--text-muted)', fontSize: 12, cursor: 'pointer', padding: 0 }}
        >
          Sair →
        </button>
      </div>
    </aside>
  );
}

export default Sidebar;
