import { useState, useCallback, useEffect } from "react";
import { NavLink, useParams } from "react-router-dom"
import { useHttp } from "../../../hooks/http-hook";
import { useContext } from "react";
import { AuthContext } from "../../../context/AuthContext";
import { t } from "i18next"
import { useTranslation } from "react-i18next";
import { NotificationManager } from "react-notifications";

export default function EditItemsPage() {
    const {t} = useTranslation()
    const {id} = useParams()
    const [item, setItem] = useState({})
    const [categories, setCategories] = useState([])
    const [message, setMessage] = useState('')
    const [success, setSuccess] = useState(false)
    const [image, setImage] = useState(null)
    const {request, loading, error} = useHttp()
    const [imagesFile, setImagesFile] = useState([])
    const {token} = useContext(AuthContext)
    const [classes, setClasses] = useState([])
    const [form, setForm] = useState({
        name: item.name,
        description: item.description,
        iconUrl: item.iconUrl,
        condition: item.condition,
        classroomId: item.classroomId,
        categoryId:item.categoryId,
        itemNumber: item.itemNumber
    })

    function handleImage(event){
        setImage(event.target.files[0])
        let newUrl = window.URL.createObjectURL(event.target.files[0])
        let container = document.getElementById('images-icon')
        let imageBox = document.createElement('img')
        imageBox.src = newUrl
        imageBox.width = 100
        container.append(imageBox)
    }

    const fetchItem = useCallback(async () => {
        try {
            const fetched = await request('/api/item/byid/'+id, 'GET', null,  {'Authorization': 'Bearer ' + token})
            setItem(fetched);
            setForm({condition: fetched.condition,classroomId: fetched.classroomName,categoryId: fetched.categoryId, itemNumber: fetched.itemNumber, name: fetched.name, description: fetched.description, iconUrl: fetched.iconUrl});
        } catch(e){}
    }, [request])


    useEffect(() => {
        fetchItem()
    }, [fetchItem])

    const fetchCategories = useCallback(async function fetchCategories(){
        try {
            const data = await request('/api/category', 'GET', null, {'Authorization': 'Bearer ' + token})
            setCategories(data)
        } catch(e) {}
    }, [request]) 

    const fetchClasses = useCallback(async function fetchClasses(){
        try {
            const data = await request('/api/classroom/getclassroomsname', 'GET', null, {'Authorization': 'Bearer ' + token})
            setClasses(data)
        } catch(e){}
    }, [request])

    useEffect(()=>{
        fetchClasses()
    }, [fetchClasses])
    useEffect(() => {
        fetchCategories()
    }, [fetchCategories])

    function handleImagesFile(event) {
        setImagesFile(event.target.files)
        let container = document.getElementById('images-list')
        container.innerHTML = ''
        for(let i = 0; event.target.files.length > i; i++) {
            let newUrl = window.URL.createObjectURL(event.target.files[i])
            let container = document.getElementById('images-list')
            let imageBox = document.createElement('img')
            imageBox.src = newUrl
            imageBox.width = 100
            container.append(imageBox)
        }
        
    }

    const catList = categories.map(x =>{
        if(x.id === item.categoryId) {
            return <option selected value={x.id}>{x.name}</option>    
        }
        return <option value={x.id}>{x.name}</option>})

    function changeHandler(event){
        setForm({...form, [event.target.name]: event.target.value})
    }

    
    async function handleSave() {
        try {
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

            NotificationManager.info(t('Saving your changes...'));
            setMessage('');
             if(image === null) {
                const data = await request('/api/item/'+id, 'PUT', {...form}, {'Authorization': 'Bearer ' + token});
                
                const formDataImages = new FormData();
                for (var i = 0; i < imagesFile.length; i++) {
                    formDataImages.append('files', imagesFile[i]);
                }
                formDataImages.append('owner', id);
                if(imagesFile.length !== 0) {
                    const imagesResponse = await fetch('https://inventory-app-aitu.azurewebsites.net' + '/api/item/uploadmany', {method: 'POST', body: formDataImages, headers:{"Authorization": 'Bearer '+ token}});
                    }
                    NotificationManager.success(t('Saved!'));
                return;
                // https://inventory-app-aitu.azurewebsites.net
             }
            
            const formData = new FormData();
            formData.append('file', image);
            const response = await (await fetch('https://inventory-app-aitu.azurewebsites.net' + '/api/item/upload', {method: 'POST', body: formData, headers: {"Authorization": 'Bearer '+ token}})).json();
// https://inventory-app-aitu.azurewebsites.net
            const formDataImages = new FormData();
            for (var i = 0; i < imagesFile.length; i++) {
                formDataImages.append('files', imagesFile[i]);
            }
            formDataImages.append('owner', id);
            if(imagesFile.length !== 0) {
                const imagesResponse = await fetch('https://inventory-app-aitu.azurewebsites.net' + '/api/item/uploadmany', {method: 'POST', body: formDataImages, headers:{"Authorization": 'Bearer '+ token}});
            }
            // https://inventory-app-aitu.azurewebsites.net
            console.log(imagesFile.length);
            const data = await request('/api/item/'+id, 'PUT', {...form, iconUrl: response.fileUrl}, {'Authorization': 'Bearer ' + token});
            setSuccess(true);
            NotificationManager.success(t('Saved!'));
        } catch(e){
            
            NotificationManager.error(e.message)
            console.log(e.message);
        }
    }
    const classList = classes.map(x =>{
        if(x.id === item.classroomId) {
            return <option selected value={x.name}>{x.name}</option>    
        }
        return <option value={x.name}>{x.name}</option>})
    return (
        <div className="edit-items-page">
            <NavLink to={'/admin/listitems/'+item.classroomId} className="back-button">{t('Back')}</NavLink>
            {!loading &&
            <div className="form-container">
                <h2 className="page-title">{t('Edit Item')}</h2>
                <div className="form-group">
                    <label htmlFor="name">{t('Name')}</label>
                    <input type="text" id="name" value={form.name} onChange={changeHandler} name={'name'} className="form-input"/>
                </div>
                <div className="form-group">
                    <label htmlFor="description">{t('Description')}</label>
                    <input id="description" value={form.description} onChange={changeHandler} name={'description'} className="form-input"/>
                </div>
                <div className="form-group">
                    <label htmlFor="itemNumber">{t('Item number')}</label>
                    <input type="text" id="itemNumber" value={form.itemNumber} onChange={changeHandler} name={'itemNumber'} className="form-input"/>
                </div>
                <div className="form-group">
                    <label htmlFor="condition">{t('Condition')}</label>
                    <input type="text" id="condition" value={form.condition} onChange={changeHandler} name={'condition'} className="form-input"/>
                </div>
                <div className="form-group">
                    <label htmlFor="categoryId">{t('Categories')}</label>
                    <select onChange={changeHandler} name={'categoryId'} className="form-input">
                        {catList}
                    </select>
                </div>
                <div className="form-group">
                    <label htmlFor="classroomId">{t('Classroom number')}</label>
                    <select onChange={changeHandler} name={'classroomId'} className="form-input">
                        {classList}
                    </select>
                    {/* <input type="text" id="classroomId" value={form.classroomId} onChange={changeHandler} name={'classroomId'} className="form-input"/> */}
                </div>
                <div className="form-group">
                    <label htmlFor="file" className="file-label">{t('Add icon')}</label>
                    <input id="file" name="file" accept={"image/png, image/jpg, image/gif, image/jpeg"} type="file" onChange={handleImage} className="file-input"/>
                    <div id="images-icon"></div>
                </div>
                <div className="form-group">
                    <label htmlFor="files" className="file-label">{t('Add images')}</label>
                    <input id="files" name="files" accept={"image/png, image/jpg, image/gif, image/jpeg"} type="file" multiple onChange={handleImagesFile} className="file-input"/>
                    <div id={'images-list'}></div>
                    {/* {imagesFile.length !== 0 && imagesFile.map((x, index) => {
                        <img className={'input-image'}
                             width={'100'}
                             id={'image-input'+index}
                             src={''}  alt={''}/>
                    })} */}
                </div>
                <div className="form-group">
                    <button onClick={handleSave} className="submit-button">{t('Save changes')}</button>
                </div>
            </div>}
            {loading && <div>{t('Loading...')}</div>}
            {success && <div className="success-message">{t('Item was edited!')}</div>}
            {message && <div className="error-message">{message}</div>}
        </div>
    )
}