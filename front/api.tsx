import { AxiosRequestConfig, AxiosResponse } from 'axios'
import config from './config'
import { apiInstance } from './axiosConfig'

const { ApiUrl } = config

import { FrontendLog } from '@/services/errorService'

const convertDateToServer = (date: Date): string => date.toISOString()

const getUrl = (url: string): string => {
  if (url.startsWith('http')) {
    return url
  }
  return new URL(url, ApiUrl).href
}

const errorHandler = (err: any): void => {
  if (!err.response) {
    console.error('Server error.')
    throw err
  }
  if (err.response.status === 400 && err.response.data) {
    console.error('Bad request.', err.response.data.message)
  } else if (err.response.status === 401) {
    console.error('Unauthorized.')
  } else if (err.response.status === 403) {
    console.error('Forbidden.')
  } else {
    console.error('Server error. ' + err.response.statusText)
  }
  throw err
}

interface BaseApi {
  get: <T>(url: string, options?: AxiosRequestConfig) => Promise<T>
  post: <T>(url: string, model: any, config?: AxiosRequestConfig) => Promise<T>
  delete: (url: string) => Promise<void>
  put: <T>(url: string, model: any, config?: AxiosRequestConfig) => Promise<T>
}

const BaseApi: BaseApi = {
  get: async function <T>(url: string, options?: AxiosRequestConfig): Promise<T> {
    try {
      const response: AxiosResponse<T> = await apiInstance.get(getUrl(url), options)
      return response.data
    } catch (err) {
      errorHandler(err)
      throw err
    }
  },
  post: async function <T>(url: string, model: any, config?: AxiosRequestConfig): Promise<T> {
    try {
      const response: AxiosResponse<T> = await apiInstance.post(getUrl(url), model, config)
      return response.data
    } catch (err) {
      errorHandler(err)
      throw err
    }
  },
  delete: async function (url: string): Promise<void> {
    try {
      await apiInstance.delete(getUrl(url))
    } catch (err) {
      errorHandler(err)
      throw err
    }
  },
  put: async function <T>(url: string, model: any, config?: AxiosRequestConfig): Promise<T> {
    try {
      const response: AxiosResponse<T> = await apiInstance.put(getUrl(url), model, config)
      return response.data
    } catch (err) {
      errorHandler(err)
      throw err
    }
  },
  
}

interface NodeUrl {
  NodeApiMainUrl: string
  NodeApiSolidityUrl: string
}

const Api = {
  ApiUrl,
  errorHandler,
    
  createFrontendLog: async(error: FrontendLog): Promise<void> =>
    BaseApi.post<void>('api/frontendLog', error),

  ping: async(): Promise<void> =>
    BaseApi.get<void>('api/health/ping'),

  // Профиль слушателя
  getHearerProfile: async(): Promise<any> =>
    BaseApi.get<any>('api/hearer/profile'),

  createHearerProfile: async(profile: any): Promise<void> =>
    BaseApi.post<void>('api/hearer/profile', profile),

  updateHearerProfile: async(profile: any): Promise<void> =>
    BaseApi.put<void>('api/hearer/profile', profile),

  getProfileProgress: async(): Promise<any> =>
    BaseApi.get<any>('api/hearer/profile/progress'),

  uploadMyPhoto: async(file: File): Promise<void> => {
    const formData = new FormData();
    formData.append('file', file);
    return BaseApi.post<void>('api/hearer/photo/my', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
  },

  getMyPhoto: async(): Promise<string> => {
    const response = await apiInstance.get(getUrl('api/hearer/photo/my'), {
      responseType: 'blob',
    });
    return URL.createObjectURL(response.data);
  },

  deleteMyPhoto: async(): Promise<void> =>
    BaseApi.delete('api/hearer/photo/my'),
}
export default Api
