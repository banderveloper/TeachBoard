export interface ILoginRequest {
    userName: string;
    password: string;
}

export interface ILoginResponse {
    accessToken: string;
    expires: number;
}