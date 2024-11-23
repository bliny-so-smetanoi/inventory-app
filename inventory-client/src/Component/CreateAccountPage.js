import React, { useState } from 'react';
import { t } from "i18next"
import { useTranslation } from "react-i18next";

const CreateAccountPage = () => {
  const {t} = useTranslation()
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [phone, setPhone] = useState('');

  const handleSubmit = (event) => {
    event.preventDefault();
    // add code to submit form data to backend
  };

  return (
    <div className="create-account-page">
      <h2>{t('Create Account')}</h2>
      <form onSubmit={handleSubmit}>
        <label htmlFor="name">{t('Name')}</label>
        <input type="text" id="name" value={name} onChange={(e) => setName(e.target.value)} />
        <label htmlFor="email">{t('Email')}</label>
        <input type="email" id="email" value={email} onChange={(e) => setEmail(e.target.value)} />
        <label htmlFor="password">{t('Password')}</label>
        <input type="password" id="password" value={password} onChange={(e) => setPassword(e.target.value)} />
        <label htmlFor="phone">{t('Phone')}</label>
        <input type="tel" id="phone" value={phone} onChange={(e) => setPhone(e.target.value)} />
        <button type="submit">{t('Create Account')}</button>
      </form>
    </div>
  );
};

export default CreateAccountPage;
