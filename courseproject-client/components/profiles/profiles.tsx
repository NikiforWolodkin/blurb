import Profile from "@/types/profile";
import ProfileComponent from "../profile/profile";

interface IProfileProps {
    profiles: Profile[] 
}

const Profiles: React.FC<IProfileProps> = ({ profiles }) => {
    return (
        <>
            {profiles.map(item => {
                return (<ProfileComponent
                    key={item.id}
                    profile={item}
                />);
            })}
        </>
    );
};

export default Profiles;