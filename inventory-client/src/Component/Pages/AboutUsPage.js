import React from 'react';
import { t } from "i18next"
import { useTranslation } from "react-i18next";
import logo from './aitu.png';

const AboutUs = () => {
  const {t} = useTranslation()
    return (
      <div className="about-us-container">
        <h1 className="about-us-title">{t('About Us')}</h1>
        <div className="about-us-content">
          <p className="about-us-text">
            {t('The management of inventory in university housing is complicated by the arrangement and monitoring of resources across various rooms and facilities. The purpose of this project is to develop a client-server application that addresses these problems in a user-friendly and efficient manner. The application makes use of cutting-edge tools including ReactJS, ASP.NET, PostgreSQL, and AWS for hosting.')}
          </p>
          <p className='about-us-text'>
            {t('The appealing and straightforward user interface of the front-end component allows users to add, edit, and remove classes and commodities. The back-end component, which also interacts with the database and ensures efficient data retrieval and storage, is in charge of handling the business logic. The front-end and back-end components are connected through HTTP calls, allowing for real-time modifications and a seamless user experience.')}
          </p>
        </div>
        <div className="about-us-logo-container">
        <img className="about-us-logo" src={logo} alt="Logo" />
      </div>
      </div>
    );
  };
  
  export default AboutUs;