import { InteractionRequiredAuthError } from "@azure/msal-browser";
import { msalConfig } from "authConfig";
import axios, { AxiosError, AxiosRequestConfig } from "axios";
import { msalInstance } from "index";
import { toast } from 'react-toastify';

const customAxios = axios.create({
    baseURL: `${process.env.REACT_APP_API_Address}/api`
});

customAxios.interceptors.request.use(
    async (axiosRequestConfig: AxiosRequestConfig) => {
        const account = msalInstance.getAllAccounts()[0];
        const accessTokenRequest = {
            account: account,
            scopes: [ "User.Read", "offline_access", "openid" ]
        }
        await msalInstance.acquireTokenSilent(accessTokenRequest).then(response => {
            console.log(response.accessToken);
            if (axiosRequestConfig.headers != null) {
                axiosRequestConfig.headers['Authorization'] = "Bearer " + response.idToken;
            }
        }).catch(function (error) {
            //Acquire token silent failure, and send an interactive request
            console.log(error);
            if (error instanceof InteractionRequiredAuthError) {
                // noinspection JSIgnoredPromiseFromCall
                msalInstance.acquireTokenRedirect(accessTokenRequest);
            }
        });
        return axiosRequestConfig;
    }
);

export const printApiError = (error: any) => {
    if (error.response.status === 0) return;
    toast.error(`Server error! Code ${error.response?.status}`, { });
};

export default customAxios;