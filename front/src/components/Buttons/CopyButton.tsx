import { Button } from '@mui/material'
import { CheckIcon, ClipboardIcon } from 'lucide-react'
import { useState } from 'react'

export interface CopyButtonProps {
  valueForCopy: string
}

function CopyButton(props: CopyButtonProps) {
  const [isCopied, setIsCopied] = useState<boolean>(false)

  const handleCopyClick = () => {
    navigator.clipboard.writeText(props.valueForCopy)
    setIsCopied(true)
    setTimeout(() => setIsCopied(false), 800)
  }
  return (
    <Button
      onClick={handleCopyClick}
      sx={{ padding: 0 }}
      variant='text'
      startIcon={isCopied ? <CheckIcon /> : <ClipboardIcon />}
    />
  )
}

export default CopyButton
