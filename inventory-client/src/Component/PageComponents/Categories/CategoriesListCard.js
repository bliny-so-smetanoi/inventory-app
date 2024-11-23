import { useContext, useEffect, useState } from "react"
import { useHttp } from "../../../hooks/http-hook"
import { AuthContext } from "../../../context/AuthContext"
import { NavLink } from "react-router-dom"
import { t } from "i18next"
import { useTranslation } from "react-i18next";
import { NotificationManager } from "react-notifications"

export default function CategoriesListCard(props){
    const {t} = useTranslation()
    const {request} = useHttp()
    const {token, role} = useContext(AuthContext)
    const categories = props.categories

    const list = categories.map(x => 
        <tr key={x.id}>
            <td>
                {x.name}
            </td>
            <td>
                {x.description}
            </td>
            <td><NavLink to={`/admin/editcategory/${x.id}`}>{t('edit')}</NavLink></td>
            <td>
            {role !== 2 && <button onClick={async () => {
                if(window.confirm('Delete category?')) {
                    NotificationManager.info(t('Deletion is in progress...'));
                    const data = await request('/api/category/'+x.id, 'DELETE', null, {'Authorization': 'Bearer ' +token})
                    NotificationManager.error(t('Deleted!'));
                    props.reload()
                }
                
            }}>{t('delete')}</button>}
            </td>
        </tr>
    )
    
    return (<div className="categories-list-card">
    <table> 
          <tr>
            <th>{t('Name')}</th>
            <th>{t('Description')}</th>
            <th></th>
          </tr>
        
        {list.length !== 0 ? list : <div>{t('No categories...')}</div>}

    </table>
    </div>)
}
