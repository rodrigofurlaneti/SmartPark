import React, { useState, useContext } from 'react';
import { Card, Button, FormField } from '../components/ui';

function LoginPage({ onLogin }) {
  const [form, setForm] = useState({ login: '', senha: '' });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e?.preventDefault();
    if (!form.login || !form.senha) { setError('Preencha login e senha.'); return; }
    setLoading(true); setError('');
    try {
      await onLogin(form);
    } catch (err) {
      setError(err.response?.data?.message || 'Credenciais inválidas.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{
      minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center',
      background: 'var(--bg)', position: 'relative', overflow: 'hidden',
    }}>
      {/* Glow bg */}
      <div style={{
        position: 'absolute', width: 700, height: 700, borderRadius: '50%',
        background: 'radial-gradient(circle, color-mix(in srgb, var(--accent) 6%, transparent), transparent 65%)',
        top: '50%', left: '50%', transform: 'translate(-50%,-50%)', pointerEvents: 'none',
      }} />

      <div style={{ width: '100%', maxWidth: 400, padding: 20 }} className="fade-in">
        <div style={{ textAlign: 'center', marginBottom: 40 }}>
          <div style={{ fontSize: 52, marginBottom: 10 }}>🅿</div>
          <h1 style={{ fontSize: 30, fontWeight: 800, letterSpacing: -1 }}>
            Smart<span style={{ color: 'var(--accent)' }}>Park</span>
          </h1>
          <p style={{ color: 'var(--text-muted)', marginTop: 6, fontSize: 14 }}>
            Sistema de Gestão de Estacionamentos
          </p>
        </div>

        <Card>
          {error && (
            <div style={{
              background: 'color-mix(in srgb, var(--danger) 12%, transparent)',
              border: '1px solid color-mix(in srgb, var(--danger) 40%, transparent)',
              color: 'var(--danger)', borderRadius: 8, padding: '10px 14px',
              fontSize: 13, marginBottom: 16,
            }}>
              {error}
            </div>
          )}

          <FormField label="Login">
            <input
              value={form.login}
              onChange={e => setForm({ ...form, login: e.target.value })}
              placeholder="seu.usuario"
              onKeyDown={e => e.key === 'Enter' && handleSubmit()}
              autoFocus
            />
          </FormField>
          <FormField label="Senha">
            <input
              type="password"
              value={form.senha}
              onChange={e => setForm({ ...form, senha: e.target.value })}
              placeholder="••••••••"
              onKeyDown={e => e.key === 'Enter' && handleSubmit()}
            />
          </FormField>

          <Button
            style={{ width: '100%', marginTop: 8 }}
            onClick={handleSubmit}
            disabled={loading}
          >
            {loading ? 'Autenticando...' : 'Entrar'}
          </Button>
        </Card>
      </div>
    </div>
  );
}

export default LoginPage;
