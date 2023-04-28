import React from "react";
import Image from "next/image";

const Profile: React.FC = () => {
    return (
        <div className="flex justify-center w-full">
            <div className="border bg-zinc-900 border-zinc-700 rounded-lg shadow-lg shadow-zinc-700/10 max-w-4xl mt-4 mx-4 w-full">
                <div className="bg-red-700 border-b border-zinc-700 rounded-t-lg w-full h-20"/>
                <div className="relative bottom-4 left-3 border border-zinc-700 rounded-full flex justify-center items-center w-16 h-8">
                    <Image
                        className="rounded-full"
                        src="/../public/avatars/basic.jpg"
                        width="100"
                        height="100"
                    />
                </div>
                <div className="flex items-end mx-3 mt-2">
                    <div className="text-3xl font-bold">
                        User
                    </div>
                    <div className="text-xl text-zinc-500 mb-px ml-3">
                        Joined on april 1st, 2023
                    </div>
                </div>
                <div className="mt-1 mb-3 mx-3">
                    Lorem ipsum dolor sit amet consectetur adipisicing elit. Necessitatibus voluptatum velit delectus hic inventore consectetur.
                    Dolore ea minus quisquam. Consequatur nostrum facere asperiores, atque ea porro vel error vero odio!
                </div>
            </div>
        </div>
    );
};

export default Profile;