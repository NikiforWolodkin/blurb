'use client';

import Header from "@/components/header/header";
import { useEffect, useState } from "react";
import Post from "../../../types/post";
import PostComponent from "@/components/post/post";
import Loading from "@/components/loading/loading";
import ErrorTextBox from "@/components/errorTextBox/errorTextBoxBig";
import Feed from "@/components/feed/feed";
import CreateCommentBox from "@/components/createPostBox/createCommentBox";
import FeedComments from "@/components/feed/feedComments";
import Profile from "@/types/profile";
import ProfileComponent from "@/components/profile/profile";

export default function UserPage({ params }) {
    const [error, setError] = useState<string>("");
    const [user, setUser] = useState<Profile>();
    const [userLoaded, setUserLoaded] = useState<boolean>();

    useEffect(() => {
        const getUser = async () => {
          const response = await fetch(`/blurb-api/users/${params.id}`, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem('token')}`
            },
            cache: 'no-store'
          });
          
          if (!response.ok) {
            setError(await response.text());
            return;
          }
          
          const user = await response.json();
          
          setUser(user);
        };
        
        getUser();
    }, []);

    useEffect(() => {
        if (user !== undefined) {
            setUserLoaded(true);            
        }
    }, [user]);

    return (
        <>
            <Header />
            {userLoaded === true && error === ""
            ? <>
              <ProfileComponent profile={user} />
            </>
            : (error === "" ? <Loading /> : <ErrorTextBox text={error} />)
            }
        </>
    );
}