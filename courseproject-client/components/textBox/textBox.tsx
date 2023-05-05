'use client';

import React, { ChangeEvent } from "react";

interface IButtonProps {
    type: string,
    placeholder: string,
    value: string,
    handleChange: (event: ChangeEvent<HTMLInputElement>) => void
}

const TextBox: React.FC<IButtonProps> = ({ type, placeholder, value, handleChange }) => {
    return (
        <div className="flex justify-center w-full">
            <input
                type={type}
                placeholder={placeholder}
                value={value}
                onChange={handleChange}
                className="outline-none bg-zinc-900 shadow-lg shadow-zinc-700/10 rounded-lg text-xl max-w-xs px-4 mt-4 mx-4 w-full h-12"
            />
        </div>
    );
};

export default TextBox;