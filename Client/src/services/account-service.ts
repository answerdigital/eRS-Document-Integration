import { AxiosError, AxiosResponse } from "axios";
import { IAuthenticatedResponse, ILoginRequest, IUser } from "common/interfaces/account.interface";
import api, { printApiError } from "./api";

export const tryLogin = async (request: ILoginRequest): Promise<IAuthenticatedResponse | undefined> => {
    return api.post('/Account/login', request)
        .then((response: AxiosResponse) => {
            return response.data;
        });
};

export const getUserDetails = async (jwt: string): Promise<IUser | undefined> => {
    return api.post('/Account/details', {token: jwt})
        .then((response: AxiosResponse) => {
            return response.data;
        }).catch((error: AxiosError) => {
            printApiError(error);
        });
};