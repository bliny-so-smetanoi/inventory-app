import { useState, useCallback, useEffect } from "react";
import { NavLink, useParams } from "react-router-dom"
import { useHttp } from "../../../hooks/http-hook";
import { useContext } from "react";
import { AuthContext } from "../../../context/AuthContext";
import { t } from "i18next"
import { useTranslation } from "react-i18next";
import { NotificationManager } from "react-notifications";

export default function EditClassPage() {
    const {id} = useParams()
    const {t} = useTranslation()
    const [classroom, setClassroom] = useState({})
    const [success, setSuccess] = useState(false)
    const {request, loading} = useHttp()
    const {token} = useContext(AuthContext)
    const [form, setForm] = useState({
        classroomName: classroom.classroomName,
        classroomDescription: classroom.classroomDescription,
        iconUrl: classroom.iconUrl
    })

    const fetchClassroom = useCallback(async () => {
        try {
            const fetched = await request('/api/classroom/'+id, 'GET', null,  {'Authorization': 'Bearer ' + token})
            setClassroom(fetched);
            setForm({classroomName: fetched.classroomName, classroomDescription: fetched.description, iconUrl: fetched.iconUrl});
        } catch(e){}
    }, [request])


    useEffect(() => {
        fetchClassroom()
    }, [fetchClassroom])


    function changeHandler(event){
        setForm({...form, [event.target.name]: event.target.value})
    }

    async function handleSave() {
        try {
            if(form.classroomName.length < 2) {
                NotificationManager.error(t('Name should be at least 2 characters!'))
                return;
            }
            NotificationManager.info(t('Editing is in progress...'))
            const data = await request('/api/classroom/'+id, 'PUT', form, {'Authorization': 'Bearer ' + token})
            NotificationManager.success(t('Classroom was edited!'))
            setSuccess(true)
        } catch(e){}
    }

    return (
        <div className="edit-class-page">
            <button className="edit-class-page-button"><NavLink to={'/admin/listclass'}>{t('Back')}</NavLink></button>
            {!loading &&
            <div>
                <div> {t('Name')}<input value={form.classroomName} onChange={changeHandler} name={'classroomName'}/></div>
                <div> {t('Description')}<input value={form.classroomDescription} onChange={changeHandler} name={'classroomDescription'}/></div>
                <button className="edit-class-page-save-changes" onClick={handleSave}>{t('Save changes')}</button>
            </div>}
            {loading && <div>{t('Loading...')}</div>}
        </div>
    );
}