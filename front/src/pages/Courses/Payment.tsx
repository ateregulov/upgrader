import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { toast } from '@/hooks/use-toast'
import Api from '../../../api'
import { Button } from '@mui/material'
import { useBalance } from '@/Contexts/BalanceContext'
import { coursesCache } from './cache'

function Payment() {
  const { courseId } = useParams<{ courseId: string }>()
  const { setBalance, balance } = useBalance();
  const [coursePrice, setCoursePrice] = useState<number>();
  const navigate = useNavigate()

  useEffect(() => {
    getCoursePrice()
  }, [courseId])

  const getCoursePrice = async () => {
    if(!courseId) return;

    let course = coursesCache.get(courseId)
    if (course){
      setCoursePrice(course.price)
      return;
    }

    try {
      const course = await Api.getCourseById(courseId)
      coursesCache.set(courseId, course)
      setCoursePrice(course.price)
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: 'Пожалуйста попробуйте позже',
        variant: 'error',
      })
    }
  }

  const handlePay = async () => {
    if (!coursePrice) return
    if (balance < coursePrice) return

    try {
      await Api.buyCourse(courseId!)
      toast({ title: 'Успешно', description: 'Курс куплен!', variant: 'success' })
      const newBalance = await Api.getBalance()
      setBalance(newBalance)
      navigate(`/tasks/${courseId}`)
    } catch {
      toast({
        title: 'Ошибка',
        description: 'Не удалось провести оплату',
        variant: 'error',
      })
    }
  }

  return (
    <div className='max-w-md mx-auto p-6 bg-gray-800 rounded-lg text-white'>
      <h2 className='text-2xl font-semibold mb-4'>Оплата курса</h2>
      <p className='mb-2'>Баланс: {balance}₽</p>
      <p className='mb-4'>Стоимость курса: {coursePrice}₽</p>

      <Button
        disabled={!coursePrice || balance < coursePrice}
        sx={{
          backgroundColor: '#2563eb',
          '&:hover': { backgroundColor: '#1d4ed8' },
          '&.Mui-disabled': {
            backgroundColor: '#4b5563',
            color: '#9ca3af',
          },
          borderRadius: '8px',
          padding: '8px 16px',
        }}
        variant='contained'
        className='text-white w-full py-2'
        onClick={handlePay}
      >
        Оплатить
      </Button>
    </div>
  )
}

export default Payment
