import { ReactNode, useState } from "react";

interface IconButtonProps {
    icon: ReactNode;
    iconHover: ReactNode;
    onClick: () => void;
}

const IconButton : React.FC<IconButtonProps> = ({icon, iconHover, onClick}) => {
    const [hovered, setHovered] = useState<boolean>(false);

    return (
        <div
        className='cursor-pointer'
        onMouseEnter={() => setHovered(true)}
        onMouseLeave={() => setHovered(false)}
        onClick={onClick}>
            {hovered ? iconHover : icon}
        </div>
    )
};

export default IconButton;