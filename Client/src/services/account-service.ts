import { AxiosError, AxiosResponse } from "axios";
import { IAuthenticatedResponse, ILoginRequest, IUser } from "common/interfaces/account.interface";
import api, { printApiError } from "./api";

export const tryLogin = async (request: ILoginRequest): Promise<IAuthenticatedResponse | undefined> => {
    return api.post('/Account/login', request)
        .then((response: AxiosResponse) => {
            return response.data;
        });
};

export const getUserDetails = async (): Promise<IUser | undefined> => {
    return api.get('/Account/details')
        .then((response: AxiosResponse) => {
            return response.data;
        }).catch((error: AxiosError) => {
            printApiError(error);
        });
};