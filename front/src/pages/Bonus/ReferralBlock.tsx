import { toast } from '@/hooks/use-toast'
import Api from '../../../api'
import { useEffect, useState } from 'react'
import CopyButton from '@/components/Buttons/CopyButton'

function ReferralBlock() {
  const [refLink, setRefLink] = useState<string>()

  useEffect(() => {
    getReferralLink()
  }, [])

  const getReferralLink = async () => {
    try {
      const link = await Api.getReferralLink()
      setRefLink(link)
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: 'Не удалось загрузить ссылку. Попробуйте обновить страницу.',
        variant: 'error',
      })
    }
  }

  return (
    <div className='p-8'>
      <div className='bg-gray-800 rounded-xl p-6 mb-8'>
        <p className='text-gray-300'>Зарабатывайте вместе с нашей партнерской программой</p>
      </div>

      <div className='bg-gray-800 rounded-xl p-6'>
        <h2 className='text-xl font-semibold text-white mb-4'>Ваша реферальная ссылка</h2>

        <div className='flex gap-4 justify-center sm:items-end'>
          <input
            disabled
            type='text'
            readOnly
            value={refLink ?? ''}
            className='bg-gray-700 text-white rounded-xl px-3 py-2 w-full sm:w-[400px] 
                       border border-gray-600 focus:border-gray-400 outline-none'
          />
          <div className='self-center'>
            <CopyButton valueForCopy={refLink ?? ''} />
          </div>
        </div>
      </div>
    </div>
  )
}

export default ReferralBlock
