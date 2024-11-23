import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import { BrowserRouter } from 'react-router-dom';
import './i18n'
import {NotificationContainer} from 'react-notifications'
import 'react-notifications/lib/notifications.css';
if(localStorage.getItem('lang') === null) {
  localStorage.setItem('lang', 'en')
}

ReactDOM.render(
  <React.StrictMode>
    <NotificationContainer/>
    <BrowserRouter>
    <App />
    </BrowserRouter>
  </React.StrictMode>,
  document.getElementById('root')
);