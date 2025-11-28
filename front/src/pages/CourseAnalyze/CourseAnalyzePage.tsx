import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { toast } from '@/hooks/use-toast'
import Api from '../../../api'
import { Button } from '@mui/material'
import { CourseAnalyzeResult } from '../Tasks/types'

function CourseAnalyzePage() {
  const { courseId } = useParams<{ courseId: string }>()
  const [result, setResult] = useState<CourseAnalyzeResult | null>(null)
  const navigate = useNavigate()

  useEffect(() => {
    if (!courseId) return
    fetchAnalyzeResult(courseId)
  }, [courseId])

  const fetchAnalyzeResult = async (id: string) => {
    try {
      const data = await Api.getCourseAnalyzeResult(id)
      setResult(data)
    } catch {
      toast({
        title: 'Ошибка',
        description: 'Не удалось загрузить анализ курса',
        variant: 'error',
      })
    }
  }

  if (result == null) {
    return <div className='p-6 text-gray-300'>Разбор пока не готов или заявка еще в обработке.</div>
  }

  return (
    <div className='max-w-3xl mx-auto p-6 bg-gray-800 rounded-lg text-gray-200'>
      <h1 className='text-2xl font-bold mb-4'>Разбор ваших ответов</h1>

      <p className='text-gray-300 whitespace-pre-line mb-6'>{result.message}</p>

      <Button
        variant='contained'
        onClick={() => navigate(`/tasks/${courseId}`)}
        sx={{
          backgroundColor: '#2563eb',
          '&:hover': { backgroundColor: '#1d4ed8' },
          color: 'white',
          borderRadius: '8px',
          padding: '8px 16px',
        }}
      >
        Назад к заданиям
      </Button>
    </div>
  )
}

export default CourseAnalyzePage
