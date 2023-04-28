import Header from "@/components/header/header";
import Feed from "@/components/feed/feed";
import CreatePostBox from "@/components/createPostBox/createPostBox";
import Button from "@/components/button/button";
import FeedHeader from "@/components/feedHeader/feedHeader";
import Post from "@/types/post";

const getPosts = async (): Promise<Post[]> => {
  const res = await fetch("http:/localhost:3000/api/trending-posts", { cache: 'no-store' });
  if (!res.ok) {
    // This will activate the closest `error.js` Error Boundary
    throw new Error("Failed to fetch data");
  }
  return res.json();
};

export default async function Home() {
  const posts: Post[] = await getPosts();

  return (
    <>
      <Header />
      <CreatePostBox />
      <FeedHeader text="For you" />
      <Feed posts={posts} />
      <FeedHeader text="Trending" />
      <Feed posts={posts} />
      <Button text="Show more" />
    </>
  )
}
