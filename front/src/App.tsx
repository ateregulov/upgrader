import { useEffect, useState } from 'react'
import { Route, Routes, useLocation } from 'react-router-dom'
import Loader from '@/common/Loader'
import PageTitle from '@/components/PageTitle'
import DefaultLayout from './layout/DefaultLayout'
import StartScreenLayout from './layout/StartScreenLayout'
import StartScreen from './pages/StartScreen'

import { ToastProvider, ToastViewport } from './components/toast'
import { ToastContainer } from './hooks/use-toast'


function App() {
  const [loading, setLoading] = useState<boolean>(true)
  const { pathname } = useLocation()

  useEffect(() => {
    window.scrollTo(0, 0)
  }, [pathname])

  useEffect(() => {
    setTimeout(() => setLoading(false), 1000)
  }, [])

  return loading ? (
    <Loader />
  ) : (
    <ToastProvider>
      <ToastViewport />
      <ToastContainer />
      <Routes>
        <Route
          path="/"
          element={
            <StartScreenLayout>
              <PageTitle title="Start Screen" />
              <StartScreen />
            </StartScreenLayout>
          }
        />
      </Routes>
    </ToastProvider>
  )
}

export default App
