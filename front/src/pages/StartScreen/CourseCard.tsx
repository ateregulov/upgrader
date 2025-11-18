import { Course } from '../Courses/types'

interface CourseCardProps {
  course: Course
  onClick?: (courseId: string) => void
}

function CourseCard({ course, onClick }: CourseCardProps) {
  return (
    <div
      onClick={() => onClick?.(course.id)}
      className={`relative bg-gray-800 p-4 rounded-lg cursor-pointer transition-colors hover:bg-gray-700`}
    >
      {course.isBought && (
        <span className='absolute top-2 right-2 bg-green-600 text-white text-xs px-2 py-1 rounded'>Куплен</span>
      )}

      <h2 className='text-white text-xl font-semibold mb-2'>{course.title}</h2>
      <p className='text-gray-300 mb-2'>{course.shortDescription}</p>
      {course.isBought && (
        <p className='text-gray-300 mb-2'>{`${course.finishedTasksCount}/${course.tasksCount} заданий завершено`}</p>
      )}
      <p className='text-gray-400 text-sm'>{course.isBought ? 'Уже в вашей библиотеке' : `Цена: ${course.price}₽`}</p>
    </div>
  )
}

export default CourseCard
