import { useCallback, useEffect, useState } from "react"
import { useHttp } from "../../../hooks/http-hook"
import UsersListCard from "../../PageComponents/Users/UsersListCard"
import { useContext } from "react"
import { AuthContext } from "../../../context/AuthContext"
import { NavLink } from "react-router-dom"
import { t } from "i18next"
import { useTranslation } from "react-i18next"

const ListUsersPage = () => {
    const [users, setUsers] = useState([])
    const {loading, request} = useHttp()
    const {token} = useContext(AuthContext)
    const {t} = useTranslation()
    const fetchUsers = useCallback(async () => {
        try {
            const fetched = await request('/api/admin', 'GET', null,  {'Authorization': 'Bearer ' + token})
            setUsers(fetched)
            
        } catch(e){}
    }, [request])


    useEffect(() => {
        fetchUsers()
    }, [fetchUsers])

    return (
        <div className="list-user-page">
            <div className="list-user-buttons">
            <button className="list-user-button-left"><NavLink to={'/admin/adminpanel'}>{t('Back')}</NavLink></button>
            <button className="list-user-button-right"><NavLink to={'/admin/adduser'} >{t('Create account')}</NavLink></button>
            </div>
            {(!loading) && <UsersListCard reload={fetchUsers} users={users} />}
            {loading && <div>{t('Loading...')}.</div>}
        </div>
    )
}

export default ListUsersPage