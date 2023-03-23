import {Role} from "../types";
import {ILoginRequest, ILoginResponse} from "../api/auth/types";
import {create} from "zustand";
import jwtDecode from "jwt-decode";
import $api, {IApiResponse} from "../api/api";
import Endpoints from "../api/endpoints";

interface IAuthStore {
    isLoggedIn: boolean;
    role: Role | null,
    accessToken: string | null;
    isLoading: boolean;
    errorCode: string | null;
    errorMessage: string | null;

    login: (params: ILoginRequest) => void;
}

export const useAuthStore = create<IAuthStore>((set) => ({
    isLoading: false,
    accessToken: null,
    role: null,
    isLoggedIn: false,
    errorCode: null,
    errorMessage: null,

    login: async (params: ILoginRequest) => {
        set({isLoading: true});

        const response = await $api.post<IApiResponse<ILoginResponse>>(Endpoints.AUTH.LOGIN, params);

        if (response.data.error) {
            console.error('error logging');
            const error = response.data.error;
            set({errorCode: error.errorCode, errorMessage: error.message})
        } else {
            const data = response.data.data!;
            const token = data.accessToken;

            set({
                role: decodeJwtToken(token) as Role | null,
                accessToken: token
            });
            localStorage.setItem('accessToken', token);

            set({isLoggedIn: true});
        }

        set({isLoading: false});
    }
}));


// Function to decode JWT token and extract user's role
function decodeJwtToken(token: string): { role: Role } | null {
    try {
        return jwtDecode(token) as { role: Role };
    } catch (error) {
        console.error('Error decoding JWT token:', error);
        return null;
    }
}
