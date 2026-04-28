import React, { useState } from 'react';
import { Button, FormField } from '../components/ui';

function LoginPage({ onLogin }) {
  const [form,    setForm]    = useState({ login: '', senha: '' });
  const [loading, setLoading] = useState(false);
  const [error,   setError]   = useState('');

  const handleSubmit = async (e) => {
    e?.preventDefault();
    if (!form.login || !form.senha) { setError('Preencha login e senha.'); return; }
    setLoading(true);
    setError('');
    try {
      await onLogin(form);
    } catch (err) {
      setError(
        err.response?.data?.erro ||
        err.response?.data?.message ||
        'Credenciais inválidas.'
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{
      minHeight: '100vh',
      display: 'flex',
      background: 'var(--parchment)',
    }}>
      {/* Left — dark hero tile */}
      <div style={{
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        width: '45%',
        background: 'var(--tile-dark-1)',
        padding: '80px 60px',
        color: '#ffffff',
      }}>
        <div style={{ textAlign: 'center', maxWidth: 360 }}>
          <div style={{
            fontSize: 48, fontWeight: 600, letterSpacing: '-0.28px',
            lineHeight: 1.07, marginBottom: 16,
          }}>
            Smart<span style={{ color: 'var(--action-blue)' }}>Park</span>
          </div>
          <p style={{
            fontSize: 21, fontWeight: 400, letterSpacing: '0.231px',
            color: 'rgba(255,255,255,0.60)', lineHeight: 1.19,
          }}>
            Sistema de Gestão de Estacionamentos
          </p>
          <div style={{
            width: 48, height: 2,
            background: 'var(--action-blue)',
            borderRadius: 980,
            margin: '32px auto 0',
          }} />
        </div>
      </div>

      {/* Right — login form */}
      <div style={{
        flex: 1,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        padding: '60px 48px',
      }}>
        <div style={{ width: '100%', maxWidth: 400 }} className="fade-in">
          <h2 style={{
            fontSize: 34, fontWeight: 600, letterSpacing: '-0.374px',
            color: 'var(--ink)', marginBottom: 8,
          }}>
            Entrar
          </h2>
          <p style={{ color: 'var(--ink-48)', fontSize: 17, marginBottom: 36 }}>
            Acesse sua conta para continuar.
          </p>

          {error && (
            <div style={{
              background: 'rgba(255, 59, 48, 0.08)',
              border: '1px solid rgba(255, 59, 48, 0.30)',
              color: 'var(--danger)',
              borderRadius: 12,
              padding: '12px 16px',
              fontSize: 14,
              marginBottom: 24,
              letterSpacing: '-0.224px',
            }}>
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit}>
            <FormField label="Login">
              <input
                value={form.login}
                onChange={e => setForm({ ...form, login: e.target.value })}
                placeholder="seu.usuario"
                autoFocus
                autoComplete="username"
              />
            </FormField>
            <FormField label="Senha">
              <input
                type="password"
                value={form.senha}
                onChange={e => setForm({ ...form, senha: e.target.value })}
                placeholder="••••••••"
                autoComplete="current-password"
              />
            </FormField>

            <Button
              type="submit"
              style={{ width: '100%', marginTop: 12, fontSize: 17 }}
              disabled={loading}
            >
              {loading ? 'Autenticando…' : 'Entrar'}
            </Button>
          </form>
        </div>
      </div>
    </div>
  );
}

export default LoginPage;
