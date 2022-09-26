import { useSession } from "hooks/useSession";
import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import { RouterPaths } from "utility/RouterPaths";

const AuthenticatedRoutes: React.FC = () => {
    const { hasJWT } = useSession();

    return (hasJWT() ? <Outlet/> : <Navigate to={RouterPaths.LoginPath} />);
}

export default AuthenticatedRoutes;