import { useState, useCallback, useEffect } from "react";
import { NavLink, useParams } from "react-router-dom"
import { useHttp } from "../../../hooks/http-hook";
import { useContext } from "react";
import { AuthContext } from "../../../context/AuthContext";
import { t } from "i18next"
import { useTranslation } from "react-i18next";
import { ThreeCircles } from "react-loader-spinner";
import { NotificationManager } from "react-notifications";
export default function EditCategoryPage() {
    const {t} = useTranslation()
    const {id} = useParams()
    const [category, setCategory] = useState({})
    const [success, setSuccess] = useState(false)
    const {request, loading} = useHttp()
    const {token} = useContext(AuthContext)
    const [form, setForm] = useState({
        name: category.name,
        description: category.description,
        imageUrl: category.imageUrl
    })

    const fetchCategory = useCallback(async () => {
        try {
            const fetched = await request('/api/category/'+id, 'GET', null,  {'Authorization': 'Bearer ' + token})
            setCategory(fetched)
            setForm({name: fetched.name, description: fetched.description, imageUrl: fetched.imageUrl})
        } catch(e){}
    }, [request])


    useEffect(() => {
        fetchCategory()
    }, [fetchCategory])


    function changeHandler(event){
        setForm({...form, [event.target.name]: event.target.value})
    }

    async function handleSave() {
        try {
            if(form.name.length < 2) {
                NotificationManager.error(t('Name should be at least 2 characters!'))
                return;
            }
            NotificationManager.info(t('Editing is in progress...'))
            const data = await request('/api/category/'+id, 'PUT', form, {'Authorization': 'Bearer ' + token})
            setSuccess(true)
            NotificationManager.success(t('Edited!'))
        } catch(e){
            NotificationManager.error(e.message)
        }
    }

    return (<>
        {!loading &&
        <div className="edit-category-page">
            <button className="edit-category-page-button"><NavLink to={'/admin/listcategory'}>{t('Back')}</NavLink></button>
            <div> {t('Name')}<input value={form.name} onChange={changeHandler} name={'name'}/></div>
            <div> {t('Description')}<input value={form.description} onChange={changeHandler} name={'description'}/></div>
            <button className="edit-category-page-save-changes" onClick={handleSave}>{t('Save changes')}</button>
        </div>}
        {loading && <div style={{maxWidth: '600px', margin: 'auto'}}> <ThreeCircles
                                            height="100"
                                            width="100"
                                            color="#163269"
                                            wrapperStyle={{height: '40em',justifyContent: 'center', alignItems: 'center'}}
                                            wrapperClass=""
                                            visible={true}
                                            ariaLabel="three-circles-rotating"
                                            outerCircleColor=""
                                            innerCircleColor=""
                                            middleCircleColor=""/></div>}
        </>)
    }