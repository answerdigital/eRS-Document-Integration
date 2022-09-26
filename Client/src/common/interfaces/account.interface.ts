export interface IUser {
    userEmail: string;
    userForename?: string;
    userSurname?: string;
    userFullName?: string;
}

export interface ILoginRequest {
    email: string;
    password: string;
}

export interface IAuthenticatedResponse {
    token: string;
    validTo: Date;
}
