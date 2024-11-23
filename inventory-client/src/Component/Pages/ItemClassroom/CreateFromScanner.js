import { useContext, useState } from "react"
import { useHttp } from "../../../hooks/http-hook"
import { AuthContext } from "../../../context/AuthContext"
import { NavLink, useParams } from "react-router-dom"
import { useEffect } from "react"
import { useCallback } from "react"
import { t } from "i18next"
import { useTranslation } from "react-i18next";
import { NotificationManager } from "react-notifications"

export default function CreateFromScanner () {
    const {number} = useParams()
    const {t} = useTranslation()
    const {request, loading, error} = useHttp()
    const {token} = useContext(AuthContext)
    const [success, setSuccess] = useState(false)
    const [categories, setCategories] = useState([])
    const [image, setImage] = useState(null)
    const [classes, setClasses] = useState([])
    const [form, setForm] = useState({
        name: "",
        description:"",
        iconUrl:"https://inventory-app-aitu.s3.eu-north-1.amazonaws.com/aitu.jpg",
        condition:"",
        classroomId: '',
        categoryId:"", 
        itemNumber: number
    })
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
    function changeHandler(event){
        setForm({...form, [event.target.name]: event.target.value})
    }
    // const fetchClasses = useCallback(async function fetchClasses(){
    //     try {
    //         const data = await request('/api/classroom/getclassroomsname', 'GET', null, {'Authorization': 'Bearer ' + token})
    //         setClasses(data)
    //         setForm({...form, classroomId: data[0].name})
            
    //     } catch(e){}
    // }, [request])

    // useEffect(()=>{
    //     fetchClasses()
    // }, [fetchClasses])

    const fetchCategories = useCallback(async function fetchCategories(){
        try {
            const data = await request('/api/category', 'GET', null, {'Authorization': 'Bearer ' + token})
            setCategories(data)
            const data1 = await request('/api/classroom/getclassroomsname', 'GET', null, {'Authorization': 'Bearer ' + token})
            setClasses(data1)
            setForm({...form, categoryId: data[0].id, classroomId: data1[0].name})
            
        } catch(e) {}
    }, [request]) 

    const classList = classes.map(x =>{
        return <option id={x.id} value={x.name}>{x.name}</option>})

    useEffect(() => {
        fetchCategories()
    }, [fetchCategories])

// https://inventory-app-aitu.azurewebsites.net
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

            NotificationManager.info(t('Adding your item...'))
            if(image !== null) {
                const formData = new FormData()
                formData.append('file', image)
                const response = await (await fetch('https://inventory-app-aitu.azurewebsites.net' + '/api/item/upload', {method: 'POST', body: formData, headers: {"Authorization": 'Bearer '+ token}})).json()
    
                const data = await request('/api/item/fromscanner', 'POST', {...form, iconUrl: response.fileUrl}, {"Authorization": 'Bearer '+ token})
                setSuccess(true)
                return;
            }

            const data = await request('/api/item/fromscanner', 'POST', {...form}, {"Authorization": 'Bearer '+ token})
                setSuccess(true)
                NotificationManager.success(t('Added!'))
        }catch(e){}
    }

    const catList = categories.map(x => <option value={x.id}>{x.name}</option>)
    
    return (
        <div className="create-from-scanner">
            <h2>{t('Create Item from Scanner')}</h2>
            <div className="form-group">
                <label htmlFor="name">{t('Name')}</label>
                <input type="text" id="name" value={form.name} name="name" onChange={changeHandler}/>
            </div>
            <div className="form-group">
                <label htmlFor="description">{t('Description')}</label>
                <textarea id="description" value={form.description} name="description" onChange={changeHandler}/>
            </div>
            <div className="form-group">
                <label htmlFor="itemNumber">{t('Item number')}</label>
                <input type="text" id="itemNumber" value={form.itemNumber} name="itemNumber" onChange={changeHandler}/>
            </div>
            <div className="form-group">
                <label htmlFor="classroomId">{t('Classroom number')}</label>
                <select id="classroomId" onChange={changeHandler} name={'classroomId'} className="form-input">
                        {classList}
                    </select>
                {/* <input type="text" id="classroomId" value={form.classroomId} name="classroomId" onChange={changeHandler}/> */}
            </div>
            <div className="form-group">
                <label htmlFor="condition">{t('Condition')}</label>
                <input type="text" id="condition" value={form.condition} name="condition" onChange={changeHandler}/> 
            </div>
            <div className="form-group">
                <label htmlFor="categoryId">{t('Categories')}</label>
                <select id="categoryId" name="categoryId" onChange={changeHandler}>
                    {catList}
                </select>
            </div>
            <div className="form-group">
            <label htmlFor="input-file">{t('>Add icon<')}</label>
            <input id={'input-file'}
                               name={'file'}
                               accept={"image/png, image/jpg, image/gif, image/jpeg"}
                               type={'file'}
                               onChange={handleImage}/>
                               <div id='images-icon'></div>
        </div>
            <div className="form-group">
                <button onClick={registerHandler}>{t('Add item')}</button>
            </div>
            {success && <div className="success-message"><b>{t('Item was created!')}</b></div>}
        </div>
    )
};