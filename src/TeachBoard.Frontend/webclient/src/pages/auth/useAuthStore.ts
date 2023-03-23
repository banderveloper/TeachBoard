import {EnumUserRole, ILoginRequest, ILoginResponse, IServerResponse} from "../../entities";
import {create} from "zustand";
import {$api, decodeJwtToken} from "../../shared";
import ENDPOINTS from "../../shared/api/endpoints";
import jwtDecode from "jwt-decode";
import {persist} from "zustand/middleware";

interface IAuthStore {
    isLoggedIn: boolean;
    role: string | null;
    accessToken: string | null;
    isLoading: boolean;
    errorCode: string | null;
    errorMessage: string | null;

    login: (params: ILoginRequest) => void;
}

export const useAuthStore = create<IAuthStore>()(persist((set) => ({
    isLoading: false,
    accessToken: null,
    role: null,
    isLoggedIn: false,
    errorCode: null,
    errorMessage: null,

    login: async (params: ILoginRequest) => {
        set({isLoading: true});

        const response = await $api.post<IServerResponse<ILoginResponse>>(ENDPOINTS.AUTH.LOGIN, params);

        if (response.data.error) {
            const error = response.data.error;
            set({errorCode: error.errorCode, errorMessage: error.message})
        } else {
            const data = response.data.data!;
            const token = data.accessToken;
            const decodedJwtToken = decodeJwtToken(token);

            set({
                role: decodedJwtToken?.userRole,
                accessToken: token
            });
            localStorage.setItem('accessToken', token);

            set({isLoggedIn: true});
        }

        set({isLoading: false});
    }
}), {name: 'useAuthUser', version: 1}));

