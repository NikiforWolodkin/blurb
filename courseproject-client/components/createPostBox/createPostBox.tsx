'use client';

import React, { useState, useRef } from "react";
import useAutoresizeTextarea from "./useAutoresizeTextarea";
import { IoSend } from "react-icons/io5";

const CreatePostBox: React.FC = () => {
    const [text, setText] = useState<string>("");
    const textAreaRef = useRef<HTMLTextAreaElement>(null);

    useAutoresizeTextarea(textAreaRef.current, text);

    const handleTextChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        const value = e.target?.value;
    
        setText(value);
    };

    return (
        <div className="flex justify-center w-full">
            <div className="bg-zinc-900 border border-zinc-700 flex flex-row justify-center items-end rounded-lg p-3 mt-4 mx-4 max-w-4xl w-full">
                <textarea 
                    className="text-xl bg-transparent outline-none resize-none overflow-hidden border-b border-zinc-700 pb-0.5 w-full"
                    placeholder="What's on your mind?"
                    onChange={handleTextChange}
                    value={text}
                    ref={textAreaRef}
                    rows={1}
                />
                <div className="cursor-pointer text-2xl text-red-700 hover:text-red-400 ml-2 mb-1">
                    <IoSend />
                </div>
                
            </div>
        </div>
    );
};

export default CreatePostBox;