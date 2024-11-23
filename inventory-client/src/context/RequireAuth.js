import { useContext, useEffect } from "react"
import { Navigate, Outlet, useLocation, useNavigate } from "react-router-dom"
import { useAuth } from "../hooks/auth-hook"
import { AuthContext } from "./AuthContext"

const RequireAuth = ({allowedRoles}) => {
    const {isAuth, role, persist} = useContext(AuthContext)
    const location = useLocation()
    const navigate = useNavigate()
    return (
        allowedRoles.includes(role) ? <Outlet/> : isAuth ? navigate('/noaccess') : navigate('/unauthorized')

    )

}

export default RequireAuth