import NavbarLink from 'components/Navbar/NavbarLink/NavbarLink';
import { useUserDetails } from 'contexts/SessionContext';
import { useSession } from 'hooks/useSession';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { RouterPaths } from 'utility/RouterPaths';

interface NavbarProps {
    children: React.ReactNode;
}

const Navbar : React.FC<NavbarProps> = ({children}) => {
    const [collapsed, setCollapsed] = useState<boolean>(false);

    const { userDetails, setUserDetails } = useUserDetails();
    const { hasJWT, clearJWT } = useSession();
    const navigate = useNavigate();

    const logout = () => {
        clearJWT();
        setUserDetails(undefined);
        navigate(RouterPaths.LoginPath);
    };

    const loggedIn = hasJWT();

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
                            {loggedIn ?
                            <>
                                <NavbarLink title={'Worklist'} link={RouterPaths.WorklistPath} />
                                <NavbarLink title={'Audit Log'} link={RouterPaths.AuditsPath} />
                                <NavbarLink title={'Manage Users'} link={RouterPaths.UsersPath} />
                            </>
                            : null}
                        </ul>
                        <div className='d-flex'>
                            {loggedIn ?
                            <>
                                <div className='d-flex m-2'>{userDetails?.userFullName}</div>
                                <button onClick={() => logout()} className='btn btn-outline-success'>Sign out</button>
                            </>
                            : null
                            }
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