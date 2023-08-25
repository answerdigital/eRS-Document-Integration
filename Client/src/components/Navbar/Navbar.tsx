import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';
import { loginRequest } from 'authConfig';
import NavbarLink from 'components/Navbar/NavbarLink/NavbarLink';
import { msalInstance } from 'index';
import React, { useEffect, useState } from 'react';
import { RouterPaths } from 'utility/RouterPaths';

interface NavbarProps {
    children: React.ReactNode;
}

const Navbar : React.FC<NavbarProps> = ({children}) => {
    const [collapsed, setCollapsed] = useState<boolean>(false);
    const { instance } = useMsal();
    const account = msalInstance.getAllAccounts()[0];

    useEffect(() => {
        console.log(account);
    }, []);

    const login = () => {
        instance.loginRedirect(loginRequest).catch(e => {
            console.error(e);
        });
    };

    const logout = () => {
        instance.logoutRedirect()
        .catch(e => {
            console.error(e);
        });
    };

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
                            <NavbarLink title={'Worklist'} link={RouterPaths.WorklistPath} />
                            <NavbarLink title={'Audit Log'} link={RouterPaths.AuditsPath} />
                        </ul>
                        <div className='d-flex'>
                            <UnauthenticatedTemplate>
                                <button onClick={() => login()} className='btn btn-outline-success'>Sign in</button>
                            </UnauthenticatedTemplate>
                            <AuthenticatedTemplate>
                                <div className='d-flex m-2'>{account?.name}</div>
                                <button onClick={() => logout()} className='btn btn-outline-success'>Sign out</button>
                            </AuthenticatedTemplate>
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