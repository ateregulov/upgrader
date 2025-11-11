import { Course } from './types'

interface courseCardProps {
  course: Course
  onClick?: (courseId: string) => void
}

function CourseCard({ course, onClick }: courseCardProps) {
  return (
    <div onClick={() => onClick?.(course.id)} className='bg-gray-800 hover:bg-gray-700 p-4 rounded-lg cursor-pointer transition-colors'>
      <h2 className='text-xl font-semibold mb-2'>{course.title}</h2>
      <p className='text-gray-300 mb-2'>{course.shortDescription}</p>
      <p className='text-gray-400 text-sm'>Цена: {course.price}₽</p>
    </div>
  )
}

export default CourseCard
