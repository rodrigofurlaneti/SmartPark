import React, { useEffect } from 'react';

// ─── Spinner ────────────────────────────────────────────────────────────────
export function Spinner({ size = 32 }) {
  return (
    <div style={{ display: 'flex', justifyContent: 'center', padding: 40 }}>
      <div style={{
        width: size, height: size,
        border: '3px solid var(--border)',
        borderTopColor: 'var(--accent)',
        borderRadius: '50%',
        animation: 'spin 0.8s linear infinite',
      }} />
    </div>
  );
}

// ─── Card ────────────────────────────────────────────────────────────────────
export function Card({ children, style }) {
  return (
    <div style={{
      background: 'var(--surface)',
      border: '1px solid var(--border)',
      borderRadius: 12, padding: 24, ...style,
    }}>
      {children}
    </div>
  );
}

// ─── Button ─────────────────────────────────────────────────────────────────
const BTN_VARIANTS = {
  primary:   { background: 'var(--accent)',   color: '#000', border: 'none' },
  secondary: { background: 'transparent',     color: 'var(--accent)',    border: '1px solid var(--accent)' },
  danger:    { background: 'var(--danger)',   color: '#fff', border: 'none' },
  ghost:     { background: 'transparent',     color: 'var(--text-muted)', border: '1px solid var(--border)' },
  success:   { background: 'var(--success)',  color: '#000', border: 'none' },
};

export function Button({ children, onClick, variant = 'primary', small, disabled, style, type = 'button' }) {
  return (
    <button
      type={type}
      onClick={onClick}
      disabled={disabled}
      style={{
        ...BTN_VARIANTS[variant],
        padding: small ? '6px 14px' : '10px 20px',
        borderRadius: 8,
        fontWeight: 600,
        fontSize: small ? 13 : 14,
        opacity: disabled ? 0.5 : 1,
        transition: 'opacity 0.2s, transform 0.1s',
        ...style,
      }}
    >
      {children}
    </button>
  );
}

// ─── Badge ───────────────────────────────────────────────────────────────────
export function Badge({ children, color = 'var(--accent)' }) {
  return (
    <span style={{
      background: color + '20', color, border: `1px solid ${color}40`,
      borderRadius: 6, padding: '3px 10px', fontSize: 12, fontWeight: 600,
    }}>
      {children}
    </span>
  );
}

// ─── StatCard ────────────────────────────────────────────────────────────────
export function StatCard({ icon, label, value, sub, color = 'var(--accent)' }) {
  return (
    <Card style={{ position: 'relative', overflow: 'hidden' }}>
      <div style={{
        position: 'absolute', top: 0, right: 0, width: 80, height: 80,
        background: `radial-gradient(circle at 80% 20%, ${color}18, transparent 70%)`,
      }} />
      <div style={{ fontSize: 26, marginBottom: 8 }}>{icon}</div>
      <div style={{ fontSize: 26, fontWeight: 700, color, letterSpacing: -1 }}>{value}</div>
      <div style={{ fontSize: 13, color: 'var(--text-muted)', marginTop: 4 }}>{label}</div>
      {sub && <div style={{ fontSize: 12, color: 'var(--text-dim)', marginTop: 6 }}>{sub}</div>}
    </Card>
  );
}

// ─── Table ───────────────────────────────────────────────────────────────────
export function Table({ columns, data = [], onAction }) {
  return (
    <div style={{ overflowX: 'auto' }}>
      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: 14 }}>
        <thead>
          <tr style={{ borderBottom: '1px solid var(--border)' }}>
            {columns.map(c => (
              <th key={c.key + c.label} style={{
                textAlign: 'left', padding: '10px 12px',
                color: 'var(--text-muted)', fontWeight: 500, fontSize: 12,
                textTransform: 'uppercase', letterSpacing: 0.5,
              }}>
                {c.label}
              </th>
            ))}
            {onAction && <th style={{ padding: '10px 12px', width: 1 }} />}
          </tr>
        </thead>
        <tbody>
          {data.length === 0 ? (
            <tr><td colSpan={columns.length + (onAction ? 1 : 0)} style={{ textAlign: 'center', padding: 40, color: 'var(--text-muted)' }}>Nenhum registro encontrado.</td></tr>
          ) : data.map((row, i) => (
            <tr key={i}
              style={{ borderBottom: '1px solid color-mix(in srgb, var(--border) 30%, transparent)', transition: 'background 0.15s' }}
              onMouseEnter={e => e.currentTarget.style.background = 'var(--surface-hover)'}
              onMouseLeave={e => e.currentTarget.style.background = 'transparent'}
            >
              {columns.map(c => (
                <td key={c.key + c.label} style={{ padding: '12px', color: c.muted ? 'var(--text-muted)' : 'var(--text)' }}>
                  {c.render ? c.render(row[c.key], row) : (row[c.key] ?? '—')}
                </td>
              ))}
              {onAction && <td style={{ padding: '12px', whiteSpace: 'nowrap' }}>{onAction(row)}</td>}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

// ─── Modal ───────────────────────────────────────────────────────────────────
export function Modal({ title, children, onClose, width = 520 }) {
  useEffect(() => {
    const handler = (e) => { if (e.key === 'Escape') onClose(); };
    window.addEventListener('keydown', handler);
    return () => window.removeEventListener('keydown', handler);
  }, [onClose]);

  return (
    <div
      onClick={e => e.target === e.currentTarget && onClose()}
      style={{
        position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.75)',
        display: 'flex', alignItems: 'center', justifyContent: 'center',
        zIndex: 1000, animation: 'fadeIn 0.2s ease',
      }}
    >
      <div style={{
        background: 'var(--surface)', border: '1px solid var(--border)',
        borderRadius: 16, padding: 28, width: '100%', maxWidth: width,
        maxHeight: '90vh', overflowY: 'auto',
      }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 24 }}>
          <h3 style={{ fontSize: 18, fontWeight: 700 }}>{title}</h3>
          <button onClick={onClose} style={{ background: 'none', border: 'none', color: 'var(--text-muted)', fontSize: 20, lineHeight: 1 }}>✕</button>
        </div>
        {children}
      </div>
    </div>
  );
}

// ─── FormField ───────────────────────────────────────────────────────────────
export function FormField({ label, children, error }) {
  return (
    <div style={{ marginBottom: 16 }}>
      <label style={{ display: 'block', fontSize: 13, color: 'var(--text-muted)', marginBottom: 6, fontWeight: 500 }}>
        {label}
      </label>
      {children}
      {error && <span style={{ fontSize: 12, color: 'var(--danger)', marginTop: 4, display: 'block' }}>{error}</span>}
    </div>
  );
}

// ─── Toast ───────────────────────────────────────────────────────────────────
export function Toast({ toasts, removeToast }) {
  const BG = { success: 'var(--success)', error: 'var(--danger)', warning: 'var(--warning)' };
  const ICON = { success: '✓', error: '✕', warning: '⚠' };
  return (
    <div style={{ position: 'fixed', bottom: 24, right: 24, zIndex: 2000, display: 'flex', flexDirection: 'column', gap: 10 }}>
      {toasts.map(t => (
        <div key={t.id} onClick={() => removeToast(t.id)} style={{
          background: BG[t.type] + '18', border: `1px solid ${BG[t.type]}`,
          color: BG[t.type], borderRadius: 10, padding: '12px 20px',
          fontSize: 14, fontWeight: 600, cursor: 'pointer',
          animation: 'fadeIn 0.3s ease', maxWidth: 340,
        }}>
          {ICON[t.type]} {t.msg}
        </div>
      ))}
    </div>
  );
}

// ─── PageHeader ──────────────────────────────────────────────────────────────
export function PageHeader({ title, subtitle, action }) {
  return (
    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: 28 }}>
      <div>
        <h1 style={{ fontSize: 24, fontWeight: 700, letterSpacing: -0.5 }}>{title}</h1>
        {subtitle && <p style={{ color: 'var(--text-muted)', fontSize: 13, marginTop: 4 }}>{subtitle}</p>}
      </div>
      {action && <div>{action}</div>}
    </div>
  );
}
