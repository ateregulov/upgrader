import React, { useEffect, useState } from 'react';

declare global {
  interface Window {
    Telegram?: {
      WebApp?: {
        initData?: string;
      }
    }
  }
}


const TestTg: React.FC = () => {
    const [webApp, setWebApp] = useState<any>(null);

    useEffect(() => {
        console.log("Window.Telegram:", window.Telegram);
        console.log("Window.Telegram.WebApp:", window.Telegram?.WebApp);
        
        const tg = window.Telegram?.WebApp;
        if (tg) {
            console.log("Initializing Telegram WebApp...");

            setWebApp(tg);
            console.log("Telegram WebApp initialized:", tg);
        } else {
            console.warn("Telegram WebApp not found in window.Telegram");
        }
    }, []);
    
    return (
        <div>
            <h2>Telegram WebApp Debug Info:</h2>
            <div>
                <h3>Window.Telegram exists:</h3>
                <pre>{String(!!window.Telegram)}</pre>
            </div>
            <div>
                <h3>Window.Telegram.WebApp exists:</h3>
                <pre>{String(!!window.Telegram?.WebApp)}</pre>
            </div>
            <div>
                <h3>WebApp Object:</h3>
                <pre>{JSON.stringify(webApp, null, 2)}</pre>
            </div>
            <div>
                <h3>Init Data:</h3>
                <pre>{JSON.stringify(webApp?.initData, null, 2)}</pre>
            </div>
        </div>
    );
};

export default TestTg; 