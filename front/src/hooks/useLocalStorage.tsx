import { useState, useEffect, useCallback } from 'react'

export const ALERT_SETTINGS = 'alertSettings'
export const COLOR_THEME = 'color-theme'
export const WALLET = 'wallet'

function useLocalStorage() {
  const getLocalStorageValue = useCallback(function <T>(key: string): T | null {
    try {
      const item = localStorage.getItem(key)
      return item ? (JSON.parse(item) as T) : null
    } catch (error) {
      console.error(`Error parsing JSON from localStorage for key "${key}":`, error)
      return null
    }
  }, [])

  const setLocalStorageValue = useCallback(function <T>(key: string, value: T | null) {
    try {
      if (value === null) {
        localStorage.removeItem(key)
      } else {
        localStorage.setItem(key, JSON.stringify(value))
      }
    } catch (error) {
      console.error(`Error saving to localStorage for key "${key}":`, error)
    }
  }, [])

  const removeLocalStorageValue = useCallback((key: string) => {
    localStorage.removeItem(key)
  }, [])

  return { getLocalStorageValue, setLocalStorageValue, removeLocalStorageValue }
}

export default useLocalStorage
