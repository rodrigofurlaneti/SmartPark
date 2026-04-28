import React, { useState, useEffect } from 'react';
import LoginPage        from './pages/LoginPage';
import DashboardPage    from './pages/DashboardPage';
import MovimentacaoPage from './pages/MovimentacaoPage';
import FaturamentoPage  from './pages/FaturamentoPage';
import ClientesPage     from './pages/ClientesPage';
import UnidadesPage     from './pages/UnidadesPage';
import FuncionariosPage from './pages/FuncionariosPage';
import ContasPagarPage  from './pages/ContasPagarPage';
import ControlePontoPage from './pages/ControlePontoPage';
import PedidoSeloPage   from './pages/PedidoSeloPage';
import MainLayout       from './components/layout/MainLayout';
import usuarioService   from './services/usuarioService';

const PAGES = {
  dashboard:    <DashboardPage />,
  movimentacao: <MovimentacaoPage />,
  faturamento:  <FaturamentoPage />,
  clientes:     <ClientesPage />,
  unidades:     <UnidadesPage />,
  funcionarios: <FuncionariosPage />,
  ponto:        <ControlePontoPage />,
  'contas-pagar': <ContasPagarPage />,
  'pedido-selo':  <PedidoSeloPage />,
};

function App() {
  const [user,    setUser]    = useState(null);
  const [loading, setLoading] = useState(true);
  const [page,    setPage]    = useState('dashboard');

  // Restaura sessão
  useEffect(() => {
    const stored = localStorage.getItem('smartpark_user');
    if (stored) {
      try { setUser(JSON.parse(stored)); } catch { localStorage.removeItem('smartpark_user'); }
    }
    setLoading(false);
  }, []);

  const handleLogin = async (form) => {
    // usuarioService.login já capitaliza {Login, Senha}
    const res = await usuarioService.login(form);
    // TokenResponseDto: { token, expiracao, usuario }
    const { token, usuario } = res.data;
    localStorage.setItem('smartpark_token', token);
    localStorage.setItem('smartpark_user', JSON.stringify(usuario));
    setUser(usuario);
  };

  const handleLogout = () => {
    localStorage.removeItem('smartpark_token');
    localStorage.removeItem('smartpark_user');
    setUser(null);
    setPage('dashboard');
  };

  if (loading) return null;

  if (!user) return <LoginPage onLogin={handleLogin} />;

  return (
    <MainLayout
      currentPage={page}
      onNavigate={setPage}
      user={user}
      onLogout={handleLogout}
    >
      <div key={page} className="fade-in">
        {PAGES[page] || <DashboardPage />}
      </div>
    </MainLayout>
  );
}

export default App;
