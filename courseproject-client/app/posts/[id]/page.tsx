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
import Comment from "@/types/comment";
import Padder from "@/components/padder/padder";

export default function PostPage({ params }) {
    const [error, setError] = useState<string>("");
    const [post, setPost] = useState<Post>();
    const [postLoaded, setPostLoaded] = useState<boolean>();
    const [comments, setComments] = useState<Comment[]>([]);

    const incrementCommentCount = () => {
      post.commentCount++;
    }

    useEffect(() => {
        const getPost = async () => {
          const response = await fetch(`/blurb-api/posts/${params.id}`, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem('token')}`
            },
            cache: 'no-store'
          });
          
          if (!response.ok) {
            setError(await response.text());
            return;
          }
          
          const post = await response.json();
          
          setPost(post);
        };

        const getComments = async () => {
          const response = await fetch(`/blurb-api/posts/${params.id}/comments`, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem('token')}`
            },
            cache: 'no-store'
          });
          
          if (!response.ok) {
            setError(await response.text());
            return;
          }
          
          const comments = await response.json();
          
          setComments(comments);
        };
        
        getPost();
        getComments();
    }, []);

    useEffect(() => {
        if (post !== undefined) {
            setPostLoaded(true);            
        }
    }, [post]);

    return (
        <>
            <Header />
            {postLoaded === true && error === ""
            ? <>
              <Feed posts={[post]} />
              <CreateCommentBox
                incrementCommentCount={incrementCommentCount}
                setError={setError}
                setComments={setComments}
                comments={comments}
                id={params.id}
              />
              <FeedComments
                comments={comments}
              />
            </>
            : (error === "" ? <Loading /> : <ErrorTextBox text={error} />)
            }
            <Padder />
        </>
    );
}