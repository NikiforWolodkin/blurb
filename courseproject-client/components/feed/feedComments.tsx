import React from "react";
import PostComponent from "../post/post";
import Comment from "@/types/comment";
import CommentComponent from "../comment/comment";

interface IFeedCommentsProps {
  comments: Comment[],
}

const FeedComments: React.FC<IFeedCommentsProps> = ({ comments }) => {    
    return (
      <div className="flex justify-center w-full">
        <div className="flex flex-col justify-center mx-4 max-w-4xl w-full">
          {comments.map(item => {
            return <CommentComponent
                    key={item.id} 
                    comment={item}
                   />;
          })}
        </div>
      </div>
    );
};

export default FeedComments;