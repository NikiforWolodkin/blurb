'use client';

import React, { useEffect, useState } from "react";
import Header from "@/components/header/header";
import ProfileComponent from "@/components/profile/myProfile";
import FeedHeader from "@/components/feedHeader/feedHeader";
import Feed from "@/components/feed/feed";
import CreatePostBox from "@/components/createPostBox/createPostBox";
import Padder from "@/components/padder/padder";
import Post from "@/types/post";
import Profile from "@/types/profile";
import ErrorTextBox from "@/components/errorTextBox/errorTextBoxBig";
import Loading from "@/components/loading/loading";

export default function Account() {
    const [error, setError] = useState<string>("")
    const [profile, setProfile] = useState<Profile>({
        id: 0, 
        username: "Loading...",
        avatar: "Loading...",
        profileColor: "Loading...",
        registrationDate: "0",
        status: "ACTIVE",
        role: "USER",
        isSubscribed: false,
        canBan: false
    });
    const [posts, setPosts] = useState<Post[]>([]);
    const [profileLoaded, setProfileLoaded] = useState<boolean>(false); 

    useEffect(() => {
        const getProfile = async () => {
            const response = await fetch("/blurb-api/users/me", {
              headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
              },
              cache: 'no-store'
            });
      
            if (!response.ok) {
              setError(await response.text());
              return
            }
      
            const profile = await response.json();
            
            setProfile(profile);
        };

        const getPosts = async () => {
          const response = await fetch("/blurb-api/users/me/posts", {
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
          
          setPosts(posts);
        };

        getProfile();
        getPosts();
    }, []);

    useEffect(() => {
      if (profile.id !== 0) {
        setProfileLoaded(true);
      }
    }, [profile]);

    return (
        <>
            <Header />
            {
            profileLoaded === true && error === "" ? <><ProfileComponent 
              profile={profile}
              setProfile={setProfile}
              setError={setError}
            />
            <CreatePostBox setError={setError}/>
            {posts.length === 0 ? <FeedHeader text="You don't have any posts yet" /> : <FeedHeader text="Your posts" />}
            <Feed posts={posts}/></>
            : (error === "" ? <Loading /> : <ErrorTextBox text={error} />)}
            <Padder />
        </>
    );
};