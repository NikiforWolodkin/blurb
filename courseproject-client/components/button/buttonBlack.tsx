import React from "react";

interface IButtonProps {
    text: string,
    handleClick: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void
}

const ButtonBlack: React.FC<IButtonProps> = ({ text, handleClick }) => {
    return (
        <div className="flex justify-center w-full">
            <button 
                className="bg-zinc-950 text-black shadow-lg border border-zinc-700 shadow-zinc-800/10 rounded-lg text-xl text-white font-bold max-w-xs mx-3 my-4 w-full h-12"
                onClick={handleClick}
            >
                {text} 
            </button>
        </div>
    );
};

export default ButtonBlack;