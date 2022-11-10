import { AxiosError } from "axios";
import { IAuthenticatedResponse, ILoginRequest, IUser } from "common/interfaces/account.interface";
import { useUserDetails } from "contexts/SessionContext";
import { useWorkflowStates } from "contexts/WorkflowStatesContext";
import { useSession } from "hooks/useSession";
import React, { useState } from "react";
import { FaEye, FaEyeSlash } from "react-icons/fa";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { getUserDetails, tryLogin } from "services/account-service";
import { RouterPaths } from "utility/RouterPaths";


const Login: React.FC = () => {
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [showPassword, setShowPassword] = useState<boolean>(false);
    const [loggingIn, setLoggingIn] = useState<boolean>(false);

    const { storeJWT } = useSession();
    const { setUserDetails } = useUserDetails();
    const { fetchStatusList } = useWorkflowStates();
    const navigate = useNavigate();

    const resetDetails = () => {
        setEmail("");
        setPassword("");
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        let emailValid = false;
        let passwordValid = false;
        const emailLength = validateLength(email, 1, 255);
        const passwordLength = validateLength(password, 1, 16);

        if (!fieldExists(email)) {
            toast.warning("Email field is required.", { });
        }else if (emailLength === 1) {
            toast.warning("Email field cannot be longer than 30 characters.", { });
        }else if (!validateEmail(email)) {
            toast.warning("Invalid email format.", { });
        }else{
            emailValid = true;
        }

        if (!fieldExists(password)) {
            toast.warning("Password field is required.", { });
        }else if (passwordLength === -1) {
            toast.warning("Password field must be at least 8 characters long.", { });
        }else if (passwordLength === 1) {
            toast.warning("Password field cannot be longer than 16 characters.", { });
        }else{
            passwordValid = true;
        }

        if (emailValid && passwordValid) {
            login();
        }
    };

    const fieldExists = (str: string) => {
        return str !== "";
    };

    const validateLength = (str: string, min: number, max: number): number => {
        if (str.length < min) {
            return -1;
        }
        if (str.length > max) {
            return 1;
        }
        return 0;
    };

    const validateEmail = (email: string) => {
        return email.toLowerCase().match(
            /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
        );
    };

    const login = () => {
        if (!loggingIn) {

            setLoggingIn(true);
            const loginRequest : ILoginRequest = {
                email: email,
                password: password
            }

            tryLogin(loginRequest).then(async (authResponse: IAuthenticatedResponse | undefined) => {
                if (authResponse) {
                    storeJWT(authResponse.token, authResponse.validTo);
                    getUserDetails().then((userResponse: IUser | undefined) => {
                        if (userResponse) {
                            setUserDetails(userResponse);
                        }
                        toast.success("Login succeeded!", { });
                        fetchStatusList();
                        navigate(RouterPaths.WorklistPath, { replace: true });
                    });
                }
                setLoggingIn(false);
            }).catch((error: AxiosError) => {
                if (error.response?.status === 404) {
                    toast.error("Login failed!", { });
                }
                resetDetails();
                setLoggingIn(false);
            });
        }
    };

    return (
    <div className='d-flex flex-column'>
        <form onSubmit={handleSubmit}>
            <div className='form-floating mt-2'>
                <input
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className='form-control'
                placeholder='Email'
                />
                <label>Email address</label>
            </div>
            <div className='input-group mt-2'>
                <div className='form-floating'>
                    <input
                    type={showPassword ? 'text' : 'password'}
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    className='form-control'
                    placeholder='Password'
                    />
                    <label>Password</label>
                </div>
                <button
                className='btn btn-outline-secondary'
                onClick={() => setShowPassword(!showPassword)}
                >
                    {showPassword ? <FaEyeSlash/> : <FaEye/>}
                </button>
            </div>
            <div className='d-flex justify-content-end mt-3'>
                <input
                type='submit'
                className='btn btn-primary'
                value='Sign in'
                />
            </div>
        </form>
    </div>
    );
}

export default Login;