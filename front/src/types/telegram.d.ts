interface TelegramCloudStorage {
    getItem: (key: string) => Promise<string | null>;
    setItem: (key: string, value: string) => Promise<void>;
  }
  
  interface TelegramWebApp {
    initData?: string;
    CloudStorage?: TelegramCloudStorage;
  }
  
  interface TelegramNamespace {
    WebApp?: TelegramWebApp;
  }
  
  interface Window {
    Telegram?: TelegramNamespace;
  }
  