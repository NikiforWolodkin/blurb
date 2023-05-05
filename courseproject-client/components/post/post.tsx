'use client';

import React, { useState } from "react";
import { AiFillHeart, AiOutlineHeart } from "react-icons/ai";
import { BiComment, BiShareAlt } from "react-icons/bi";
import { MdOutlineReport } from "react-icons/md";
import Image from 'next/image';
import colorsBorder from "../colorsBorder";
import Post from "@/types/post";
import avatars from "../avatars";
import { useRouter } from "next/navigation";

interface IPostProps {
    post: Post
}

const Post: React.FC<IPostProps> = ({ post }) => {
    const [isLiked, setIsLiked] = useState<boolean>(post.isLiked);
    const [likeCount, setLikeCount] = useState<number>(post.likeCount);
    const [shareCount, setShareCount] = useState<number>(post.shareCount);
    const [reportCount, setReportCount] = useState<number>(post.reportCount);
    const [shared, setShared] = useState<boolean>(false);

    const router = useRouter();

    const navigateToComments = () => {
        router.push(`/posts/${post.id}`);
    };

    const navigateToUser = () => {
        router.push(`/users/${post.authorId}`);
    };

    const report = async () => {
        const response = await fetch(`/blurb-api/posts/${post.id}/report`, {
            method: "POST",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem('token')}`
            },
            cache: 'no-store'
        });

        if (!response.ok) {
            return;
        }

        const reports = parseInt(await response.text());
        setReportCount(reports);
    };

    const like = async () => {
        setIsLiked(true);

        const response = await fetch(`/blurb-api/posts/${post.id}/like`, {
            method: "POST",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem('token')}`
            },
            cache: 'no-store'
        });

        if (!response.ok) {
            return;
        }

        const likes = parseInt(await response.text());
        setLikeCount(likes);
    };

    const unlike = async () => {
        setIsLiked(false);

        const response = await  fetch(`/blurb-api/posts/${post.id}/like`, {
            method: "DELETE",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem('token')}`
            },
            cache: 'no-store'
        });

        const likes = parseInt(await response.text());
        setLikeCount(likes);
    };

    const share = async () => {
        setShared(true);
    
        const response = await fetch(`/blurb-api/posts/${post.id}/share`, {
            method: "POST",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem('token')}`
            },
            cache: 'no-store'
        });

        if (!response.ok) {
            return;
        }

        const shares = parseInt(await response.text());
        setShareCount(shares);
    };

    return (
        <div
            className="bg-zinc-900 shadow-lg shadow-zinc-700/10 border border-zinc-700 rounded-lg mt-4 w-full"
        >
            <div className="flex ml-3 mt-3">
                <div 
                    className={(colorsBorder[post.authorProfileColor] === undefined ? "border-red-700 " : colorsBorder[post.authorProfileColor])
                    + "border rounded-full flex justify-center items-center p-1 w-12 h-12 cursor-pointer"}
                    onClick={navigateToUser}
                >
                    <Image
                        alt="User avatar"
                        className="rounded-full"
                        src={`/../public/${(avatars[post.authorAvatar] === undefined) ? avatars.Basic : avatars[post.authorAvatar]}`} 
                        width="100"
                        height="100"
                        
                    />
                </div>
                <div className="flex flex-col items-start ml-2">
                    <div className="font-bold text-lg">
                        {post.authorUsername}
                    </div>
                    <div className="text-zinc-500 text-sm">
                        Posted on {new Date(post.creationTime).toDateString()}
                    </div>
                </div>
            </div>

            <div className="mx-3 mt-3">
                {post.text}
            </div>

            <div className="text-2xl flex ml-3 mt-3 mb-3 w-full">
                <div
                    className="cursor-pointer"
                    onClick={() => setIsLiked(!isLiked)}
                >
                    {isLiked === false ?
                        <div 
                            className="hover:text-red-400 flex flex-col items-center"
                            onClick={like}
                        >
                            <AiOutlineHeart />
                            <div className="text-base">{likeCount}</div>
                        </div> :
                        <div 
                            className="text-red-700 hover:text-red-400 flex flex-col items-center"
                            onClick={unlike}
                        >
                            <AiFillHeart />
                            <div className="text-base">{likeCount}</div>
                        </div>
                    }
                </div>
                <div 
                    className="cursor-pointer ml-2 flex flex-col items-center hover:text-red-400"
                    onClick={navigateToComments}
                >
                    <BiComment />
                    <div className="text-base">{post.commentCount}</div>
                </div>
                <div 
                    className="cursor-pointer ml-2 flex flex-col items-center hover:text-red-400"
                    onClick={share}
                >
                    <BiShareAlt />
                    <div className="text-base">{shareCount}</div>
                </div>
                <div 
                    className="cursor-pointer ml-auto mr-6 flex flex-col items-center hover:text-red-400"
                    onClick={report}
                >
                    <MdOutlineReport />
                    <div className="text-base">{reportCount}</div>
                </div>
            
            </div>
                {shared === true ? <div className="ml-4 mb-3 px-4 py-2 border border-zinc-700 rounded-md shadow-lg shadow-zinc-700/10 bg-zinc-950 w-fit">
                    Link: localhost:3000/posts/{post.id}
                </div> : null}
        </div>
    );
};

export default Post;