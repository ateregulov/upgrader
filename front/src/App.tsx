import { useEffect, useState } from 'react'
import { Route, Routes, useLocation } from 'react-router-dom'
import Loader from '@/common/Loader'
import PageTitle from '@/components/PageTitle'
import DefaultLayout from './layout/DefaultLayout'
import StartScreenLayout from './layout/StartScreenLayout'
import StartScreen from './pages/StartScreen/StartScreen'

import { ToastProvider, ToastViewport } from './components/toast'
import { ToastContainer } from './hooks/use-toast'
import CourseTasks from './pages/Tasks/CourseTasks'
import Courses from './pages/Courses/Courses'
import TaskPage from './pages/Tasks/TaskPage'
import CourseDetails from './pages/Courses/CourseDetails'
import Payment from './pages/Courses/Payment'
import WelcomeBonusScreen from './pages/Bonus/WelcomeBonusScreen'
import RegistrationLayout from './layout/RegistrationLayout'


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
          path='/'
          element={
            <StartScreenLayout>
              <PageTitle title='Start Screen' />
              <StartScreen />
            </StartScreenLayout>
          }
        />
        <Route
          path='/welcome-bonus'
          element={
            <StartScreenLayout>
              <PageTitle title='Register' />
              <WelcomeBonusScreen />
            </StartScreenLayout>
          }
        />
        <Route
          path='/courses'
          element={
            <DefaultLayout>
              <PageTitle title='Courses' />
              <Courses />
            </DefaultLayout>
          }
        />
        <Route
          path='/courses/:courseId'
          element={
            <DefaultLayout>
              <PageTitle title='Course Details' />
              <CourseDetails />
            </DefaultLayout>
          }
        />
        <Route
          path='/payment/:courseId'
          element={
            <DefaultLayout>
              <PageTitle title='Payment' />
              <Payment />
            </DefaultLayout>
          }
        />
        <Route
          path='/tasks/:courseId'
          element={
            <DefaultLayout>
              <PageTitle title='Course Tasks' />
              <CourseTasks />
            </DefaultLayout>
          }
        />
        <Route
          path='/tasks/:courseId/:taskId'
          element={
            <DefaultLayout>
              <PageTitle title='Task' />
              <TaskPage />
            </DefaultLayout>
          }
        />
      </Routes>
    </ToastProvider>
  )
}

export default App
