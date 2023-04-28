import React from "react";
import Header from "@/components/header/header";
import Profile from "@/components/profile/profile";
import FeedHeader from "@/components/feedHeader/feedHeader";
import Feed from "@/components/feed/feed";
import CreatePostBox from "@/components/createPostBox/createPostBox";
import Padder from "@/components/padder/padder";
import Post from "@/types/post";

const getPosts = async (): Promise<Post[]> => {
    const res = await fetch("http:/localhost:3000/api/trending-posts", { cache: 'no-store' });
    if (!res.ok) {
      // This will activate the closest `error.js` Error Boundary
      throw new Error("Failed to fetch data");
    }
    return res.json();
};

export default async function Account() {
    const posts:Post[] = await getPosts();

    return (
        <>
            <Header />
            <Profile />
            <CreatePostBox />
            <FeedHeader text="Your posts" />
            <Feed posts={posts}/>
            <Padder />
        </>
    );
};