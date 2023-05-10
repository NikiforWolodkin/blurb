"use client";

import FeedHeader from "@/components/feedHeader/feedHeader";
import Header from "@/components/header/header";
import Statistics from "@/components/statistics/statistics";
import { useEffect, useState } from "react";
import Stats from "../../types/stats";
import Loading from "@/components/loading/loading";
import ErrorTextBox from "@/components/errorTextBox/errorTextBoxBig";
import Post from "@/types/post";
import Feed from "@/components/feed/feed";
import Padder from "@/components/padder/padder";
import FeedReported from "@/components/feed/feedReported";

export default function Admin() {
    const [error, setError] = useState<string>("");
    const [stats, setStats] = useState<Stats>();
    const [statsLoaded, setStatsLoaded] = useState<boolean>(false);
    const [reportedPosts, setReportedPosts] = useState<Post[]>([]);

    useEffect(() => {
        const getStats = async () => {
            const response = await fetch(`/blurb-api/stats`, {
              headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
              },
              cache: 'no-store'
            });
            
            if (!response.ok) {
              setError(await response.text());
              return;
            }
            
            const stats = await response.json();
            
            setStats(stats);
            setStatsLoaded(true);
        };

        const getPosts = async () => {
            const response = await fetch(`/blurb-api/posts/reported`, {
              headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
              },
              cache: 'no-store'
            });
            
            if (!response.ok) {
              setError(await response.text());
              return;
            }
            
            const posts = await response.json();
            
            setReportedPosts(posts);
        };

        getStats();
        getPosts();
    }, []);

    return (
        <>
            <Header />
            {statsLoaded === true && error === "" ? <>
            <FeedHeader text="Statistics" />
            <Statistics stats={stats}/> 
            {reportedPosts.length === 0 ? <FeedHeader text="There are no reported posts yet" /> : <FeedHeader text="Reported posts" />}
            <FeedReported 
              posts={reportedPosts} 
              setError={setError}
              setPosts={setReportedPosts}
            />
            <Padder />
            </>
            : error === "" ? <Loading /> : <ErrorTextBox text={error} />}
        </>
    );
};