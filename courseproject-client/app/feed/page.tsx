'use client';

import Header from "@/components/header/header";
import Feed from "@/components/feed/feed";
import CreatePostBox from "@/components/createPostBox/createPostBox";
import Button from "@/components/button/button";
import FeedHeader from "@/components/feedHeader/feedHeader";
import Post from "@/types/post";
import { useEffect, useState } from "react";
import ErrorTextBox from "@/components/errorTextBox/errorTextBoxBig";
import Padder from "@/components/padder/padder";
import Loading from "@/components/loading/loading";
import Search from "@/components/search/searchPost";

export default function FeedPage() {
  const [error, setError] = useState<string>("");
  const [trendingPosts, setTrendingPosts] = useState<Post[]>([]);
  const [subscriptionPosts, setSubscriptionPosts] = useState<Post[]>([])
  const [searchPosts, setSearchPosts] = useState<Post[]>([]);
  const [postsLoaded, setPostsLoaded] = useState<boolean>(false);
  const [trendingPage, setTrendingPage] = useState<number>(1);
  const [subscriptionsPage, setSubscriptionsPage] = useState<number>(1);

  const getMoreTrendingPosts = async () => {
    const response = await fetch(`/blurb-api/posts/trending?page=${trendingPage}`, {
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
    
    setTrendingPosts([...trendingPosts, ...posts]);
    setTrendingPage(trendingPage + 1);
  };

  const getMoreSubscriptionPosts = async () => {
    const response = await fetch(`/blurb-api/users/me/subscriptions/posts?page=${trendingPage}`, {
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
    
    setSubscriptionPosts([...subscriptionPosts, ...posts]);
    setSubscriptionsPage(subscriptionsPage + 1);
  };

  useEffect(() => {
    const getSubscriptionPosts = async () => {
      const response = await fetch("/blurb-api/users/me/subscriptions/posts", {
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
      
      setSubscriptionPosts(posts);
      setPostsLoaded(true);
    };

    const getTrendingPosts = async () => {
      const response = await fetch("/blurb-api/posts/trending", {
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
      
      setTrendingPosts(posts);
    };
    
    getSubscriptionPosts();
    getTrendingPosts();
  }, []);

  return (
    <>
      <Header />
      {postsLoaded === true && error === "" ? <>
      <CreatePostBox setError={setError} />
      <Search 
        setPosts={setSearchPosts}
        setError={setError}
        setLoaded={setPostsLoaded}
        placeholder="Find post..."
      />
      <Feed posts={searchPosts} />
      {subscriptionPosts.length !== 0 ? <><FeedHeader text="For you" />
      <Feed posts={subscriptionPosts} />
      <Button 
        text="Show more"
        handleClick={getMoreSubscriptionPosts}
      /></> : null}
      <FeedHeader text="Trending" />
      <Feed posts={trendingPosts} />
      <Button 
        text="Show more"
        handleClick={getMoreTrendingPosts}
      /></> : error === "" ? <Loading /> : <ErrorTextBox text={error} />}
      <Padder />
    </>
  )
}

