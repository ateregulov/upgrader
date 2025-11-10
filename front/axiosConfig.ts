import axios from 'axios'
import config from './config'
const { ApiUrl } = config

declare global {
  interface Window {
    Telegram?: {
      WebApp?: {
        initData?: string
      }
    }
  }
}

const INIT_DATA = window.Telegram?.WebApp?.initData

export const apiInstance = axios.create({
  baseURL: ApiUrl,
  // timeout: 10000,
})

const requestInterceptor = (config: any ) => {
  config.headers['Authorization'] = `tma ${INIT_DATA}`
  return config
}

apiInstance.interceptors.request.use(requestInterceptor)

export const updateHeaders = (newHeaders: any) => {
  const updateInterceptor = (config: any) => {
    Object.assign(config.headers, newHeaders)
    return config
  }
  apiInstance.interceptors.request.use(updateInterceptor)
}
