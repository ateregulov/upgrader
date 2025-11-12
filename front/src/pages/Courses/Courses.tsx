import { toast } from '@/hooks/use-toast'
import Api from '../../../api'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import CourseCard from '../StartScreen/CourseCard'
import { Course } from './types'

function Courses() {
  const [courses, setCourses] = useState<Course[]>([])
  const navigate = useNavigate()

  useEffect(() => {
    fetchCourses()
  }, [])

  const fetchCourses = async () => {
    try {
      const courses = await Api.getCourses()
      setCourses(courses)
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: 'Не удалось загрузить курсы',
        variant: 'error',
      })
    }
  }

  const handleCourseClick = (courseId: string) => {
    navigate(`/tasks/${courseId}`)
  }

  return (
    <div className='w-full'>
      <h1 className='text-3xl font-bold mb-8'>Выберите курс</h1>

      <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6'>
        {courses.map((course) => (
          <CourseCard key={course.id} course={course} onClick={handleCourseClick} />
        ))}
      </div>
    </div>
  )
}

export default Courses
