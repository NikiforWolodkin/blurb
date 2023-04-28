'use client';

import React, { useState } from "react";
import { AiFillHeart, AiOutlineHeart } from "react-icons/ai";
import { BiComment, BiShareAlt } from "react-icons/bi";
import Image from 'next/image';

interface IPostProps {
    author: string,
    text: string,
    date: number,
    likes: number,
    comments: number,
    shares: number,
}

const Post: React.FC<IPostProps> = ({ author, text, date, likes, comments, shares }) => {
    const [isLiked, setIsLiked] = useState<boolean>(false);

    return (
        <div
            className="bg-zinc-900 shadow-lg shadow-zinc-700/10 border border-zinc-700 rounded-lg mt-4 w-full"
        >
            <div className="flex ml-3 mt-3">
                <div className="border border-red-700 rounded-full flex justify-center items-center p-1 w-12 h-12">
                    <Image
                        className="rounded-full"
                        src="/../public/avatars/basic.jpg"
                        width="100"
                        height="100"
                    />
                </div>
                <div className="flex flex-col items-start ml-2">
                    <div className="font-bold text-lg">
                        {author}
                    </div>
                    <div className="text-zinc-500 text-sm">
                        Posted on {new Date(date).toDateString()}
                    </div>
                </div>
            </div>

            <div className="mx-3 mt-3">
                {text}
            </div>

            <div className="text-2xl flex ml-3 mt-3 mb-3 w-full">
                <div
                    className="cursor-pointer"
                    onClick={() => setIsLiked(!isLiked)}
                >
                    {isLiked === false ?
                        <div className="hover:text-red-400 flex flex-col items-center">
                            <AiOutlineHeart />
                            <div className="text-base">{likes}</div>
                        </div> :
                        <div className="text-red-700 hover:text-red-400 flex flex-col items-center">
                            <AiFillHeart />
                            <div className="text-base">{likes + 1}</div>
                        </div>
                    }
                </div>
                <div className="cursor-pointer ml-2 flex flex-col items-center hover:text-red-400">
                    <BiComment />
                    <div className="text-base">{comments}</div>
                </div>
                <div className="cursor-pointer ml-2 flex flex-col items-center hover:text-red-400">
                    <BiShareAlt />
                    <div className="text-base">{shares}</div>
                </div>
            </div>
        </div>
    );
};

export default Post;