import React from 'react';
import Sidebar from './Sidebar';
import { Toast } from '../ui';
import useToast from '../../hooks/useToast';

export const ToastContext = React.createContext(null);

function MainLayout({ currentPage, onNavigate, user, onLogout, children }) {
  const { toasts, addToast, removeToast } = useToast();

  return (
    <ToastContext.Provider value={addToast}>
      <div style={{ display: 'flex', minHeight: '100vh', background: 'var(--parchment)' }}>
        <Sidebar
          currentPage={currentPage}
          onNavigate={onNavigate}
          user={user}
          onLogout={onLogout}
        />
        <main style={{
          flex: 1,
          marginLeft: 'var(--sidebar-w)',
          padding: '40px 48px',
          minHeight: '100vh',
          background: 'var(--parchment)',
          maxWidth: 'calc(100% - var(--sidebar-w))',
        }}>
          {children}
        </main>
        <Toast toasts={toasts} removeToast={removeToast} />
      </div>
    </ToastContext.Provider>
  );
}

export default MainLayout;
