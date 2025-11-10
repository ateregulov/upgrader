import React from 'react';
import config from '../../config';

const StartScreen: React.FC = () => {
  const { AppVersion } = config;

  return (
    <div className="relative h-screen w-screen bg-gradient-to-br from-black to-gray-900 text-white flex flex-col">
      {/* Основной контент */}
      <div className="flex-1 flex flex-col items-center justify-center px-8">
        {/* Версия приложения */}
        <div className="absolute bottom-4 left-4 text-gray-400 text-xs">
          v{AppVersion}
        </div>
      </div>
    </div>
  );
};

export default StartScreen; 