import { toast } from '@/hooks/use-toast'
import Api from '../../../api'
import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { CreateTaskResultDto, Task, TaskResult, TaskType } from './types'

function TaskPage() {
  const { courseId, taskId } = useParams()
  const [answer, setAnswer] = useState('')
  const [task, setTask] = useState<Task | null>(null)
  const [taskResult, setTaskResult] = useState<TaskResult | null>()
  const navigate = useNavigate()

  useEffect(() => {
    loadTask()
  }, [taskId])

  useEffect(() => {
    if (task?.results?.[0]) {
      const result = task.results?.[0]
      setTaskResult(result)
      setAnswer(result?.text ?? result?.listItems?.join(', ') ?? '')
    }
  }, [task])

  const loadTask = async () => {
    if (!taskId) return

    try {
      const data = await Api.getTask(taskId!, true)
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
    if (!task) return
    if (taskResult) {
      navigate(`/tasks/${courseId}`)
      return
    }

    try {
      let answerDto: CreateTaskResultDto = {
        taskId: task?.id!,
      }
      answerDto = fillAnswerByType(answerDto)

      await Api.createTaskResult(answerDto)
      navigate(`/tasks/${courseId}`)
    } catch {
      toast({
        title: 'Ошибка',
        description: 'Не удалось отправить ответ',
        variant: 'error',
      })
    }
  }

  const fillAnswerByType = (answerDto: CreateTaskResultDto): CreateTaskResultDto => {
    if (task?.type === TaskType.Text) {
      answerDto.text = answer
    }
    if (task?.type === TaskType.TextList) {
      answerDto.listItems = answer.split(',').map((item) => item.trim())
    }
    return answerDto
  }

  const renderAnswerInput = () => {
    if (task?.type === TaskType.Text) {
      return (
        <textarea
          disabled={taskResult != null}
          value={answer}
          onChange={(e) => setAnswer(e.target.value)}
          className='w-full h-48 p-4 rounded-lg bg-boxdark-2 border border-gray-600 focus:border-gray-400 outline-none text-gray-100 resize-none'
          placeholder='Введите ваш ответ...'
        />
      )
    }
    if (task?.type === TaskType.TextList) {
      return (
        <textarea
          disabled={taskResult != null}
          value={answer}
          onChange={(e) => setAnswer(e.target.value)}
          className='w-full h-48 p-4 rounded-lg bg-boxdark-2 border border-gray-600 focus:border-gray-400 outline-none text-gray-100 resize-none'
          placeholder='Введите ваш ответ, разделяйте запятой...'
        />
      )
    }
    return null
  }

  if (!task) return <div className='p-6 text-gray-300'>Загрузка...</div>

  return (
    <div className='max-w-3xl mx-auto'>
      <div className='bg-boxdark p-6 md:p-8 rounded-xl shadow-lg border border-gray-700'>
        <h1 className='text-2xl font-semibold mb-4 text-gray-100'>{task.title}</h1>

        <p className='text-gray-300 mb-6 whitespace-pre-line'>{task.text}</p>

        {renderAnswerInput()}

        <button
          onClick={handleSubmit}
          className='mt-6 w-full bg-primary text-white py-3 rounded-lg font-medium hover:bg-primary/80 transition'
        >
          {taskResult == null ? 'Отправить' : 'Назад к заданиям'}
        </button>
      </div>
    </div>
  )
}

export default TaskPage
