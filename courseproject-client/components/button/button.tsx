import React from "react";

interface IButtonProps {
    text: string,
    handleClick: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void
}

const Button: React.FC<IButtonProps> = ({ text, handleClick }) => {
    return (
        <div className="flex justify-center w-full">
            <button 
                className="bg-zinc-100 text-black shadow-lg shadow-zinc-100/10 rounded-lg text-xl font-bold max-w-xs mx-3 mt-4 w-full h-12"
                onClick={handleClick}
            >
                {text} 
            </button>
        </div>
    );
};

export default Button;