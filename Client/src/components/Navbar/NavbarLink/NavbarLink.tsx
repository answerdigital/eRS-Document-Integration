import React from 'react';
import { useLocation } from 'react-router-dom';

interface NavbarLinkProps {
    title: string;
    link: string;
}

const NavbarLink : React.FC<NavbarLinkProps> = ({title, link}) => {
    const location = useLocation();

    return (
        <li className='nav-item'>
            <a
                className={`nav-link ${location.pathname === link && 'active'}`}
                aria-current='page'
                href={link}
            >
                {title}
            </a>
        </li>
    );
}

export default NavbarLink;