import React, { useEffect } from 'react';

// ─── Spinner ─────────────────────────────────────────────────────────────────
export function Spinner({ size = 28 }) {
  return (
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', padding: 48 }}>
      <div style={{
        width: size, height: size,
        border: '2.5px solid var(--hairline)',
        borderTopColor: 'var(--action-blue)',
        borderRadius: '50%',
        animation: 'spin 0.75s linear infinite',
      }} />
    </div>
  );
}

// ─── Card ────────────────────────────────────────────────────────────────────
// Apple utility card: Pure White, 18px radius, hairline border, no shadow
export function Card({ children, style, onMouseEnter, onMouseLeave }) {
  return (
    <div
      onMouseEnter={onMouseEnter}
      onMouseLeave={onMouseLeave}
      style={{
        background: 'var(--white)',
        border: '1px solid var(--hairline)',
        borderRadius: 18,
        padding: '24px 28px',
        transition: 'border-color 0.18s',
        ...style,
      }}
    >
      {children}
    </div>
  );
}

// ─── Button ──────────────────────────────────────────────────────────────────
// Pill = primary / secondary; rect (8px) = utility
const VARIANTS = {
  primary: {
    background: 'var(--action-blue)',
    color: '#ffffff',
    border: 'none',
    borderRadius: 980,
    fontSize: 17,
    fontWeight: 400,
    padding: '11px 22px',
  },
  secondary: {
    background: 'transparent',
    color: 'var(--action-blue)',
    border: '1px solid var(--action-blue)',
    borderRadius: 980,
    fontSize: 17,
    fontWeight: 400,
    padding: '11px 22px',
  },
  utility: {
    background: 'var(--ink)',
    color: '#ffffff',
    border: 'none',
    borderRadius: 8,
    fontSize: 14,
    fontWeight: 400,
    letterSpacing: '-0.224px',
    padding: '8px 15px',
  },
  ghost: {
    background: 'var(--pearl)',
    color: 'var(--ink-80)',
    border: '3px solid var(--divider)',
    borderRadius: 11,
    fontSize: 14,
    fontWeight: 400,
    padding: '7px 14px',
  },
  danger: {
    background: 'var(--danger)',
    color: '#ffffff',
    border: 'none',
    borderRadius: 980,
    fontSize: 17,
    fontWeight: 400,
    padding: '11px 22px',
  },
  success: {
    background: 'var(--success)',
    color: '#ffffff',
    border: 'none',
    borderRadius: 980,
    fontSize: 17,
    fontWeight: 400,
    padding: '11px 22px',
  },
};

export function Button({ children, onClick, variant = 'primary', small, disabled, style, type = 'button' }) {
  const base = VARIANTS[variant] || VARIANTS.primary;
  return (
    <button
      type={type}
      onClick={onClick}
      disabled={disabled}
      style={{
        display: 'inline-flex',
        alignItems: 'center',
        justifyContent: 'center',
        gap: 6,
        lineHeight: 1,
        cursor: disabled ? 'not-allowed' : 'pointer',
        ...(small
          ? { ...base, padding: '7px 16px', fontSize: Math.max((base.fontSize || 17) - 3, 12) }
          : base),
        ...style,
      }}
    >
      {children}
    </button>
  );
}

// ─── Badge ───────────────────────────────────────────────────────────────────
// Apple chip — translucent colored background
export function Badge({ children, color = 'var(--action-blue)' }) {
  return (
    <span style={{
      display: 'inline-block',
      background: color + '18',
      color,
      border: `1px solid ${color}30`,
      borderRadius: 980,
      padding: '3px 12px',
      fontSize: 12,
      fontWeight: 600,
      letterSpacing: '0.1px',
      whiteSpace: 'nowrap',
    }}>
      {children}
    </span>
  );
}

// ─── StatCard ────────────────────────────────────────────────────────────────
export function StatCard({ icon, label, value, sub, color = 'var(--action-blue)' }) {
  return (
    <Card>
      <div style={{ fontSize: 22, marginBottom: 10 }}>{icon}</div>
      <div style={{
        fontSize: 28, fontWeight: 600, color, letterSpacing: '-0.374px', lineHeight: 1.1,
      }}>
        {value}
      </div>
      <div style={{ fontSize: 14, color: 'var(--ink-48)', marginTop: 6, letterSpacing: '-0.224px' }}>
        {label}
      </div>
      {sub && (
        <div style={{ fontSize: 12, color: 'var(--ink-48)', marginTop: 4 }}>{sub}</div>
      )}
    </Card>
  );
}

// ─── Table ───────────────────────────────────────────────────────────────────
export function Table({ columns, data = [], onAction }) {
  return (
    <div style={{ overflowX: 'auto', borderRadius: 12 }}>
      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: 14 }}>
        <thead>
          <tr style={{ borderBottom: '1px solid var(--hairline)' }}>
            {columns.map((c, i) => (
              <th key={i} style={{
                textAlign: 'left', padding: '10px 16px',
                color: 'var(--ink-48)', fontWeight: 600, fontSize: 11,
                textTransform: 'uppercase', letterSpacing: '0.6px',
              }}>
                {c.label}
              </th>
            ))}
            {onAction && <th style={{ padding: '10px 16px', width: 1 }} />}
          </tr>
        </thead>
        <tbody>
          {data.length === 0 ? (
            <tr>
              <td
                colSpan={columns.length + (onAction ? 1 : 0)}
                style={{ textAlign: 'center', padding: 48, color: 'var(--ink-48)', fontSize: 14 }}
              >
                Nenhum registro encontrado.
              </td>
            </tr>
          ) : data.map((row, i) => (
            <tr
              key={i}
              style={{
                borderBottom: i < data.length - 1 ? '1px solid var(--hairline)' : 'none',
                transition: 'background 0.12s',
              }}
              onMouseEnter={e => e.currentTarget.style.background = 'var(--parchment)'}
              onMouseLeave={e => e.currentTarget.style.background = 'transparent'}
            >
              {columns.map((c, j) => (
                <td key={j} style={{
                  padding: '13px 16px',
                  color: c.muted ? 'var(--ink-48)' : 'var(--ink)',
                  fontSize: 14,
                  letterSpacing: '-0.224px',
                  verticalAlign: 'middle',
                }}>
                  {c.render ? c.render(row[c.key], row) : (row[c.key] ?? '—')}
                </td>
              ))}
              {onAction && (
                <td style={{ padding: '13px 16px', whiteSpace: 'nowrap', textAlign: 'right', verticalAlign: 'middle' }}>
                  {onAction(row)}
                </td>
              )}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

// ─── Modal ───────────────────────────────────────────────────────────────────
export function Modal({ title, children, onClose, width = 540 }) {
  useEffect(() => {
    const handler = (e) => { if (e.key === 'Escape') onClose(); };
    window.addEventListener('keydown', handler);
    return () => window.removeEventListener('keydown', handler);
  }, [onClose]);

  return (
    <div
      onClick={e => e.target === e.currentTarget && onClose()}
      style={{
        position: 'fixed', inset: 0,
        background: 'rgba(0, 0, 0, 0.44)',
        backdropFilter: 'blur(4px)',
        display: 'flex', alignItems: 'center', justifyContent: 'center',
        zIndex: 1000,
        animation: 'fadeIn 0.2s ease',
        padding: 20,
      }}
    >
      <div style={{
        background: 'var(--white)',
        border: '1px solid var(--hairline)',
        borderRadius: 20,
        padding: '28px 32px',
        width: '100%', maxWidth: width,
        maxHeight: '90vh', overflowY: 'auto',
        boxShadow: '0 24px 64px rgba(0,0,0,0.16)',
      }}>
        {/* Header */}
        <div style={{
          display: 'flex', justifyContent: 'space-between', alignItems: 'center',
          marginBottom: 24,
        }}>
          <h3 style={{ fontSize: 21, fontWeight: 600, letterSpacing: '0.231px', color: 'var(--ink)' }}>
            {title}
          </h3>
          <button
            onClick={onClose}
            style={{
              background: 'rgba(210, 210, 215, 0.64)',
              border: 'none', borderRadius: '50%',
              width: 32, height: 32,
              display: 'flex', alignItems: 'center', justifyContent: 'center',
              color: 'var(--ink)', fontSize: 14, fontWeight: 600,
              cursor: 'pointer',
            }}
          >
            ✕
          </button>
        </div>
        {children}
      </div>
    </div>
  );
}

// ─── FormField ───────────────────────────────────────────────────────────────
export function FormField({ label, children, error }) {
  return (
    <div style={{ marginBottom: 18 }}>
      <label style={{
        display: 'block', fontSize: 12, fontWeight: 600,
        color: 'var(--ink-48)', marginBottom: 8,
        letterSpacing: '0.3px', textTransform: 'uppercase',
      }}>
        {label}
      </label>
      {children}
      {error && (
        <span style={{
          fontSize: 12, color: 'var(--danger)', marginTop: 6, display: 'block',
        }}>
          {error}
        </span>
      )}
    </div>
  );
}

// ─── Toast ───────────────────────────────────────────────────────────────────
const TOAST_COLORS = {
  success: 'var(--success)',
  error:   'var(--danger)',
  warning: 'var(--warning)',
};
const TOAST_ICONS = { success: '✓', error: '✕', warning: '⚠' };

export function Toast({ toasts, removeToast }) {
  return (
    <div style={{
      position: 'fixed', bottom: 28, right: 28, zIndex: 2000,
      display: 'flex', flexDirection: 'column', gap: 10,
    }}>
      {toasts.map(t => {
        const c = TOAST_COLORS[t.type] || TOAST_COLORS.success;
        return (
          <div
            key={t.id}
            onClick={() => removeToast(t.id)}
            style={{
              background: 'var(--white)',
              border: `1px solid ${c}`,
              borderLeft: `4px solid ${c}`,
              borderRadius: 12,
              padding: '12px 18px',
              fontSize: 14, fontWeight: 500,
              cursor: 'pointer',
              animation: 'fadeIn 0.25s ease',
              maxWidth: 360,
              display: 'flex', gap: 10, alignItems: 'center',
              boxShadow: '0 4px 24px rgba(0,0,0,0.10)',
              color: 'var(--ink)',
            }}
          >
            <span style={{ color: c, fontWeight: 700 }}>{TOAST_ICONS[t.type]}</span>
            {t.msg}
          </div>
        );
      })}
    </div>
  );
}

// ─── PageHeader ──────────────────────────────────────────────────────────────
export function PageHeader({ title, subtitle, action }) {
  return (
    <div style={{
      display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start',
      marginBottom: 32,
    }}>
      <div>
        <h1 style={{ fontSize: 34, fontWeight: 600, letterSpacing: '-0.374px', lineHeight: 1.2, color: 'var(--ink)' }}>
          {title}
        </h1>
        {subtitle && (
          <p style={{ color: 'var(--ink-48)', fontSize: 14, marginTop: 6, letterSpacing: '-0.224px' }}>
            {subtitle}
          </p>
        )}
      </div>
      {action && <div style={{ marginTop: 4 }}>{action}</div>}
    </div>
  );
}
