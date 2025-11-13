import React, { useState, ReactNode } from 'react'
import Header from '../components/Header/index'
import Sidebar from '../components/Sidebar/index'
import config from '../../config'
import { BalanceProvider } from '@/Contexts/BalanceContext'

const DefaultLayout: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [sidebarOpen, setSidebarOpen] = useState(false)
  const { AppVersion } = config

  return (
    <BalanceProvider>
      <div className='dark:bg-boxdark-2 dark:text-bodydark'>
        {/* <!-- ===== Page Wrapper Start ===== --> */}
        <div className='flex h-screen overflow-hidden'>
          {/* <!-- ===== Sidebar Start ===== --> */}
          <Sidebar sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />
          {/* <!-- ===== Sidebar End ===== --> */}

          {/* <!-- ===== Content Area Start ===== --> */}
          <div className='relative flex flex-1 flex-col overflow-y-auto overflow-x-hidden'>
            {/* <!-- ===== Header Start ===== --> */}
            <Header sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />
            {/* <!-- ===== Header End ===== --> */}

            {/* <!-- ===== Main Content Start ===== --> */}
            <main className='flex-grow'>
              <div className='mx-auto max-w-screen-2xl p-4 md:p-6 2xl:p-10'>{children}</div>
            </main>
            {/* <!-- ===== Main Content End ===== --> */}

            {/* <!-- ===== Footer Start ===== --> */}
            <footer className='mt-auto flex justify-center p-4 text-sm text-gray-500'>Version {AppVersion}</footer>
            {/* <!-- ===== Footer End ===== --> */}
          </div>
          {/* <!-- ===== Content Area End ===== --> */}
        </div>
        {/* <!-- ===== Page Wrapper End ===== --> */}
      </div>
    </BalanceProvider>
  )
}

export default DefaultLayout
