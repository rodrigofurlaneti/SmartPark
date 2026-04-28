import React from 'react';
import Sidebar from './Sidebar';
import { Toast } from '../ui';
import useToast from '../../hooks/useToast';

export const ToastContext = React.createContext(null);

function MainLayout({ currentPage, onNavigate, user, onLogout, children }) {
  const { toasts, addToast, removeToast } = useToast();

  return (
    <ToastContext.Provider value={addToast}>
      <div style={{ display: 'flex', minHeight: '100vh' }}>
        <Sidebar currentPage={currentPage} onNavigate={onNavigate} user={user} onLogout={onLogout} />
        <main style={{ flex: 1, marginLeft: 220, padding: 32, minHeight: '100vh', background: 'var(--bg)' }}>
          {children}
        </main>
        <Toast toasts={toasts} removeToast={removeToast} />
      </div>
    </ToastContext.Provider>
  );
}

export default MainLayout;
