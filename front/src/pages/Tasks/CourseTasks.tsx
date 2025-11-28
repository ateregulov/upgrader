import React, { useState, useEffect } from 'react'
import { CourseAnalyzeRequest, Task } from './types'
import TaskCard from './TaskCard'
import { useNavigate, useParams } from 'react-router-dom'
import Api from '../../../api'
import { toast } from '@/hooks/use-toast'
import { Button } from '@mui/material'
import { isAxiosError } from 'axios'
import { useBalance } from '@/Contexts/BalanceContext'

const CourseTasksMock: React.FC = () => {
  const { courseId } = useParams()
  const [tasks, setTasks] = useState<Task[]>([])
  const [isAllPassed, setIsAllPassed] = useState<boolean>(false)
  const [analyzeRequest, setAnalyzeRequest] = useState<CourseAnalyzeRequest>()
  const [analyzePrice, setAnalyzePrice] = useState<number>(0)
  const { syncBalance } = useBalance()

  const navigate = useNavigate()

  useEffect(() => {
    fetchTasks()
  }, [courseId])

  const fetchTasks = async () => {
    if (!courseId) return

    try {
      const tasks = await Api.getTasks(courseId)
      const allTasksPassed = tasks.every((task) => task.isCompleted)
      setIsAllPassed(allTasksPassed)
      setTasks(tasks)

      if (allTasksPassed) {
        const analyzeRequest = fetchAnalyzeRequest()
        const price = fetchAnalyzePrice()
        await Promise.all([analyzeRequest, price])
      }
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: 'Не удалось получить задания курса',
        variant: 'error',
      })
    }
  }

  const fetchAnalyzePrice = async () => {
    if (!courseId) return

    try {
      const price = await Api.getCourseAnalyzePrice()
      setAnalyzePrice(price)
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: 'Не удалось получить стоимость анализа курса',
        variant: 'error',
      })
    }
  }

  const fetchAnalyzeRequest = async () => {
    if (!courseId) return

    try {
      const request = await Api.getCourseAnalyzeRequest(courseId, true)
      setAnalyzeRequest(request)
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: 'Не удалось получить запрос на анализ курса',
        variant: 'error',
      })
    }
  }

  const createAnalyzeRequest = async () => {
    if (!courseId) return

    try {
      await Api.createCourseAnalyzeRequest(courseId)
      toast({
        title: 'Успех',
        description: 'Запрос на анализ создан мы пришлем вам уведомление как только разбор будет готов',
        variant: 'success',
      })
      fetchAnalyzeRequest()
      syncBalance()
    } catch (error) {
      if (isAxiosError(error) && error.response?.status === 400 && error.response?.data == 'INFLUENT_BALANCE') {
        toast({
          title: 'Ошибка',
          description: 'Недостаточно средств для создания запроса на анализ',
          variant: 'error',
        })
        return
      }
      toast({
        title: 'Ошибка',
        description: 'Не удалось создать запрос на анализ курса',
        variant: 'error',
      })
    }
  }

  const handleAnalyzeRequestClick = () => {
    navigate(`/courses/${courseId}/analyze`)
  }

  const handleTaskClick = (taskId: string) => {
    navigate(`/tasks/${courseId}/${taskId}`)
  }

  return (
    <div className='p-8'>
      <h1 className='text-2xl font-bold mb-6'>Задания курса</h1>

      <div className='flex gap-4 flex-wrap'>
        {tasks.map((task) => (
          <TaskCard onClick={handleTaskClick} key={task.id} task={task} />
        ))}
      </div>
      <div className='text-center mt-7'>
        {isAllPassed &&
          (!analyzeRequest ? (
            <div>
              <Button
                onClick={createAnalyzeRequest}
                fullWidth
                variant='contained'
                sx={{
                  backgroundColor: '#1f2937',
                  '&:hover': { backgroundColor: '#374151' },
                  '&.Mui-disabled': {
                    backgroundColor: '#4b5563',
                    color: '#9ca3af',
                  },
                  mt: '10px',
                  borderRadius: '8px',
                  padding: '8px 16px',
                  width: { sx: '200px', sm: '30%' },
                }}
              >
                Создать запрос на анализ ответов специалистом
              </Button>
              <div className='text-center mt-4 text-gray-500'>Стоимость разбора: {analyzePrice} ₽</div>
            </div>
          ) : analyzeRequest.result == null ? (
            <div>
              <div className='text-center mt-4 text-gray-500'>
                Ваша заявка на разбор курса находится в обработке. Мы пришлем вам уведомление как только разбор будет готов.
              </div>
            </div>
          ) : (
            <Button
              onClick={handleAnalyzeRequestClick}
              fullWidth
              variant='contained'
              sx={{
                backgroundColor: '#1f2937',
                '&:hover': { backgroundColor: '#374151' },
                '&.Mui-disabled': {
                  backgroundColor: '#4b5563',
                  color: '#9ca3af',
                },
                mt: '10px',
                borderRadius: '8px',
                padding: '8px 16px',
                width: { sx: '200px', sm: '30%' },
              }}
            >
              Перейти к разбору ваших ответов
            </Button>
          ))}
      </div>
    </div>
  )
}

export default CourseTasksMock
