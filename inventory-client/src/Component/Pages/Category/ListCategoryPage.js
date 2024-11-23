import { useHttp } from "../../../hooks/http-hook"
import { useState, useCallback, useEffect } from "react"
import { AuthContext } from "../../../context/AuthContext"
import { useContext } from "react"
import { NavLink } from "react-router-dom"
import CategoriesListCard from "../../PageComponents/Categories/CategoriesListCard"
import { t } from "i18next"
import { useTranslation } from "react-i18next";

export default function ListCategoryPage() {
    const {t} = useTranslation()
    const [categories, setCategories] = useState([])
    const {loading, request} = useHttp()
    const {token} = useContext(AuthContext)

    const fetchCategories = useCallback(async () => {
        try {
            const fetched = await request('/api/category', 'GET', null,  {'Authorization': 'Bearer ' + token})
            setCategories(fetched)
            
        } catch(e){}
    }, [request])


    useEffect(() => {
        fetchCategories()
    }, [fetchCategories])

    return (
        <div className="list-category-page">
            <div className="list-category-buttons">
            <button className="list-category-button-left"><NavLink to={'/admin/adminpanel'}>{t('Back')}</NavLink></button>
            <button className="list-category-button-right"><NavLink to={'/admin/createcategory'} >{t('Create category')}</NavLink></button>
            </div>
            {(!loading) && <CategoriesListCard reload={fetchCategories} categories={categories} />}
            {loading && <div>{t('Loading...')}</div>}
        </div>
    )
}