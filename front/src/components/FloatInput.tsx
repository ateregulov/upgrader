import { TextField, TextFieldProps } from '@mui/material';
import { useEffect, useState } from 'react';

interface FloatInputProps extends Omit<TextFieldProps, 'onChange'> {
  value: number | '';
  onChange: (value: number) => void;
  maxValue?: number;
  minValue?: number;
}

const FloatInput: React.FC<FloatInputProps> = ({ value, onChange, maxValue, minValue, ...props }) => {
  const [displayValue, setDisplayValue] = useState(value.toString());

  useEffect(() => {
    setDisplayValue(value.toString());
  }, [value]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;
    
    // Allow empty input
    if (newValue === '') {
      setDisplayValue('');
      onChange(0);
      return;
    }

    // Allow only numbers, one decimal point and minus sign
    if (!/^-?\d*\.?\d*$/.test(newValue)) {
      return;
    }

    setDisplayValue(newValue);

    // Don't update parent if we're in an intermediate state
    if (newValue === '-' || newValue === '.' || newValue === '-.' || newValue.endsWith('.')) {
      return;
    }

    const numValue = Number(newValue);
    if (!isNaN(numValue)) {
      // Only apply min/max validation if the number is not zero
      if (numValue !== 0) {
        if (maxValue !== undefined && numValue > maxValue) {
          onChange(maxValue);
          return;
        }
        if (minValue !== undefined && numValue < minValue) {
          onChange(minValue);
          return;
        }
      }
      onChange(numValue);
    }
  };

  const handleBlur = () => {
    const numValue = Number(displayValue);
    if (!isNaN(numValue)) {
      // Only apply min/max validation if the number is not zero
      if (numValue !== 0) {
        if (maxValue !== undefined && numValue > maxValue) {
          onChange(maxValue);
        } else if (minValue !== undefined && numValue < minValue) {
          onChange(minValue);
        } else {
          onChange(numValue);
        }
      } else {
        onChange(0);
      }
    } else {
      onChange(0);
    }
  };

  return (
    <TextField
      type="text"
      inputMode="decimal"
      value={displayValue}
      onChange={handleChange}
      onBlur={handleBlur}
      {...props}
    />
  );
};

export default FloatInput; 