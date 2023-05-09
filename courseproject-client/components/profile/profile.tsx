'use client';

import React, { useState } from "react";
import Image from "next/image";
import Profile from "@/types/profile";
import Button from "../button/button";
import ButtonBlack from "../button/buttonBlack";
import { unsubscribe } from "diagnostics_channel";
import colors from "../colors";

interface IProfileProps {
    profile: Profile 
}

const Profile: React.FC<IProfileProps> = ({ profile }) => {
    const [isSubscribed, setIsSubscribed] = useState<boolean>(profile.isSubscribed);

    const subscribe = async () => {
        const response = await fetch(`/blurb-api/users/me/subscriptions`, {
            method: "POST",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem('token')}`
            },
            body: JSON.stringify({ publisherId: profile.id }),
            cache: 'no-store'
        });

        setIsSubscribed(true);
    };

    const unsubscribe = async () => {
        const response = await fetch(`/blurb-api/users/me/subscriptions`, {
            method: "DELETE",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem('token')}`
            },
            body: JSON.stringify({ publisherId: profile.id }),
            cache: 'no-store'
        });

        setIsSubscribed(false);
    };

    return (
        <div className="flex justify-center w-full">
            <div className="border bg-zinc-900 border-zinc-700 rounded-lg shadow-lg shadow-zinc-700/10 max-w-4xl mt-4 mx-4 w-full">
                <div className={(colors[profile.profileColor] === undefined ? "bg-red-700 " : colors[profile.profileColor])
                + "border-b border-zinc-700 rounded-t-lg w-full h-20"}/>
                <div className="relative bottom-4 left-3 border border-zinc-700 rounded-full flex justify-center items-center w-16 h-8">
                {profile.avatar === "Cat" ? <Image
                        className="rounded-full border border-zinc-700"
                        alt="user-avatar"
                        src="/../public/avatars/cat.jpg"
                        width="100"
                        height="100"
                    /> : <Image
                    className="rounded-full border border-zinc-700"
                    alt="user-avatar"
                    src="/../public/avatars/basic.jpg"
                    width="100"
                    height="100"
                />}
                </div>
                <div className="flex flex-wrap items-end mx-3 mt-2">
                    <div className="text-3xl font-bold mr-3">
                        {profile.username + (profile.status === "BANNED" ? " (Blocked)" : "")}
                    </div>
                    {profile.registrationDate === "0" ? null : 
                    <div className="text-xl text-zinc-500 mb-px">
                        Joined on {new Date(profile.registrationDate).toDateString()}
                    </div>}
                </div>
                <div className="w-72">
                    {isSubscribed === true 
                    ? <ButtonBlack
                            text="Unsubscribe"
                            handleClick={unsubscribe}
                    />
                    : <div className="mb-4"><Button
                        text="Subscribe"
                        handleClick={subscribe}
                    /></div>
                    }
                </div>
            </div>
        </div>
    );
};

export default Profile;