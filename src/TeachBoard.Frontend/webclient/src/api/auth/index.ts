import $api from '../api'
import Endpoints from "../endpoints";
import {AxiosPromise} from "axios";
import {ILoginRequest, ILoginResponse} from "./types";

export const login = (params: ILoginRequest): AxiosPromise<ILoginResponse> =>
    $api.post(Endpoints.AUTH.LOGIN, params);

// export const logout = () : AxiosPromise => {
//     return axiosInstance.get(Endpoints.AUTH.LOGOUT);
// }
