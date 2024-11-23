import { useContext, useState } from "react"
import { useHttp } from "../../../hooks/http-hook"
import { AuthContext } from "../../../context/AuthContext"
import { NavLink } from "react-router-dom"
import { t } from "i18next"
import { useTranslation } from "react-i18next";
import { NotificationManager } from "react-notifications"

export default function CreateClassPage () {
    const {t} = useTranslation()
    const {request, loading, error} = useHttp()
    const {token} = useContext(AuthContext)
    const [success, setSuccess] = useState(false)
    const [form, setForm] = useState({
        classroomName: '',
        classroomDescription: '',
        iconUrl: '123123'
    })

    function changeHandler(event){
        setForm({...form, [event.target.name]: event.target.value})
    }

    async function registerHandler(){
        try{
            if(form.classroomName.length < 2) {
                NotificationManager.error(t('Name should be at least 2 characters!'))
                return;
            }
            NotificationManager.info(t('Classroom creation is in progress...'))
            const data = await request('/api/classroom/', 'POST', {...form}, {"Authorization": 'Bearer '+ token})
            setSuccess(true)
            NotificationManager.success(t('Classroom was added!'))
        }catch(e){}
    }
    
    return (
        <div className="create-class-page">
            <button className="create-class-page-button"><NavLink to={'/admin/listclass'}>{t('Back')}</NavLink></button>
            <div>
                {t('Name')}
                <input type={'text'} value={form.classroomName} name={"classroomName"} onChange={changeHandler}/>
            </div>
            <div>
                {t('Description')}
                 <input value={form.classroomDescription} name={"classroomDescription"} onChange={changeHandler}/>
            </div>
            <div>
                <button className="add-classroom" onClick={registerHandler}>{t('Add classroom')}</button>
            </div>
            {success && <div><b>{t('Classroom was created!')}</b></div>}
        </div>
    );
};