import { useContext, useState } from "react"
import { useHttp } from "../../../hooks/http-hook"
import { AuthContext } from "../../../context/AuthContext"
import { NavLink } from "react-router-dom"
import { t } from "i18next"
import { useTranslation } from "react-i18next";
import { NotificationManager } from "react-notifications"

export default function CreateCategoryPage () {
    const {t} = useTranslation()
    const {request, loading, error} = useHttp()
    const {token} = useContext(AuthContext)
    const [success, setSuccess] = useState(false)
    const [form, setForm] = useState({
        name: '',
        description: '',
        imageUrl: '123123'
    })

    function changeHandler(event){
        setForm({...form, [event.target.name]: event.target.value})
    }

    async function registerHandler(){
        try{
            if(form.name.length < 2) {
                NotificationManager.error(t('Name should be at least 2 characters!'))
                return;
            }
            NotificationManager.info(t('Creation is in progress...'))
            const data = await request('/api/category/', 'POST', {...form}, {"Authorization": 'Bearer '+ token})
            setSuccess(true)
            NotificationManager.success(t('Added!'))
        }catch(e){
            NotificationManager.error(e.message)
        }
    }
    
    return (<div className="create-category-page">
        
            <button className="create-category-page-button"><NavLink to={'/admin/listcategory'}>{t('Back')}</NavLink></button>
        
        <div>
            {t('Name')}
            <input type={'text'} value={form.name} name={"name"} onChange={changeHandler}/>
        </div>
        <div>
            {t('Description')}
             <input value={form.description} name={"description"} onChange={changeHandler}/>
        </div>
        <div>
            <button className="add-category" onClick={registerHandler}>{t('Add category')}</button>
        </div>
    </div>)
};