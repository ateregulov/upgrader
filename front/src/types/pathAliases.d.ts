import { MnemonicAccountInfo } from '../hooks/useWallet'

declare module '../hooksuseSessionStorage' {
  function useSessionStorage<T>(): {
    getSessionStorageValue: (key: string) => T | null
    setSessionStorageValue: (key: string, value: T | null) => void
    removeSessionStorageValue: (key: string) => void
  }

  export default useSessionStorage
}

declare module '../hooksuseLocalStorage' {
  function useLocalStorage<T>(): {
    getLocalStorageValue: (key: string) => T | null
    setLocalStorageValue: (key: string, value: T | null) => void
    removeLocalStorageValue: (key: string) => void
  }

  export default useLocalStorage
}

declare module '../hookssessionStorageKeys' {
  export const SEED_PHRASE_KEY: string
  export const PRIVATE_KEY_KEY: string
}

declare module 'TronWeb' {
  const TronWeb: any
  export default TronWeb
}

declare module 'main' {
  const main: any
  export default main
}

declare module '../hooksuseTronWeb' {
  export const useTronWeb: () => any | null
}
declare module '../hooksuseWallet' {
  export const useWallet: () => MnemonicAccountInfo | null
}
