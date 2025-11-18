import Api from '../../api'
import { createContext, ReactNode, useContext, useEffect, useState } from 'react'

interface BalanceContext {
  balance: number
  setBalance: (balance: number) => void
}

const balanceContext = createContext<BalanceContext | undefined>(undefined)

export const BalanceProvider = ({ children }: { children: ReactNode }) => {
  const [balance, setBalance] = useState(0)

  useEffect(() => {
    getBalance()
  }, [])

  const getBalance = async () => {
    try {
      const newBalance = await Api.getBalance()
      setBalance(newBalance)
    } catch (error) {}
  }

  return <balanceContext.Provider value={{ balance, setBalance }}>{children}</balanceContext.Provider>
}

export const useBalance = () => {
  const ctx = useContext(balanceContext)
  if (!ctx) throw new Error('useBalance должен использоваться внутри BalanceProvider')
  return ctx
}
