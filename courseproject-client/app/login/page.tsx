import React from "react";
import Button from "@/components/button/button";
import TextBox from "@/components/textBox/textBox";
import Link from "next/link";

export default function Login () {
    return (
        <div className="flex flex-col items-center justify-center w-screen h-screen">
            <TextBox 
                type="text"
                placeholder="Username"
            />
            <TextBox 
                type="password"
                placeholder="Password"s
            />
            <Link href="/" className="w-full">
                <Button text="Log in"/>
            </Link>
            <div>
                Don't have an account?&nbsp;
                <Link 
                    className="underline text-blue-600"
                    href="/register"
                >
                    Sign up
                </Link>
            </div>
        </div>
    );
};