import { Button } from '@mui/material'
import { useBalance } from '@/Contexts/BalanceContext'

export default function WalletPage() {
  const { balance } = useBalance()

  return (
    <div className='p-8 max-w-xl mx-auto space-y-8'>
      <h1 className='text-3xl font-bold mb-2'>Кошелёк</h1>

      <div className='bg-gray-800 rounded-xl p-6 shadow-sm space-y-2 text-center'>
        <p className='text-gray-400'>Текущий баланс</p>
        <p className='font-bold text-white text-4xl'>{balance.toLocaleString()} ₽</p>
      </div>

      <div className='grid grid-cols-1 sm:grid-cols-2 gap-4'>
        <Button
          disabled
          variant='contained'
          sx={{
            backgroundColor: 'rgba(37, 99, 235, 0.9)',
            color: '#fff',
            '&:hover': {
              backgroundColor: 'rgba(30, 64, 175, 0.9)',
            },
            height: '52px',
            fontSize: '1rem',
            fontWeight: '600',
            borderRadius: '12px',
            textTransform: 'none',
          }}
        >
          Отправить
        </Button>

        <Button
          disabled
          variant='contained'
          sx={{
            backgroundColor: 'rgba(100, 116, 139, 0.9)',
            color: '#fff',
            '&:hover': {
              backgroundColor: 'rgba(71, 85, 105, 0.9)',
            },
            height: '52px',
            fontSize: '1rem',
            fontWeight: '600',
            borderRadius: '12px',
            textTransform: 'none',
          }}
        >
          Вывести
        </Button>
      </div>

      <div className='bg-gray-800 rounded-xl p-6'>
        <p className='text-gray-300 leading-relaxed'>
          Отправка и вывод доступны для пользователей с высоким рейтингом.
        </p>
      </div>
    </div>
  )
}
