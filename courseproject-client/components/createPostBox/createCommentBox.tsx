'use client';

import React, { useState, useRef } from "react";
import useAutoresizeTextarea from "./useAutoresizeTextarea";
import { IoSend } from "react-icons/io5";
import { useRouter } from "next/navigation";
import Comment from "@/types/comment";

interface ICreateCommentBoxProps {
    setError: React.Dispatch<React.SetStateAction<string>>,
    setComments: React.Dispatch<React.SetStateAction<Comment[]>>,
    incrementCommentCount: () => void,
    comments: Comment[]
    id: number
}

const CreateCommentBox: React.FC<ICreateCommentBoxProps> = ({ setError, setComments, incrementCommentCount, comments, id }) => {
    const router = useRouter();

    const [text, setText] = useState<string>("");
    
    const textAreaRef = useRef<HTMLTextAreaElement>(null);

    useAutoresizeTextarea(textAreaRef.current, text);

    const handleTextChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        const value = e.target?.value;
    
        setText(value);
    };

    const createPost = async () => {
        if (text === "") {
            return;
        }

        const response = await fetch(`/blurb-api/posts/${id}/comments`, {
            method: "POST",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem('token')}`
            },
            body: JSON.stringify({text}),
            cache: 'no-store'
        });

        if (!response.ok) {
            setError(await response.text());
            return;
        }

        const comment = await response.json();
        setText("");
        setComments([comment, ...comments]);
        incrementCommentCount();
    };

    return (
        <div className="flex justify-center w-full">
            <div className="bg-zinc-900 border border-zinc-700 flex flex-row justify-center items-end rounded-lg p-3 mt-4 mx-4 max-w-4xl w-full">
                <textarea 
                    className="text-xl bg-transparent outline-none resize-none overflow-hidden border-b border-zinc-700 pb-0.5 w-full"
                    placeholder="What do you think?"
                    onChange={handleTextChange}
                    value={text}
                    ref={textAreaRef}
                    rows={1}
                />
                <div 
                    className="cursor-pointer text-2xl text-red-700 hover:text-red-400 ml-2 mb-1"
                    onClick={createPost}   
                >
                    <IoSend />
                </div>
                
            </div>
        </div>
    );
};

export default CreateCommentBox;