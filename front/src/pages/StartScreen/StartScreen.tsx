import { Button } from '@mui/material'
import config from '../../../config'
import { useNavigate } from 'react-router-dom'

const StartScreen: React.FC = () => {
  const { AppVersion } = config
  const navigate = useNavigate()

  const handleStartBtn = () => {
    navigate('/courses')
  }

  return (
    <div className='relative h-screen w-screen bg-gradient-to-br from-black to-gray-900 text-white flex flex-col items-center justify-center px-6 text-center'>
      <h1 className='text-2xl font-semibold max-w-md mb-6'>
        Прокачай мышление. Уверенность. Финансы. Отношения. Одна игра, чтобы стать лучше.
      </h1>

      <Button
        onClick={handleStartBtn}
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
        Начать
      </Button>

      <div className='absolute bottom-4 left-4 text-gray-400 text-xs'>v{AppVersion}</div>
    </div>
  )
}

export default StartScreen
