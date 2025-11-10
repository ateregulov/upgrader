import { useState, useEffect, useCallback } from 'react'

export const SEED_PHRASE_KEY = 'seedPhrase'
export const PRIVATE_KEY_KEY = 'privateKey'
export const CREATION_METHOD_BEGINNER_KEY = 'beginner'

function useSessionStorage<T>() {
  const getSessionStorageValue = useCallback((key: string): T | null => {
    try {
      const item = sessionStorage.getItem(key)
      return item ? (JSON.parse(item) as T) : null
    } catch (error) {
      console.error('Error parsing JSON from sessionStorage:', error)
      return null
    }
  }, [])

  const setSessionStorageValue = useCallback((key: string, value: T | null) => {
    try {
      if (value === null) {
        sessionStorage.removeItem(key)
      } else {
        sessionStorage.setItem(key, JSON.stringify(value))
      }
    } catch (error) {
      console.error('Error saving to sessionStorage:', error)
    }
  }, [])

  const removeSessionStorageValue = useCallback((key: string) => {
    try {
      sessionStorage.removeItem(key)
    } catch (error) {
      console.error('Error removing from sessionStorage:', error)
    }
  }, [])

  return { getSessionStorageValue, setSessionStorageValue, removeSessionStorageValue }
}

export default useSessionStorage
