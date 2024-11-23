import {useState, useCallback} from 'react'

export const useHttp = () => {
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState(null)

    const request = useCallback(async (url, method = 'GET', body = null, headers = {}) => {
        setLoading(true)
        try {
            if (body) {
                body = JSON.stringify(body)
                headers['Content-Type'] = 'application/json'
            }

            const response = await fetch('https://inventory-app-aitu.azurewebsites.net' + url, {method, body, headers})
            const data = await response.json()
// https://inventory-app-aitu.azurewebsites.net
            if (!response.ok) {
                throw new Error(data.message || 'Something went wrong...')
            }

            /* 
            if (!response.ok) {
                const errorMessage = data.message || t('Something went wrong...');
                throw new Error(errorMessage);
            }
            */

            setLoading(false)

            return data
        } catch (e) {
            setLoading(false)
            setError(e.message)
            throw e
        }
    }, [])

    const clearError = useCallback(() => setError(null), [])

    return { loading, request, error, clearError }
}