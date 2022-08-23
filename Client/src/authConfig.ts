import { Configuration } from "@azure/msal-browser";

// Config object to be passed to Msal on creation
export const msalConfig: Configuration = {
    auth: {
        clientId: String(process.env.REACT_APP_AUTH_CLIENT_ID),
        authority: "",
        knownAuthorities: [""],
        redirectUri: String(process.env.REACT_APP_AUTH_REDIRECTURI),
        postLogoutRedirectUri: String(process.env.REACT_APP_AUTH_POSTLOGOUTREDIRECTURI)
    },
    cache: {
        cacheLocation: "localStorage",
        storeAuthStateInCookie: true
    }
};