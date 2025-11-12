import { AxiosRequestConfig, AxiosResponse } from 'axios'
import config from './config'
import { apiInstance } from './axiosConfig'
import { Course } from '@/pages/Courses/types'
import { CreateTaskResultDto, Task } from '@/pages/Tasks/types'

const { ApiUrl } = config

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

  getCourses: async(): Promise<Course[]> =>
    BaseApi.get<Course[]>('api/courses'),

  getCourseById: async(id: string): Promise<Course> =>
    BaseApi.get<Course>(`api/courses/${id}`),

  getTasks: async(courseId: string): Promise<Task[]> =>
    BaseApi.get<Task[]>(`api/tasks?courseId=${courseId}`),

  getTask: async(taskId: string): Promise<Task> =>
    BaseApi.get<Task>(`api/tasks/${taskId}`),

  createTaskResult: async(dto: CreateTaskResultDto): Promise<void> =>
    BaseApi.post<void>(`api/task-results`, dto),
}
export default Api
