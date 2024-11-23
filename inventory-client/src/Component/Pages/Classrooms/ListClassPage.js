import { useHttp } from "../../../hooks/http-hook"
import { useState, useCallback, useEffect } from "react"
import { AuthContext } from "../../../context/AuthContext"
import { useContext } from "react"
import { NavLink } from "react-router-dom"
import ClassroomListCard from "../../PageComponents/Classes/ClassroomListCard"
import { t } from "i18next"
import { useTranslation } from "react-i18next";

export default function ListClassPage() {
    const {t} = useTranslation()
    const [classes, setClasses] = useState([])
    const {loading, request} = useHttp()
    const {token} = useContext(AuthContext)
    const [search, setSearch] = useState({
        search: ''
    })
    const fetchClasses = useCallback(async () => {
        try {
            const fetched = await request('/api/classroom/', 'GET', null,  {'Authorization': 'Bearer ' + token})
            setClasses(fetched)
            
        } catch(e){}
    }, [request])

    const searchClassrooms = async () => {
        try{
            if(search.search.length === 0) {
                fetchClasses()
                return
            }
            console.log(search.search);
            const data = await request('/api/classroom/search/'+search.search,'GET', null, {'Authorization': 'Bearer ' + token})
            setClasses(data)
        }catch(e){}
    }

    function handleChange(e){
        setSearch({...search, [e.target.name]: e.target.value})
    }

    useEffect(()=>{
        searchClassrooms()
    }, [search])

    useEffect(() => {
        fetchClasses()
    }, [fetchClasses])

    return (
        <div className="list-class-page">
            <div className="buttons">
            <button className="button-left"><NavLink to={'/admin/adminpanel'}>{t('Back')}</NavLink></button>
            <button className="button-right"><NavLink to={'/admin/createclass'} >{t('Create class')}</NavLink></button>
            </div>
            
            
            <div><NavLink to={'/admin/searchbycategory'}>{t('Search by category')}</NavLink></div>
            <div>
            <input className="search-input" placeholder={t('Search by classroom name')} type={'text'} name={'search'} value={search.search} onChange={handleChange}/>
            </div>
            {(!loading) && <ClassroomListCard reload={fetchClasses} classes={classes} />}
            {loading && <div>{t('Loading...')}</div>}
        </div>
    );
}