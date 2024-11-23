import React, { useContext, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { AuthContext } from "../../context/AuthContext";
import { FaBars, FaTimes } from "react-icons/fa";
import logo from "./aitu.png";
import i18n from "i18next";
import { useTranslation } from "react-i18next";



const NavigationBar = () => {
    const { isAuth, logout } = useContext(AuthContext);
    const navigate = useNavigate();
    const [isOpen, setIsOpen] = useState(false);
    const { t } = useTranslation();
    const [language, setLanguage] = useState(localStorage.getItem('lang'));

    const handleMenuClick = () => {
        setIsOpen(!isOpen);
    };

    const handleLogoutClick = () => {
        logout();
        navigate("/");
    };

    const handleLanguageChange = (e) => {
        if(localStorage.getItem('lang') === 'en') {
            localStorage.setItem('lang', 'ru');
            i18n.changeLanguage('ru', (err, t) => {
                if (err) return console.log(err);
            });
            setLanguage('ru');
        } else {
            localStorage.setItem('lang', 'en');
            i18n.changeLanguage('en', (err, t) => {
                if (err) return console.log(err);
            });
            setLanguage('en');
        }
    };

    return (
        <>
            <div className="navbar">
                <div className="navbar-container">
                    <div className="navbar-logo">
                        <Link to="admin/adminpanel">
                        <img src={logo} alt="AITU Logo"/>
                        </Link>
                    </div>
                    <div className="navbar-menu-icon" onClick={handleMenuClick}>
                        {isOpen ? <FaTimes /> : <FaBars />}
                    </div>
                    <div className={`navbar-links ${isOpen ? "active" : ""}`}>
                        <ul>
                            {isAuth ? (
                                <>
                                    <li onClick={handleLogoutClick}>
                                        {t("Logout")}
                                    </li>
                                    <li onClick={() => navigate("/scan")}>
                                        {t("Scan")}
                                    </li>
                                    <li onClick={() => navigate('/userinfo')}>
                                        {t("Profile")}
                                    </li>
                                </>
                            ) : (
                                <>
                                    <li onClick={() => navigate("/")}>
                                        {t("Log In")}
                                    </li>
                                </>
                            )}
                            <li onClick={() => navigate("/aboutus")}>
                                {t("About Us")}
                            </li>
                            <li>
                                <select value={language} onChange={handleLanguageChange}>
                                    <option value="en">{t("Eng")}</option>
                                    <option value="ru">{t("Rus")}</option>
                                </select>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </>
    );
};

export default NavigationBar;