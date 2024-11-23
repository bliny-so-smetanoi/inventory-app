import { useCallback, useContext, useEffect, useState } from "react"
import { useHttp } from "../../../hooks/http-hook"
import { AuthContext } from "../../../context/AuthContext"
import { Link, Navigate, useNavigate, redirect } from "react-router-dom"
import Moment from "react-moment"
import { useTranslation } from "react-i18next";
import { NavLink } from "react-router-dom"
import { ThreeCircles } from "react-loader-spinner"
import { NotificationManager } from "react-notifications"
export default function UserPage(){
  const {t} = useTranslation()
    const [userData, setUserData] = useState({})
    const {request, loading} = useHttp()
    const {token, role} = useContext(AuthContext)
    const [reports, setReports] = useState([])
    const navigate = useNavigate()
    const [logs, setLogs] = useState(null)

    const fetchInfo = useCallback(async () => {
        try {
            const data = await request('/api/admin/identity', 'GET', null, {'Authorization': 'Bearer ' + token})
            setUserData(data)
            setReports(data.reportsUrl)
        } catch(e) {}
    }, [request])

    useEffect(() => {
        fetchInfo()
    }, [fetchInfo])

  
    const list = reports.map(x => {
        return <tr>
            <td><Moment format={'DD/MM/YYYY HH:mm a'} date={x.dateTime}/>
                
            </td>
            <td><button onClick={() => window.open(x.reportUrl, '_blank')}>{t('Show PDF')}</button></td>
            <td><button onClick={() => window.open(x.xlsUrl, '_blank')}>{t('Download Excel')}</button></td>
        </tr>
    })

    

    return (
        <>
          {loading && <div style={{maxWidth: '600px', margin: 'auto'}}> <ThreeCircles
                                            height="100"
                                            width="100"
                                            color="#163269"
                                            wrapperStyle={{height: '40em',justifyContent: 'center', alignItems: 'center'}}
                                            wrapperClass=""
                                            visible={true}
                                            ariaLabel="three-circles-rotating"
                                            outerCircleColor=""
                                            innerCircleColor=""
                                            middleCircleColor=""/></div>}
          {!loading && (
            <div className="user-page">
              <div className="user-page-buttons">
              <button className="user-page-button-left"><NavLink to={'/admin/adminpanel'}>{t('Back')}</NavLink></button>
              {role === 0 && <button className="user-page-button-right1" onClick={async () => {
                  try {
                    NotificationManager.info(t('Extracting logs...'))
                    const data = await request('/api/admin/logs', 'GET', null,{'Authorization': 'Bearer ' + token})
                    setLogs(data)
                    NotificationManager.success(t('Logs were extracted from server!'))
                  } catch(e){}
              }}>{t('Get logs')}</button>}
              {logs !== null && <div><button className="user-page-button-right2" onClick={() => window.open(logs.url, '_blank')}>{t('Open logs')}</button></div>}
              </div>
            <div className="user-page-container">
              <table className="user-page-table">
                <tbody>
                  <tr>
                    <th>{t('Email')}:</th>
                    <td>{userData.email}</td>
                  </tr>
                  <tr>
                    <th>{t('Access level')}:</th>
                    <td>{userData.role}</td>
                  </tr>
                </tbody>
              </table>
              <div className="user-page-reports">
                {t('Generated reports')}:
                {reports.length === 0 && <p>{t('No reports...')}</p>}
                {reports.length !== 0 && (
                  <table className="user-page-reports-table">
                    <thead>
                      <tr>
                        <th>{t('Report time')}</th>
                        <th>{t('PDF')}</th>
                        <th>{t('Excel')}</th>
                      </tr>
                    </thead>
                    <tbody>{list}</tbody>
                  </table>
                )}
              </div>
            </div>
            </div>
          )}
        </>
      )}