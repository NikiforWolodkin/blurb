'use client';

import Header from "@/components/header/header";
import { useEffect, useState } from "react";
import Profiles from "@/components/profiles/profiles";
import Profile from "@/types/profile";
import Search from "@/components/search/search";
import ErrorTextBox from "@/components/errorTextBox/errorTextBoxBig";
import Loading from "@/components/loading/loading";
import FeedHeader from "@/components/feedHeader/feedHeader";
import Padder from "@/components/padder/padder";

export default function FeedPage() {
  const [error, setError] = useState<string>("");
  const [profiles, setProfiles] = useState<Profile[]>([]);
  const [searchProfiles, setSearchProfiles] = useState<Profile[]>([]);
  const [profilesLoaded, setProfilesLoaded] = useState<boolean>(false);
  const [searchProfilesLoaded, setSearchProfilesLoaded] = useState<boolean>(false);

  useEffect(() => {
    const getSubscriptions = async () => {
      const response = await fetch("/blurb-api/users/me/subscriptions", {
        headers: {
          'Content-type': 'application/json', 
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
        cache: 'no-store'
      });

      if (!response.ok) {
        setError("Failed to fetch data");
        return
      }

      const profiles = await response.json();
      setProfiles(profiles);
      setProfilesLoaded(true);
    };
    
    getSubscriptions();
  }, []);

  return (
    <>
      <Header />
      <ErrorTextBox text={error} />
      <Search
        placeholder="Find user..." 
        setError={setError}
        setProfiles={setSearchProfiles}
        setLoaded={setSearchProfilesLoaded}
      />
      {searchProfilesLoaded === true ?
          searchProfiles.length !== 0 ? <Profiles profiles={searchProfiles} /> : <FeedHeader text="No users found" />
         : null
      }
      {profilesLoaded === true
        ? <>
          {profiles.length === 0 ? <FeedHeader text="You don't have any subscriptions yet"/> : <FeedHeader text="Your subscriptions" />}
          <Profiles profiles={profiles}/>
        </> 
        : <Loading />
      }
      <Padder />
    </>
  );
}

