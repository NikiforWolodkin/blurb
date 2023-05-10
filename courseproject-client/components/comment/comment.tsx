'use client';

import React, { useReducer, useState } from "react";
import { AiFillHeart, AiOutlineHeart } from "react-icons/ai";
import { BiComment, BiShareAlt } from "react-icons/bi";
import Image from 'next/image';
import colorsBorder from "../colorsBorder";
import Post from "@/types/post";
import avatars from "../avatars";
import Comment from "@/types/comment";
import { useRouter } from "next/navigation";

interface ICommentProps {
    comment: Comment
}

const Comment: React.FC<ICommentProps> = ({ comment }) => {
    return (
        <div
            className="bg-zinc-900 shadow-lg shadow-zinc-700/10 border border-zinc-700 rounded-lg mt-4 w-full"
        >
            <div className="flex ml-3 mt-3">
                <div className={(colorsBorder[comment.authorProfileColor] === undefined ? "border-red-700 " : colorsBorder[comment.authorProfileColor])
                    + "border rounded-full flex justify-center items-center p-1 w-12 h-12"}>
                    <Image
                        className="rounded-full"
                        src={`/../public/${(avatars[comment.authorAvatar] === undefined) ? avatars.Basic : avatars[comment.authorAvatar]}`} 
                        width="100"
                        height="100"
                    />
                </div>
                <div className="flex flex-col items-start ml-2">
                    <div className="font-bold text-lg">
                        {comment.authorUsername + (comment.authorStatus === "BANNED" ? " (Blocked)" : "")}
                    </div>
                    <div className="text-zinc-500 text-sm">
                        Posted on {new Date(comment.creationTime).toDateString()}
                    </div>
                </div>
            </div>

            <div className="mx-3 mt-3 mb-3">
                {comment.text}
            </div>
        </div>
    );
};

export default Comment;