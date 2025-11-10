import { useState, useEffect, useCallback } from 'react'
import useLocalStorage from './useLocalStorage'
import { WALLET } from './useLocalStorage'

export interface MnemonicAccountInfo {
  Hash: string
  HashHex: string
  PublicKey: string
  PrivateKey: string
}

const useWallet = () => {
  const { getLocalStorageValue, setLocalStorageValue } = useLocalStorage()
  const [wallet, setWalletState] = useState<MnemonicAccountInfo | null>(null)
  const [loadingWallet, setLoadingWallet] = useState<boolean>(true)

  useEffect(() => {
    const start = performance.now()
    const storedWallet = getLocalStorageValue(WALLET) as MnemonicAccountInfo | null
    setWalletState(storedWallet)
    setLoadingWallet(false)
    const end = performance.now()
    console.log('useWallet time:', end - start)
  }, [getLocalStorageValue])

  const setWallet = useCallback(
    (newWallet: MnemonicAccountInfo | null) => {
      setWalletState(newWallet)
      setLocalStorageValue(WALLET, newWallet)
    },
    [setLocalStorageValue],
  )

  return {
    wallet,
    setWallet,
    loadingWallet,
  }
}

export default useWallet
