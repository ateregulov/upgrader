import React, { useState, useEffect } from 'react'
import { Task } from './types'
import TaskCard from './TaskCard'
import { useNavigate, useParams } from 'react-router-dom'
import Api from '../../../api'
import { toast } from '@/hooks/use-toast'

const CourseTasksMock: React.FC = () => {
  const { courseId } = useParams()
  const [tasks, setTasks] = useState<Task[]>([])
  const navigate = useNavigate()

  useEffect(() => {
    fetchTasks()
  }, [courseId])

  const fetchTasks = async () => {
    if (!courseId) return

    try {
      const tasks = await Api.getTasks(courseId)
      setTasks(tasks)
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: 'Не удалось получить задания курса',
        variant: 'error',
      })
    }
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
    </div>
  )
}

export default CourseTasksMock
