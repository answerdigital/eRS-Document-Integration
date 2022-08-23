import NavbarLink from 'components/Navbar/NavbarLink/NavbarLink';
import React, { useState } from 'react';
import { RouterPaths } from 'utility/RouterPaths';

interface NavbarProps {
    children: React.ReactNode;
}

const Navbar : React.FC<NavbarProps> = ({children}) => {
    const [collapsed, setCollapsed] = useState<boolean>(false);

    return (
        <>
            <nav className='navbar navbar-expand-lg navbar-light bg-light'>
                <div className='container-fluid'>
                    <a className='navbar-brand'>Rotherham eRS</a>
                    <button className='navbar-toggler' type='button' aria-label='Toggle navigation'
                        onClick={() => setCollapsed(!collapsed)}>
                        <span className='navbar-toggler-icon'></span>
                    </button>
                    <div className={`collapse navbar-collapse ${collapsed && 'show'}`}>
                        <ul className='navbar-nav me-auto mb-2 mb-lg-0'>
                            <NavbarLink title={'Home'} link={RouterPaths.HomePath} />
                            <NavbarLink title={'Worklist'} link={RouterPaths.WorklistPath} />
                            <NavbarLink title={'Users'} link={RouterPaths.UsersPath} />
                            <NavbarLink title={'Audit Log'} link={RouterPaths.AuditsPath} />
                        </ul>
                        <div className='d-flex'>
                            <button className='btn btn-outline-success'>Sign out</button>    
                        </div>
                    </div>
                </div>
            </nav>
            <div className='container-fluid'>
                {children}
            </div>
        </>
    );
}

export default Navbar;