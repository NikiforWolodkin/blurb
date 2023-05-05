'use client';

import React, { useState } from "react";
import Button from "@/components/button/button";
import TextBox from "@/components/textBox/textBox";
import Link from "next/link";
import { useRouter } from "next/navigation";
import Logo from "@/components/logo/logo";
import ErrorTextBox from "@/components/errorTextBox/errorTextBox";
import Padder from "@/components/padder/padder";

export default function Login () {
    const router = useRouter();

    const [error, setError] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");

    const logIn = async (e:React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        e.preventDefault();

        if (email === "") {
            setError("Please enter an email address");
            return;
        }
        if (password === "") {
            setError("Please enter a password");
            return;
        }

        try {
            const response = await fetch("/blurb-api/auth/login", {
                method: 'POST',
                headers: {'Content-Type': 'application/json'}, 
                body: JSON.stringify({email, password}),
                cache: 'no-store'
            });
    
            if (!response.ok) {
                const result = await response.text();
                setError(result);
                throw new Error(response.status.toString());
            }

            const result = await response.text();
            setError("");
            localStorage.setItem("token", result);

            router.push("/feed");
        }
        catch (e) {
            console.log(e);
        }
    };

    return (
        <form className="flex flex-col items-center justify-center w-screen h-screen">
            <Logo />
            <ErrorTextBox text={error} />
            <TextBox 
                type="text"
                placeholder="E-Mail"
                value={email}
                handleChange={ e => setEmail(e.target.value) }
            />
            <TextBox 
                type="password"
                placeholder="Password"
                value={password}
                handleChange={ e => setPassword(e.target.value) }
            />
            <Button 
                text="Log in"
                handleClick={logIn}
            />
            <Padder />
            <div>
                Don't have an account?&nbsp;
                <Link 
                    className="underline text-blue-600"
                    href="/register"
                >
                    Sign up
                </Link>
            </div>
        </form>
    );
};