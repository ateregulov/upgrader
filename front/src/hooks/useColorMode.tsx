import { useEffect, useState } from 'react'
import useLocalStorage from './useLocalStorage'
import { COLOR_THEME } from './useLocalStorage'

const useColorMode = () => {
  const { getLocalStorageValue, setLocalStorageValue } = useLocalStorage()
  const [colorMode, setColorMode] = useState(getLocalStorageValue(COLOR_THEME))

  useEffect(() => {
    const className = 'dark'
    const bodyClass = window.document.body.classList

    colorMode === 'dark' ? bodyClass.add(className) : bodyClass.remove(className)
    setLocalStorageValue(COLOR_THEME, colorMode)
  }, [colorMode])

  return [colorMode, setColorMode]
}

export default useColorMode
