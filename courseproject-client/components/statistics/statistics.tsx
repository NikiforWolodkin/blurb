'use client';

import React, { useState } from "react";
import Stats from "@/types/stats";

interface IStatisticsProps {
    stats: Stats
}

const Statistics: React.FC<IStatisticsProps> = ({ stats }) => {
    return (
        <div className="w-full flex justify-center">
            <div
                className="max-w-4xl mx-4 flex flex-nowrap flex-row bg-zinc-900 shadow-lg shadow-zinc-700/10 border border-zinc-700 rounded-lg mt-4 w-full"
            >
                <div className="flex basis-1/2 ml-3 my-3 text-xl">
                    <div className="">
                        <p>Users:</p>
                        <p>Posts:</p>
                        <p>Comments:</p>
                        <p>Likes:</p>
                        <p>Shares:</p>
                        <p>Reports:</p>
                        <p>Blocked users:</p>
                    </div>
                </div>
                <div className="flex basis-1/2 my-3 text-xl">
                    <div className="">
                        <p>{stats.users}</p>
                        <p>{stats.posts}</p>
                        <p>{stats.comments}</p>
                        <p>{stats.likes}</p>
                        <p>{stats.shares}</p>
                        <p>{stats.reports}</p>
                        <p>{stats.blockedUsers}</p>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Statistics;