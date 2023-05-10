import { MouseEventHandler } from "react";

interface IPostActionsProps {
    dismissHandler: MouseEventHandler<HTMLButtonElement>,
    deleteHandler: MouseEventHandler<HTMLButtonElement>,
    deleteAndBanHandler: MouseEventHandler<HTMLButtonElement>
};

const PostActions: React.FC<IPostActionsProps> = ({ dismissHandler, deleteHandler, deleteAndBanHandler }) => {
    return (
        <div className="bg-zinc-900 shadow-lg shadow-zinc-700/10 border border-zinc-700 rounded-lg mt-4 w-full">
    <div className="flex m-1">
        <button 
            className="bg-red-700 text-white shadow-lg shadow-red-700/5 rounded-lg basis-1/3 text-lg font-bold py-2 px-4 m-2"
            onClick={deleteAndBanHandler}
        >
            Delete and ban
        </button>
        <button 
            className="bg-red-700 text-white shadow-lg shadow-red-700/5 rounded-lg basis-1/3 text-lg font-bold py-2 px-4 m-2"
            onClick={deleteHandler}
        >
            Delete
        </button>
        <button 
            className="bg-zinc-100 text-black shadow-lg shadow-zinc-100/5 rounded-lg basis-1/3 text-lg font-bold py-2 px-4 m-2"
            onClick={dismissHandler}
        >
            Dismiss
        </button>
    </div>
</div>
    );
};

export default PostActions;