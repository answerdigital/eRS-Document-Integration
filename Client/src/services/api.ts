import axios, { AxiosError, AxiosRequestConfig } from "axios";
import { toast } from 'react-toastify';
import { LocalStorage } from "utility/LocalStorage";

const customAxios = axios.create({
    baseURL: `${process.env.REACT_APP_API_Address}/api`
});

customAxios.interceptors.request.use(
    async (axiosRequestConfig: AxiosRequestConfig) => {
        let jwt = localStorage.getItem(LocalStorage.Token);
        const expires = localStorage.getItem(LocalStorage.Expires);

        if (jwt) {
            /*
            if (expires) {
                if (new Date() >= new Date(expires)) {
                    jwt = await 
                }
            }
            */

            axiosRequestConfig.headers = {
                ...axiosRequestConfig.headers,
                authorization: `Bearer ${jwt}`,
            };
        }

        return axiosRequestConfig;
    }, (error: AxiosError) => Promise.reject(error)
);

/*
customAxios.interceptors.request.use(
    async (axiosRequestConfig: AxiosRequestConfig) => {
        const account = msalInstance.getAllAccounts()[0];
        const accessTokenRequest = {
            account: account,
            scopes: [ "offline_access", "openid", msalConfig.auth.clientId ]
        }
        await msalInstance.acquireTokenSilent(accessTokenRequest).then(response => {
            if (axiosRequestConfig.headers != null){
                axiosRequestConfig.headers['Authorization'] = "Bearer " + response.accessToken;
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
*/

export const printApiError = (error: any) => {
    toast.error(`Server error! Code ${error.response?.status}`, { });
};

export default customAxios;