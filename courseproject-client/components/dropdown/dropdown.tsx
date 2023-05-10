import { useState } from 'react';

interface IDropDownProps {
    zIndex: boolean,
    options: string[],
    setSelectedOption: React.Dispatch<React.SetStateAction<string>>,
    selectedOption: string,
}

const Dropdown:React.FC<IDropDownProps> = ({ zIndex, options, selectedOption, setSelectedOption }) => {
  const [isOpen, setIsOpen] = useState<boolean>(false);

  const handleOptionClick = (option: string) => {
    setSelectedOption(option);
    setIsOpen(false);
  };

  return (
    <div className="relative">
      <div className="flex justify-center w-full">
      <button
        className="bg-zinc-900 border text-left pl-6 border-zinc-700 shadow-md shadow-zinc-700/10 rounded-lg text-xl font-bold max-w-xs mx-3 mt-4 w-full h-12"
        onClick={() => setIsOpen(!isOpen)}
      >
        {selectedOption || 'Select an option'}
      </button>
      </div>
      {isOpen && (
        <ul className={(zIndex === true ? "z-50 " : "") + "absolute bg-zinc-900 border border-zinc-700 shadow-md shadow-zinc-700/20 font-bold text-xl rounded-md p-2 mx-3 mt-2"}>
          {options.map((option) => (
            <li
              key={option}
              className="px-4 py-2 hover:bg-zinc-800 rounded-md cursor-pointer"
              onClick={() => handleOptionClick(option)}
            >
              {option}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default Dropdown;