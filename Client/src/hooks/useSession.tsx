import { LocalStorage } from "utility/LocalStorage";

interface IUseSession {
    getJWT: () => string | null;
    hasJWT: () => boolean;
    storeJWT: (token: string, expires: Date) => void;
    clearJWT: () => void;
}

export const useSession = (): IUseSession => {

    const getJWT = () => {
        return localStorage.getItem(LocalStorage.Token);
    };

    const hasJWT = () => {
        return localStorage.getItem(LocalStorage.Token) != null;
    };

    const storeJWT = (token: string, expires: Date) => {
        localStorage.setItem(LocalStorage.Token, token);
        localStorage.setItem(LocalStorage.Expires, expires.toString());
    }

    const clearJWT = () => {
        localStorage.removeItem(LocalStorage.Token);
        localStorage.removeItem(LocalStorage.Expires);
    };

    return { getJWT, hasJWT, storeJWT, clearJWT };
};