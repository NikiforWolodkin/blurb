'use client';

import Profile from "@/types/profile";
import React, { useState, useRef } from "react";
import { IoSearchSharp } from "react-icons/io5";

interface ISearchProps {
    placeholder: string,
    setError: React.Dispatch<React.SetStateAction<string>>,
    setProfiles: React.Dispatch<React.SetStateAction<Profile[]>>,
    setLoaded: React.Dispatch<React.SetStateAction<boolean>>
}

const Search: React.FC<ISearchProps> = ({ placeholder, setError, setProfiles, setLoaded }) => {
    const [text, setText] = useState<string>("");

    const getProfiles = async () => {
        if (text === "") {
            return;
        }

        const response = await fetch(`/blurb-api/users?search=${text}`, {
            headers: {
                "Authorization": `Bearer ${localStorage.getItem('token')}`
            },
            cache: 'no-store'
        });

        if (!response.ok) {
            setError(await response.text());
            return;
        }

        const profiles = await response.json();
        setProfiles(profiles);
        setLoaded(true);
    };

    return (
        <div className="flex justify-center w-full">
            <div className="bg-zinc-900 border border-zinc-700 flex flex-row justify-center items-end rounded-lg p-3 mt-4 mx-4 max-w-4xl w-full">
                <input
                    type="text" 
                    className="text-xl bg-transparent outline-none resize-none overflow-hidden border-b border-zinc-700 pb-0.5 w-full"
                    placeholder={placeholder}
                    onChange={ e => setText(e.target.value) }
                    value={text}
                />
                <div 
                    className="cursor-pointer text-2xl hover:text-red-400 ml-2 mb-1"
                    onClick={getProfiles}   
                >
                    <IoSearchSharp />
                </div>
                
            </div>
        </div>
    );
};

export default Search;