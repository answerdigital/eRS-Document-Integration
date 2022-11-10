import { IUser } from "common/interfaces/account.interface";
import { useSession } from "hooks/useSession";
import { createContext, useContext, useEffect, useState } from "react";
import { getUserDetails } from "services/account-service";

interface ISessionContext {
    userDetails?: IUser;
    setUserDetails: (user: IUser | undefined) => void;
}

const SessionContext = createContext<ISessionContext>({
    userDetails: undefined,
    setUserDetails: (user) => undefined
});

interface ISessionContextProviderProps {
    children: React.ReactNode;
}

const SessionContextProvider: React.FC<ISessionContextProviderProps> = ({children}) => {
    const [userDetails, setUserDetails] = useState<IUser | undefined>();
    const { getJWT } = useSession();

    useEffect(() => {
        const token = getJWT();
        if (!userDetails) {
            if (token) {
                getUserDetails().then((userResponse: IUser | undefined) => {
                    setUserDetails(userResponse);
                })
            }
        }else{
            if (!token) {
                setUserDetails(undefined);
            }
        }
    }, []);

    return (
        <SessionContext.Provider value={{
            userDetails: userDetails,
            setUserDetails: setUserDetails
        }}>
            {children}
        </SessionContext.Provider>
    );
}

export const useUserDetails = () => {
    const context = useContext(SessionContext);
    if (context === undefined) {
        throw new Error('useSession must be used within a SessionContextProvider');
    }
    return context;
}

export default SessionContextProvider;