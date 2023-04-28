import { NextResponse } from 'next/server';
import Post from '@/types/post';

const Posts: Post[] = [
  {
    author: "MrKotek",
    text: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. #lorem",
    date: new Date(Date.now()).getTime(),
    likes: 123,
    comments: 32,
    shares: 57,
  }, {
    author: "Fredguitarist",
    text: "Aliquam tempus est vel sapien consectetur, sed sagittis nibh molestie. Suspendisse molestie arcu eu nisi lacinia commodo at quis nisi.",
    date: new Date(Date.now()).getTime(),
    likes: 23,
    comments: 35,
    shares: 90,
  }, {
    author: "Jason",
    text: "Aenean quis urna vitae quam dapibus ullamcorper ut vel turpis.",
    date: new Date(Date.now()).getTime(),
    likes: 236,
    comments: 56,
    shares: 124,
  },
]

export async function GET(request: Request) {
  return NextResponse.json(Posts);
}
