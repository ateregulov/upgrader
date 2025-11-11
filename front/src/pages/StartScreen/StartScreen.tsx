import React, { useEffect, useState } from 'react'
import config from '../../../config'
import { Course } from './types'
import CourseCard from './CourseCard'
import Api from '../../../api'
import { toast } from '@/hooks/use-toast'

// const courses: Course[] = [
//   {
//     id: '1',
//     title: 'React Basics',
//     shortDescription: 'Learn React from scratch',
//     longDescription: 'Full React course for beginners',
//     price: 0,
//   },
//   {
//     id: '2',
//     title: 'Advanced TypeScript',
//     shortDescription: 'Deep dive into TS',
//     longDescription: 'Master TypeScript for large projects',
//     price: 50,
//   },
//   {
//     id: '3',
//     title: 'Tailwind CSS',
//     shortDescription: 'Design modern UI',
//     longDescription: 'Learn Tailwind CSS from basics to advanced',
//     price: 30,
//   },
// ]

const StartScreen: React.FC = () => {
  const { AppVersion } = config
  const [courses, setCourses] = useState<Course[]>([]);

  useEffect(() => {
    fetchCourses()
  }, [])

  const fetchCourses = async () => {
    try {
      const courses = await Api.getCourses();
      setCourses(courses)
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: 'Не удалось загрузить курсы',
        variant: 'error',
      })
    }
  }

  return (
    <div className='relative h-screen w-screen bg-gradient-to-br from-black to-gray-900 text-white flex flex-col'>
      {/* Основной контент */}
      <div className='flex-1 flex flex-col items-center justify-center px-8'>
        <h1 className='text-3xl font-bold mb-8'>Выберите курс</h1>
        <div className='grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6 w-full max-w-5xl'>
          {courses.map((course) => (
            <CourseCard key={course.id} course={course} />
          ))}
        </div>
      </div>

      {/* Версия приложения */}
      <div className='absolute bottom-4 left-4 text-gray-400 text-xs'>v{AppVersion}</div>
    </div>
  )
}

export default StartScreen
