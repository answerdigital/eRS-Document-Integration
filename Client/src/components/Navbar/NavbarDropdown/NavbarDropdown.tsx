import React, { useState } from 'react';

interface NavbarDropdownProps {
    children: React.ReactNode;
}

const NavbarDropdown : React.FC<NavbarDropdownProps> = ({children}) => {
    const [active, setActive] = useState<boolean>(false);

    return (
        <>
            <li className='nav-item dropdown'>
                <a className='nav-link dropdown-toggle' role='button' onClick={() => setActive(!active)}>
                    Log in
                </a>
                <ul className={`dropdown-menu dropdown-menu-end ${active && 'show'}`}aria-labelledby='navbarDropdown'>
                    {children}
                </ul>
            </li>
        </>
    );
}

export default NavbarDropdown;