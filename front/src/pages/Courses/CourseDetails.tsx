import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { toast } from '@/hooks/use-toast'
import { Course } from '../Courses/types'
import { Button } from '@mui/material'
import Api from '../../../api'

function CourseDetails() {
  const { courseId } = useParams<{ courseId: string }>()
  const [course, setCourse] = useState<Course | null>(null)
  const navigate = useNavigate()

  useEffect(() => {
    if (!courseId) return
    fetchCourse(courseId)
  }, [courseId])

  const fetchCourse = async (id: string) => {
    try {
      const data = await Api.getCourseById(id)
      setCourse(data)
    } catch {
      toast({
        title: 'Ошибка',
        description: 'Не удалось загрузить курс',
        variant: 'error',
      })
    }
  }

  const handleBuy = () => {
    navigate(`/payment/${course?.id}`)
  }

  if (!course) return <p className='text-gray-400'>Загрузка...</p>

  return (
    <div className='max-w-3xl mx-auto p-6 bg-gray-800 rounded-lg'>
      <h1 className='text-3xl text-white font-bold mb-4'>{course.title}</h1>

      <p className='text-gray-300 mb-4'>{course.longDescription}</p>

      <div className='flex flex-col gap-2 text-gray-400 mb-6'>
        <p>Количество заданий: {course.tasksCount}</p>
        <p>Цена: {course.price}₽</p>
      </div>

      {course.isBought ? (
        <Button
          sx={{
            backgroundColor: '#16a34a',
            '&:hover': { backgroundColor: '#15803d' },
            color: 'white',
            borderRadius: '8px',
            padding: '8px 16px',
          }}
          onClick={() => navigate(`/tasks/${course.id}`)}
        >
          Уже куплен — перейти к заданиям
        </Button>
      ) : (
        <Button
          sx={{
            backgroundColor: '#2563eb',
            '&:hover': { backgroundColor: '#1d4ed8' },
            color: 'white',
            borderRadius: '8px',
            padding: '8px 16px',
          }}
          onClick={handleBuy}
        >
          Купить
        </Button>
      )}
    </div>
  )
}

export default CourseDetails
