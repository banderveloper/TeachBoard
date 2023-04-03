import {EnumUserRole, IApprovePendingStudentRequest, ILoginRequest, ILoginResponse, IServerResponse} from "../index";
import {create} from "zustand";
import {$api, decodeJwtToken} from "../../shared";
import ENDPOINTS from "../../shared/api/endpoints";
import jwtDecode from "jwt-decode";
import {persist} from "zustand/middleware";
import {AES, enc} from 'crypto-js'

const encryptState = (state: any) => {
    const encryptedState = AES.encrypt(JSON.stringify(state), "secret-key-from-environment");
    return encryptedState.toString();
};

const decryptState = (encryptedState: any) => {
    const decryptedState = AES.decrypt(encryptedState, "secret-key-from-environment");
    return JSON.parse(decryptedState.toString(enc.Utf8));
};

interface IAuthStore {
    isLoggedIn: boolean;
    role: string | null;
    accessToken: string | null;
    isLoading: boolean;
    errorCode: string | null;
    errorMessage: string | null;

    login: (params: ILoginRequest) => Promise<void>;
    register: (params: IApprovePendingStudentRequest) => Promise<void>;
    logout: () => Promise<void>;

    clearAuth: () => void;
    resetErrorInfo: () => void;
}

export const useAuthStore = create<IAuthStore>()(persist((set, get) => ({
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
    },

    register: async (params: IApprovePendingStudentRequest) => {

        set({isLoading: true});

        const response = await $api.post<IServerResponse<any>>(ENDPOINTS.STUDENT.APPROVE_PENDING, params);

        if (response.data.error) {
            const error = response.data.error;
            set({errorCode: error.errorCode, errorMessage: error.message})
        }

        set({isLoading: false});
    },

    logout: async () => {
        await $api.delete<IServerResponse<any>>(ENDPOINTS.AUTH.LOGOUT, {
            method: "DELETE"
        });
        const {clearAuth} = get();
        clearAuth();
    },

    clearAuth: () => {
        set({role: null, isLoggedIn: false, accessToken: null});
    },
    resetErrorInfo: () => {
        set({isLoading: false, errorCode: null, errorMessage: null});
    }

}), {
    name: 'useAuthUser',
    version: 1,
    serialize: state => encryptState(state),
    deserialize: state => decryptState(state)
}));

