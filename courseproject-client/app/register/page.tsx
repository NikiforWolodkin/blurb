import React from "react";
import Button from "@/components/button/button";
import TextBox from "@/components/textBox/textBox";
import Link from "next/link";

export default function Register () {
    return (
        <div className="flex flex-col items-center justify-center w-screen h-screen">
            <TextBox 
                type="password"
                placeholder="Username"
            />
            <TextBox 
                type="text"
                placeholder="E-mail"
            />  
            <TextBox 
                type="password"
                placeholder="Password"
            />
            <TextBox 
                type="password"
                placeholder="Repeat password"
            />
            <Link href="/" className="w-full">
                <Button text="Sign up"/>
            </Link>
            <div>
                Already have an account?&nbsp;
                <Link 
                    className="underline text-blue-600"
                    href="/login"
                >
                    Log in
                </Link>
            </div>
        </div>
    );
};