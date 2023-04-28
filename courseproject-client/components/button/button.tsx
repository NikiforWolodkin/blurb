import React from "react";

interface IButtonProps {
    text: string,
}

const Button: React.FC<IButtonProps> = ({ text }) => {
    return (
        <div className="flex justify-center w-full">
            <button className="bg-zinc-100 text-black shadow-lg shadow-zinc-100/10 rounded-lg text-xl font-bold max-w-xs m-4 w-full h-12">
                {text} 
            </button>
        </div>
    );
};

export default Button;