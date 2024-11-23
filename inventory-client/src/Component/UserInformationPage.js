import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { t } from "i18next"
import { useTranslation } from "react-i18next";

const UserInformationPage = () => {
  const {t} = useTranslation()
  const [user, setUser] = useState({});
  const navigate = useNavigate();

  useEffect(() => {
    // Fetch user information from API
    fetch('/api/user')
      .then(response => response.json())
      .then(data => setUser(data))
      .catch(error => console.log(error));
  }, []);

  const handleEditClick = () => {
    // Redirect to Edit User Information page
    navigate.push('/edit-user');
  }

  return (
    <div className="user-information-page">
      <h2>{t('User Information')}</h2>
      <div>
        <img src={user.profile_picture} alt="Profile" />
        <h3>{user.name}</h3>
        <p>{t('Email:')} {user.email}</p>
        <p>{t('Phone:')} {user.phone}</p>
        <button onClick={handleEditClick}>{t('Edit')}</button>
      </div>
    </div>
  );
};

export default UserInformationPage;