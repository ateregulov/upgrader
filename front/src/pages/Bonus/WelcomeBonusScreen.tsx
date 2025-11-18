import { Button } from '@mui/material'
import Api from '../../../api'
import { useNavigate } from 'react-router-dom'
import { useEffect, useState } from 'react'
import { toast } from '@/hooks/use-toast'

const WelcomeBonusScreen: React.FC = () => {
  const navigate = useNavigate()
  const [registerBonus, setRegisterBonus] = useState<number>(0)

  useEffect(() => {
    getRegisterBonusAmount()
  }, [])

  const getRegisterBonusAmount = async () => {
    try {
      const bonus = await Api.getRegisterBonus()
      setRegisterBonus(bonus)
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: 'Ошибка пожалуйста попробуйте позже',
        variant: 'error',
      })
    }
  }

  const handleContinue = () => {
    navigate('/courses')
  }

  return (
    <div className='relative h-screen w-screen bg-gradient-to-br from-black to-gray-900 text-white flex flex-col items-center justify-center px-6 text-center'>
      <div className='max-w-md'>
        <h1 className='text-3xl font-bold mb-4'>Добро пожаловать!</h1>
        <p className='text-gray-300 text-base mb-6'>
          Вам начислен приветственный бонус <span className='font-semibold text-green-400'>{registerBonus} рублей</span>.
          Используйте его для покупок внутри приложения — открывайте новые курсы, задания и уровни.
        </p>

        <Button
          onClick={handleContinue}
          variant='contained'
          sx={{
            backgroundColor: 'white',
            color: 'black',
            fontWeight: 600,
            padding: '10px 28px',
            borderRadius: '12px',
            textTransform: 'none',
            boxShadow: '0 4px 14px rgba(255,255,255,0.2)',
            '&:hover': {
              backgroundColor: '#e6e6e6',
              boxShadow: '0 6px 18px rgba(255,255,255,0.25)',
            },
          }}
        >
          Продолжить
        </Button>
      </div>
    </div>
  )
}

export default WelcomeBonusScreen
