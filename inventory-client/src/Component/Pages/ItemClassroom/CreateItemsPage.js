import { useContext, useState } from "react"
import { useHttp } from "../../../hooks/http-hook"
import { AuthContext } from "../../../context/AuthContext"
import { NavLink, useParams } from "react-router-dom"
import { useEffect } from "react"
import { useCallback } from "react"
import { t } from "i18next"
import { useTranslation } from "react-i18next";
import { NotificationManager } from "react-notifications"

export default function CreateItemsPage () {
    const {id} = useParams()
    const {t} = useTranslation()
    const {request, loading, error} = useHttp()
    const {token} = useContext(AuthContext)
    const [success, setSuccess] = useState(false)
    const [categories, setCategories] = useState([])
    const [pending, setPending] = useState(false)
    const [image, setImage] = useState(null)
    
    const [form, setForm] = useState({
        name: "",
        description:"",
        iconUrl:"https://inventory-app-aitu.s3.eu-north-1.amazonaws.com/aitu.jpg",
        condition:"",
        classroomId: id,
        categoryId:"", 
        itemNumber:''
    })
    
    function changeHandler(event){
        setForm({...form, [event.target.name]: event.target.value})
    }

    

    function handleImage(event){
        let container = document.getElementById('images-icon')
        container.innerHTML = ''
        setImage(event.target.files[0])
        let newUrl = window.URL.createObjectURL(event.target.files[0])
        let imageBox = document.createElement('img')
        imageBox.src = newUrl
        imageBox.width = 100
        container.append(imageBox)
    }

    const fetchCategories = useCallback(async function fetchCategories(){
        try {
            const data = await request('/api/category', 'GET', null, {'Authorization': 'Bearer ' + token})
            setCategories(data)
            setForm({...form, categoryId: data[0].id})
            
        } catch(e) {}
    }, [request]) 

    useEffect(() => {
        fetchCategories()
    }, [fetchCategories])

    async function registerHandler(){
        try{

            if((form.name.length < 2
                || form.description.length < 2
                 ||form.condition.length < 2
                 || form.itemNumber.length < 2)) {
                       NotificationManager.error(t('Empty fields!'))
                       return;
                    }

           if(form.name.length > 50
               || form.description.length > 150
                ||form.condition.length > 50
                || form.itemNumber.length > 100) {
                   NotificationManager.error(t('Too much data!'))
                       return;
                }
            setPending(true)
            NotificationManager.info(t('Loading...'))
            if(image !== null ) {
                const formData = new FormData()
                formData.append('file', image)
                
                // https://inventory-app-aitu.azurewebsites.net
    
                const response = await (await fetch('https://inventory-app-aitu.azurewebsites.net' + '/api/item/upload', {method: 'POST', body: formData, headers: {"Authorization": 'Bearer '+ token}})).json()
                    
                const data = await request('/api/item/', 'POST', {...form, iconUrl: response.fileUrl}, {"Authorization": 'Bearer '+ token})
                setSuccess(true)
                NotificationManager.success(t('Item was created!'))
                setPending(false)
                return;
            }

            const data = await request('/api/item/', 'POST', {...form}, {"Authorization": 'Bearer '+ token})
            setSuccess(true)
                NotificationManager.success(t('Item was created!'))
                setPending(false)
        }catch(e){}
    }




    const catList = categories.map(x => <option value={x.id}>{x.name}</option>)
    
    return (
        <div className="create-items-page">
            <button className="back-button"><NavLink to={'/admin/listitems/'+id}>{t('Back')}</NavLink></button>
            <div className="form-container">
            <h2 className="page-title">{t('Create Item')}</h2>
                <div className="form-group">
                    <label htmlFor="name">{t('Name')}</label>
                    <input type="text" id="name" value={form.name} name="name" onChange={changeHandler} className="form-input"/>
                </div>
                <div className="form-group1">
                    <label htmlFor="description">{t('Description')}</label>
                    <input id="description" value={form.description} name="description" onChange={changeHandler} className="form-input"/>
                </div>
                <div className="form-group2">
                    <label htmlFor="itemNumber">{t('Barcode')}</label>
                    <input type="text" id="itemNumber" value={form.itemNumber} name="itemNumber" onChange={changeHandler} className="form-input"/>
                </div>
                <div className="form-group3">
                    <label htmlFor="condition">{t('Condition')}</label>
                    <input type="text" id="condition" value={form.condition} name="condition" onChange={changeHandler} className="form-input"/> 
                </div>
                <div className="form-group4">
                    <label htmlFor="categoryId">{t('Categories')}</label>
                    <select id="categoryId" name="categoryId" onChange={changeHandler} className="form-input">
                        {catList}
                    </select>
                </div>
                <div className="form-group5">
                    <label htmlFor="file" className="file-label">{t('>Add icon<')}</label>
                    <input id="file"
                           name="file"
                           accept="image/png, image/jpg, image/gif, image/jpeg"
                           type="file"
                           onChange={handleImage}
                           className="file-input"/>
                           <div id="images-icon">

                           </div>
                </div>
                <div className="form-group6">
                    <button onClick={registerHandler} className="submit-button">{t('Add item')}</button>
                </div>
                {success && <div className="success-message">{t('Item was created!')}</div>}
            </div>
        </div>
    )
};