import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { toast } from '@/hooks/use-toast'
import Api from '../../../api'
import { Button } from '@mui/material'
import { CoursePaymentInfo } from './types'
import { useBalance } from '@/Contexts/BalanceContext'

function Payment() {
  const { courseId } = useParams<{ courseId: string }>()
  const [paymentInfo, setPaymentInfo] = useState<CoursePaymentInfo>()
  const { setBalance } = useBalance();
  const navigate = useNavigate()

  useEffect(() => {
    getPaymentInfo()
  }, [courseId])

  const getPaymentInfo = async () => {
    if (!courseId) return

    try {
      const paymentInfo = await Api.getCoursePaymentInfo(courseId!)
      setPaymentInfo(paymentInfo)
    } catch (error) {
      toast({
        title: 'Ошибка',
        variant: 'error',
      })
    }
  }

  const handlePay = async () => {
    if (!paymentInfo) return

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
      <p className='mb-2'>Баланс: {paymentInfo?.balance}₽</p>
      <p className='mb-4'>Стоимость курса: {paymentInfo?.price}₽</p>

      <Button
        disabled={!paymentInfo || paymentInfo.balance < paymentInfo.price}
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
