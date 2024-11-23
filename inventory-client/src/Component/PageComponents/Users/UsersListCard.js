import { useContext, useEffect, useState } from "react"
import { useHttp } from "../../../hooks/http-hook"
import { AuthContext } from "../../../context/AuthContext"
import { t } from "i18next"
import { useTranslation } from "react-i18next";

export default function UsersListCard(props){
    const {t} = useTranslation()
    const {request} = useHttp()
    const {token} = useContext(AuthContext)
    const users = props.users

    const list = users.map(x => 
        <tr key={x.id}>
            <td>
                {x.email}
            </td>
            <td>
                {x.fullName}
            </td>
            <td>
                {x.role === 2 && t('Moderator')}
                {x.role === 1 && t('Admin')}
            </td>
            <td><button onClick={async () => {
                if(window.confirm("Delete user?")){
                    const data = await request('/api/admin/'+x.id, 'DELETE', null, {'Authorization': 'Bearer '+token})

                    props.reload()
                }
            }}>{t('delete')}</button></td>
        </tr>
    )
    
    return (<div className="users-list-card">
    <table> 
          <tr>
            <th>{t('Email')}</th>
            <th>{t('Full name')}</th>
            <th>{t('Access level')}</th>
            <th></th>
          </tr>
        
        {list.length !== 0 ? list : <div>{t('No users...')}</div>}

    </table>
    </div>)
}