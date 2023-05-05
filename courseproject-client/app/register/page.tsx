'use client';

import React, { useState } from "react";
import Button from "@/components/button/button";
import TextBox from "@/components/textBox/textBox";
import Link from "next/link";
import { useRouter } from "next/navigation";
import Logo from "@/components/logo/logo";
import ErrorTextBox from "@/components/errorTextBox/errorTextBox";
import Padder from "@/components/padder/padder";

export default function Register () {
    const router = useRouter();

    const [error, setError] = useState<string>("");
    const [username, setUsername] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [repeatPassword, setRepeatPassword] = useState<string>("");
    const [email, setEmail] = useState<string>("");

    const singUp = async (e:React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        e.preventDefault();

        if (username === "") {
            setError("Please enter an username");
            return;
        }
        if (email === "") {
            setError("Please enter an email address");
            return;
        }
        if (password === "") {
            setError("Please enter a password");
            return;
        }
        if (password !== repeatPassword) {
            setError("Please check your password");
            return;
        }

        try {
            const response = await fetch("/blurb-api/auth/signup", {
                method: 'POST',
                headers: {'Content-Type': 'application/json'}, 
                body: JSON.stringify({email, password, username}),
                cache: 'no-store'
            });
    
            if (!response.ok) {
                const result = await response.text();
                setError(result);
                throw new Error(response.status.toString());
            }

            const result = await response.text();
            setError("");

            router.push("/login");
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
                placeholder="Username"
                value={username}
                handleChange={ e => setUsername(e.target.value) }
            />
            <TextBox 
                type="text"
                placeholder="E-mail"
                value={email}
                handleChange={ e => setEmail(e.target.value) }
            />  
            <TextBox 
                type="password"
                placeholder="Password"
                value={password}
                handleChange={ e => setPassword(e.target.value) }
            />
            <TextBox 
                type="password"
                placeholder="Repeat password"
                value={repeatPassword}
                handleChange={ e => setRepeatPassword(e.target.value) }
            />
            <Button 
                text="Sign up"
                handleClick={singUp}
            />
            <Padder />
            <div>
                Already have an account?&nbsp;
                <Link 
                    className="underline text-blue-600"
                    href="/login"
                >
                    Log in
                </Link>
            </div>
        </form>
    );
};