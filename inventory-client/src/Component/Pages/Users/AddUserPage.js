import { useContext, useState } from "react"
import { useHttp } from "../../../hooks/http-hook"
import { AuthContext } from "../../../context/AuthContext"
import { NavLink } from "react-router-dom"
import { t } from "i18next"
import { useTranslation } from "react-i18next";
import { NotificationManager } from "react-notifications"

export default function AddUserPage () {
    const {t} = useTranslation()
    const {request, loading, error} = useHttp()
    const {token} = useContext(AuthContext)
    const [success, setSuccess] = useState(false)
    const [form, setForm] = useState({
        email: '',
        fullName: '',
        role: '1'
    })

    function changeHandler(event){
        setForm({...form, [event.target.name]: event.target.value.trim()})
    }

    async function registerHandler(){
        try{
            NotificationManager.info(t('Loading...'))
            const data = await request('/api/admin/', 'POST', {...form}, {"Authorization": 'Bearer '+ token})
            setSuccess(true)
            NotificationManager.success(t('User was added!'))
        }catch(e){}
    }
    
    return (<div className="add-user-page">
        <button className="add-user-page-button"><NavLink to={'/admin/listusers'}>{t('Back')}</NavLink></button>
    <div className="form-container">
        <h2>{t('Add User')}</h2>
        <div className="form-input">
            <label htmlFor="email">{t('Email')}</label>
            <input className="form-input1" type="email" value={form.email} name="email" onChange={changeHandler}/>
        </div>
        <div className="form-input">
            <label htmlFor="fullName">{t('Full name')}</label>
            <input className="form-input1" value={form.fullName} name="fullName" onChange={changeHandler}/>
        </div>
        {/* <div className="form-input">
            <label htmlFor="password">{t('Password')}</label>
            <input className="form-input1" type="password" value={form.password} name="password" onChange={changeHandler}/>
        </div> */}
        <div className="form-input">
            <label htmlFor="role">{t('Role:')}</label>
            <select onChange={changeHandler} value={form.role} name="role">
                <option value="1">
                    {t('Admin')}
                </option>
                <option value="2">
                    {t('Moderator')}
                </option>
            </select>
        </div>
        <div className="form-input">
            <button className="add-user-page-add-user" onClick={registerHandler}>{t('Add user')}</button>
        </div>
        {success && <div className="success-message"><b>{t('Credentials were sent to the email! (Check spam box)')}</b></div>}
    </div>
</div>)
};