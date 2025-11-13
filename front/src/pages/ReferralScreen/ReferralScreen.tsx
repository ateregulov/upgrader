import { TextField } from '@mui/material'
import { useEffect, useState } from 'react'
import { toast } from '@/hooks/use-toast'
import { RefInfo } from './types'
import Api from '../../../api'

const ReferralScreen: React.FC = () => {
  const [refInfo, setRefInfo] = useState<RefInfo>()

  useEffect(() => {
    getRefInfo()
  }, [])

  const getRefInfo = async () => {
    try {
      const data = await Api.getReferralInfo()
      setRefInfo(data)
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: 'Не удалось получить реферальную информацию',
        variant: 'destructive',
      })
    }
  }

  return (
    <div className='p-8'>
      <h1 className='text-3xl font-bold mb-6'>Реферальная программа</h1>

      {/* Интро-карточка */}
      <div className='bg-gray-800 rounded-xl p-6 mb-8'>
        <h2 className='text-xl font-semibold text-white mb-2'>Пригласите друзей — получите бонусы!</h2>
        <p className='text-gray-300 mb-4'>За каждого друга, который зарегистрируется используя вашу ссылку</p>
        <ul className='text-gray-300 space-y-1 mb-4'>
          <li>
            • Вы получаете <span className='font-semibold text-green-400'>+{refInfo?.refBonusAmount || 0}₽</span> за каждого друга
          </li>
        </ul>
      </div>

      <div className='bg-gray-800 rounded-xl p-6 mb-8'>
        <h2 className='text-xl font-semibold text-white mb-4'>Ваша реферальная ссылка</h2>
        <div className='flex flex-col sm:flex-row gap-4 items-start sm:items-end'>
          <TextField
            value={refInfo?.link || ''}
            variant='outlined'
            size='small'
            InputProps={{
              readOnly: true,
              sx: {
                minWidth: '300px',
                backgroundColor: 'rgba(30, 30, 30, 0.8)',
                color: 'white',
                borderRadius: '12px',
                width: '200px',
                '& .MuiOutlinedInput-notchedOutline': {
                  borderColor: 'rgba(255, 255, 255, 0.15)',
                },
              },
            }}
          />
        </div>
        <p className='text-gray-400 text-sm mt-3'>Отправьте эту ссылку другу.</p>
      </div>

      <div className='grid grid-cols-1 md:grid-cols-2 gap-6 mb-8'>
        <div className='bg-gray-800 rounded-xl p-5 flex flex-col items-center'>
          <p className='text-3xl font-bold text-blue-400'>{refInfo?.referralsCount || 0}</p>
          <p className='text-gray-400 mt-1 text-center'>Всего приглашено</p>
        </div>
        <div className='bg-gray-800 rounded-xl p-5 flex flex-col items-center'>
          <p className='text-3xl font-bold text-yellow-400'>{refInfo?.earnedFromReferrals || 0} ₽</p>
          <p className='text-gray-400 mt-1 text-center'>Уже зачислено</p>
        </div>
      </div>

      <div className='bg-gray-800 rounded-xl p-6'>
        <h2 className='text-xl font-semibold text-white mb-4'>Как это работает?</h2>
        <div className='space-y-4'>
          {[1, 2, 3].map((step) => (
            <div key={step} className='flex'>
              <div className='flex-shrink-0 w-8 h-8 rounded-full bg-gray-700 flex items-center justify-center text-sm font-medium text-gray-300 mr-3'>
                {step}
              </div>
              <p className='text-gray-300'>
                {step === 1 && 'Поделитесь своей ссылкой с друзьями'}
                {step === 2 && 'Друг регистрируется предварительно перейдя по ссылке'}
                {step === 3 && 'После его регистрации бонусы зачисляются вам автоматически'}
              </p>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}

export default ReferralScreen
