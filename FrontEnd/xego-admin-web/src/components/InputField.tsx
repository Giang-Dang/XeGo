import { useState } from "react";


interface InputFieldProps {
  id: string;
  name: string;
  type: string;
  autoComplete: string;
  required: boolean;
  placeholder: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  validation?: (value: string) => string | null; // Add this line
}

const InputField: React.FC<InputFieldProps> = ({
  id,
  name,
  type,
  autoComplete,
  required,
  placeholder,
  value,
  onChange,
  validation,
}) => {
  const [error, setError] = useState<string | null>(null);

  const handleBlur = () => {
    if (validation) {
      setError(validation(value));
    }
  };

  return (
    <div>
      <label htmlFor={id} className="sr-only">
        {placeholder}
      </label>
      <input
        id={id}
        name={name}
        type={type}
        autoComplete={autoComplete}
        required={required}
        className="appearance-none rounded-none relative block w-full px-3 py-2 
        border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md 
        focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        onBlur={handleBlur}
      />
      {error && <p className="text-red-500 text-xs mt-1">{error}</p>}
    </div>
  );
};

export default InputField;
