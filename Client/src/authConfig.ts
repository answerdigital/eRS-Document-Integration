import { Configuration } from "@azure/msal-browser";

// Config object to be passed to Msal on creation
export const msalConfig: Configuration = {
    auth: {
        clientId: String(process.env.REACT_APP_AUTH_CLIENT_ID),
        authority: `https://login.microsoftonline.com/d3db4717-2853-4110-bc89-f41b4c7eb1c6`,
        redirectUri: String(process.env.REACT_APP_AUTH_REDIRECTURI),
        postLogoutRedirectUri: String(process.env.REACT_APP_AUTH_POSTLOGOUTREDIRECTURI)
    },
    cache: {
        cacheLocation: "localStorage",
        storeAuthStateInCookie: true
    }
};

export const loginRequest = {
    scopes: ["User.Read"]
};