import { useState } from 'react';

interface CheckboxOneProps {
  id: string;
  checked: boolean;
  onChange: (checked: boolean) => void;
  label: string;
}

const CheckboxOne: React.FC<CheckboxOneProps> = ({ id, checked, onChange, label }) => {
  return (
    <div>
      <label
        htmlFor={id}
        className="flex cursor-pointer select-none items-center"
      >
        <div className="relative">
          <input
            type="checkbox"
            id={id}
            className="sr-only"
            checked={checked}
            onChange={() => {
              onChange(!checked);
            }}
          />
          <div
            className={`mr-4 flex h-5 w-5 items-center justify-center rounded border ${
              checked && 'border-primary bg-gray dark:bg-transparent'
            }`}
          >
            <span
              className={`h-2.5 w-2.5 rounded-sm ${checked && 'bg-primary'}`}
            ></span>
          </div>
        </div>
        {label}
      </label>
    </div>
  );
};

export default CheckboxOne;
