import React, { useContext, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../../../context/AuthContext';
import NavigationBar from '../../PageComponents/Navigationbar';
import { useHttp } from '../../../hooks/http-hook';
import { t } from "i18next"
import { useTranslation } from "react-i18next";
import {NotificationManager} from 'react-notifications';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const {loading, request, error, clearError} = useHttp();
  const [message, setMessage] = useState('')
  const auth = useContext(AuthContext)
  const {t} = useTranslation()
  const navigate = useNavigate()
  const handleEmailChange = (event) => {
    setEmail(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handleLoginSubmit = async (event) => {
    try {
      event.preventDefault();
      NotificationManager.info(t('Loading...'))
      const data = await request('/api/admin/identity', 'POST', {email, password})
  
      auth.login(data.token, data.token, data.role);
      navigate('admin/adminpanel');
      NotificationManager.success(t('Welcome!'))
    } catch(e) {
      NotificationManager.error(t('Incorrect credentials data!'))
      console.log(e.message);
    }
    
  };

  return (
    <>
    
    <div className="container login-container">
      <form className="login-form" onSubmit={handleLoginSubmit}>
        <h2>{t('Login')}</h2>
        <label>{t('Email:')}</label>
        <input placeholder='Email' className="login-input" type="email" value={email} onChange={handleEmailChange} required />
        <label>{t('Password:')}</label>
        <input placeholder={t('Password')} className="login-input" type="password" value={password} onChange={handlePasswordChange} required />
        <button disabled={loading} className="login-button" type="submit">{!loading && t('Login')} {loading && t('Loading...')}</button>
      </form>
      <div>
        <b className="login-message">{message}</b>
      </div>
    </div>
    </>
  );
};

export default Login;