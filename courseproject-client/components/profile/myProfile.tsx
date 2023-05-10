'use client';

import React, { useEffect, useState } from "react";
import Image from "next/image";
import Profile from "@/types/profile";
import Button from "../button/button";
import Dropdown from "../dropdown/dropdown";
import ButtonBlack from "../button/buttonBlack";
import colors from "../colors";
import avatars from "../avatars";
import { useRouter } from "next/navigation";

interface IProfileProps {
    profile: Profile,
    setProfile: React.Dispatch<React.SetStateAction<Profile>>,
    setError: React.Dispatch<React.SetStateAction<string>>
}

const Profile: React.FC<IProfileProps> = ({ profile, setProfile, setError }) => {
    const [username, setUsername] = useState<string>(profile.username);
    const [avatar, setAvatar] = useState<string>(profile.avatar);
    const [color, setColor] = useState<string>(profile.profileColor);

    const router = useRouter();

    useEffect(() => {
        setUsername(profile.username);
        setColor(profile.profileColor);
        setAvatar(profile.avatar);
    }, [profile]);

    const navigateToAdmin = () => {
        router.push("/admin");
    };

    const updateProfile = async () => {
        const response = await fetch("/blurb-api/users/me/update", {
            method: "PUT",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem('token')}`
            },
            body: JSON.stringify({username, avatar, profileColor: color}),
            cache: 'no-store'
        });

        if (!response.ok) {
            setError(await response.text());
            return;
        }

        const profile = await response.json();
        setProfile(profile);
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
                    <div className="flex justify-center w-full">
                    <input
                        className="outline-none bg-zinc-900 border text-left pl-6 border-zinc-700 shadow-md shadow-zinc-700/10 rounded-lg text-xl font-bold max-w-xs mx-3 mt-4 w-full h-12"
                        type="text"
                        placeholder="Username"
                        value={username}
                        onChange={ e => setUsername(e.target.value) }
                    />
                    </div>
                </div>
                <div className="w-72">
                    <Dropdown 
                        zIndex={true}
                        options={Object.keys(avatars)}
                        selectedOption={avatar}
                        setSelectedOption={setAvatar}
                    />
                </div>
                <div className="w-72">
                    <Dropdown 
                        zIndex={false}
                        options={Object.keys(colors)}
                        selectedOption={color}
                        setSelectedOption={setColor}
                    />
                </div>
                <div className="w-72">
                    {username === "" ? <ButtonBlack text="Save changes" />
                    : <Button 
                        text="Save changes"
                        handleClick={updateProfile}
                    />}
                </div> 
                <div className="w-72 mb-4">
                    {profile.role === "ADMIN" || profile.role === "MODERATOR" ? <Button 
                        text="Enter admin panel"
                        handleClick={navigateToAdmin}
                    /> : null}
                </div>
            </div>
        </div>
    );
};

export default Profile;