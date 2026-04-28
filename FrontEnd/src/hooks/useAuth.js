import { useState, useEffect } from 'react';
import usuarioService from '../services/usuarioService';

export function useAuth() {
  const [user,    setUser]    = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const stored = localStorage.getItem('smartpark_user');
    if (stored) setUser(JSON.parse(stored));
    setLoading(false);
  }, []);

  const login = async (loginDto) => {
    const res = await usuarioService.login(loginDto);
    const { token, usuario } = res.data;
    localStorage.setItem('smartpark_token', token);
    localStorage.setItem('smartpark_user', JSON.stringify(usuario));
    setUser(usuario);
    return usuario;
  };

  const logout = () => {
    localStorage.removeItem('smartpark_token');
    localStorage.removeItem('smartpark_user');
    setUser(null);
  };

  return { user, loading, login, logout, isAuthenticated: !!user };
}

export default useAuth;
