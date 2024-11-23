import { useState, useCallback, useEffect, useContext } from "react"
import { AuthContext } from "../../../context/AuthContext"
import { useHttp } from "../../../hooks/http-hook"
import { useTranslation } from "react-i18next";
import { Link, NavLink } from "react-router-dom"
import { NotificationManager } from "react-notifications";

export default function SearchByCategory() {
    const {t} = useTranslation()
    const {token} = useContext(AuthContext)
    const {request, loading} = useHttp()
    const [result, setResult] = useState([])
    const [form, setForm] = useState({
        number: '',
        category: ''
    })

    const [categories, setCategories] = useState([])
    const fetchCategories = useCallback(async function fetchCategories(){
        try {
            const data = await request('/api/category', 'GET', null, {'Authorization': 'Bearer ' + token})
            setCategories(data)
            setForm({...form, category: data[0].name})
        } catch(e) {}
    }, [request]) 

    useEffect(() => {
        fetchCategories()
    }, [fetchCategories])

    const findClassrooms = async () => {
        try {
            NotificationManager.info(t('Searching...'))
            const data = await request('/api/classroom/search/'+form.category+'/'+form.number, 'GET', null, {'Authorization': 'Bearer ' + token})
            
            setResult(data)
            if(data.length === 0) {
                NotificationManager.error(t('No result'))
            }
            else {
                NotificationManager.success(t('Found!'))
            }
        } catch(e) {}
    }

    function changeHandler(event){
        setForm({...form, [event.target.name]: event.target.value.trim()})
        
    }

    const catList = categories.map(x =>{
        return <option value={x.name}>{x.name}</option>})

    const resList = result.map(x => {
    return<tr>
    <td><Link to={'/admin/listitems/'+x.id}>{x.classroomName}</Link></td>
    <td>{x.description}</td>
    <td>{x.numberOfItems}</td></tr>})
    return (
        <div className="search-by-category">
            <div><NavLink to={'/admin/adminpanel'}>{t('Back')}</NavLink></div>
        <h3>
            {t('This function was created to find appropriate classrooms with certain number of items per category.')}
        </h3>
        <div>
        <div>
            {t('Choose category')}: 
            <select onChange={changeHandler} name={'category'}>
                {catList}
            </select>
        </div>
        <div>
            <span>{t('Enter desired number of items:')} </span>
            <input type={'text'} name={"number"} onChange={changeHandler}/>
        </div>
        <div>
            <button onClick={findClassrooms}>{t('Find')}</button>
        </div>
        {result.length !== 0 && <table>
            <tr>
            <th>
                
                {t('Classroom name/number')}
                
            </th>
            <th>
                {t('Description')}
            </th>
            <th>
                {t('Number of')} {form.category}
            </th>
            </tr>
            {resList}
        </table>}
        </div>
        </div>
    )
}