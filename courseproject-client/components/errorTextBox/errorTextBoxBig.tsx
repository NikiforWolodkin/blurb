import { TiDelete } from 'react-icons/ti';

interface IErrorTextBoxProps {
    text: string
}

const ErrorTextBox: React.FC<IErrorTextBoxProps> = ({ text }) => {
    if (text === "") {
        return (<></>);
    }

    return (
        <div className="flex justify-center">
            <div className="flex items-center outline-none bg-red-600 shadow-lg shadow-red-600/10 rounded-lg max-w-4xl px-4 mt-4 mx-4 w-full h-12">
                <div className="text-2xl mr-1">
                    <TiDelete />
                </div>
                {text}
            </div>
        </div>
    );
}

export default ErrorTextBox;