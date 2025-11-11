import { toast } from '@/hooks/use-toast'
import Api from '../../../api'
import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'

function TaskPage() {
  const { courseId, taskId } = useParams()
  const [answer, setAnswer] = useState('')
  const [task, setTask] = useState<{
    id: string
    title: string
    text: string
  } | null>(null)
  const navigate = useNavigate()

  useEffect(() => {
    if (!taskId) return 

    loadTask()
  }, [taskId])

  const loadTask = async () => {
    try {
      const data = await Api.getTask(taskId!)
      setTask(data)
    } catch {
      toast({
        title: 'Ошибка',
        description: 'Не удалось загрузить задание',
        variant: 'error',
      })
    }
  }

  const handleSubmit = async () => {
    try {
      await Api.createTaskResult({
        taskId: task?.id!,
        text: answer,
      })
      navigate(`/tasks/${courseId}`)
    } catch {
      toast({
        title: 'Ошибка',
        description: 'Не удалось отправить ответ',
        variant: 'error',
      })
    }
  }

  if (!task) return <div className='p-6 text-gray-300'>Загрузка...</div>

  return (
    <div className='max-w-3xl mx-auto'>
      <div className='bg-boxdark p-6 md:p-8 rounded-xl shadow-lg border border-gray-700'>
        <h1 className='text-2xl font-semibold mb-4 text-gray-100'>{task.title}</h1>

        <p className='text-gray-300 mb-6 whitespace-pre-line'>{task.text}</p>

        <textarea
          value={answer}
          onChange={(e) => setAnswer(e.target.value)}
          className='w-full h-48 p-4 rounded-lg bg-boxdark-2 border border-gray-600 focus:border-gray-400 outline-none text-gray-100 resize-none'
          placeholder='Введите ваш ответ...'
        />

        <button
          onClick={handleSubmit}
          className='mt-6 w-full bg-primary text-white py-3 rounded-lg font-medium hover:bg-primary/80 transition'
        >
          Отправить
        </button>
      </div>
    </div>
  )
}

export default TaskPage
