import Link from "next/link";
import React from "react";
import { FaUserAlt } from "react-icons/fa";
import { MdSubscriptions } from "react-icons/md";

const Header: React.FC = () => {
    return (
        <>
            <div className="fixed z-10 bg-zinc-900 shadow-lg shadow-zinc-700/10 border-b border-zinc-700 flex justify-center items-center w-full h-16">
                <div className="max-w-4xl flex items-center px-3 mx-4 w-full">
                    <div className="font-black cursor-pointer text-4xl text-transparent bg-clip-text bg-gradient-to-r from-purple-500 to-red-500">
                        <Link href="/feed">
                            Blurb
                        </Link>
                    </div>
                    <div className="hover:text-red-400 text-3xl cursor-pointer ml-auto mr-4">
                        <Link href="/subscriptions">
                            <MdSubscriptions />
                        </Link>
                    </div>
                    <div className="hover:text-red-400 text-2xl cursor-pointer">
                        <Link href="/account">
                            <FaUserAlt />
                        </Link>
                    </div>
                </div>
            </div>

            <div className="h-16"></div>
        </>
    );
};


export default Header;