import React from "react";

interface IFeedHeaderProps {
    text: string,
}

const FeedHeader: React.FC<IFeedHeaderProps> = ({text}) => {
    return (
        <div className="flex justify-center px-4 mt-4 w-full">
            <div className="text-4xl font-bold max-w-4xl px-3 w-full">
                {text}
            </div>
        </div>
    );
};

export default FeedHeader;