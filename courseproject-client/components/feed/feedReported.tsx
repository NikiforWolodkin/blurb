import React, { SetStateAction } from "react";
import PostComponent from "../post/post";
import Post from "@/types/post";
import PostActions from "../post/postActions";

interface IFeedProps {
  setError: React.Dispatch<SetStateAction<string>>,
  setPosts: React.Dispatch<SetStateAction<Post[]>>,
  posts: Post[]
}

const FeedReported: React.FC<IFeedProps> = ({ setError, setPosts, posts }) => { 
    const dismissPost = async (id: number) => {
      const response = await fetch(`/blurb-api/posts/${id}/reports`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
        cache: 'no-store'
      });
      
      if (!response.ok) {
        setError(await response.text());
        return;
      }
      
      setPosts(posts.filter(item => item.id !== id));
    };

    const deletePost = async (id: number) => {
      const response = await fetch(`/blurb-api/posts/${id}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
        cache: 'no-store'
      });
      
      if (!response.ok) {
        setError(await response.text());
        return;
      }
      
      setPosts(posts.filter(item => item.id !== id));
    };

    const deletePostAndBanUser = async (id: number, userId: number) => {
      deletePost(id);

      const response = await fetch(`/blurb-api/users/${userId}/status`, {
        method: 'PUT',
        headers: {
          'Content-type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
        body: JSON.stringify({ status: "BANNED" }),
        cache: 'no-store'
      });
      
      if (!response.ok) {
        setError(await response.text());
        return;
      }
    };
  
    return (
      <div className="flex justify-center w-full">
        <div className="flex flex-col justify-center mx-4 max-w-4xl w-full">
          {posts.map(item => {
            return <div key={item.id}>
                <PostComponent
                    post={item}
                />
                <PostActions 
                  dismissHandler={ () => dismissPost(item.id) }
                  deleteHandler={ () => deletePost(item.id) }
                  deleteAndBanHandler={ () => deletePostAndBanUser(item.id, item.authorId) }
                />
                </div>;
          })}
        </div>
      </div>
    );
};

export default FeedReported;