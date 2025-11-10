import config from '../../config'
import React, { ReactNode } from 'react'

const RegistrationLayout: React.FC<{ children: ReactNode }> = ({ children }) => {
  const { AppVersion } = config

  return (
    <div className='dark:bg-boxdark-2 dark:text-bodydark'>
      {/* <!-- ===== Page Wrapper Start ===== --> */}
      <div className='flex h-screen overflow-hidden'>
        {/* <!-- ===== Content Area Start ===== --> */}
        <div className='relative flex flex-1 flex-col overflow-y-auto overflow-x-hidden'>
          {/* <!-- ===== Main Content Start ===== --> */}
          <main>
            <div className='mx-auto max-w-screen-2xl p-4 md:p-6 2xl:p-10'>{children}</div>
          </main>
          {/* <!-- ===== Main Content End ===== --> */}
          
          {/* <!-- ===== Footer Start ===== --> */}
          <footer className='sticky bottom-0 flex justify-center p-4 text-sm text-gray-500'>
            Version {AppVersion}
          </footer>
          {/* <!-- ===== Footer End ===== --> */}
        </div>
        {/* <!-- ===== Content Area End ===== --> */}
      </div>
      {/* <!-- ===== Page Wrapper End ===== --> */}
    </div>
  )
}

export default RegistrationLayout
