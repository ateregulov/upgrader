import React from 'react';

interface StartScreenLayoutProps {
  children: React.ReactNode;
}

const StartScreenLayout: React.FC<StartScreenLayoutProps> = ({ children }) => {
  return (
    <div className="min-h-screen">
      {children}
    </div>
  );
};

export default StartScreenLayout; 