import {axiosInstance} from "../api";
import Endpoints from "../endpoints";
import {AxiosPromise} from "axios";
import {ILoginRequest, ILoginResponse, IResponse} from "./types";

export const login = (params: ILoginRequest): AxiosPromise<IResponse<ILoginResponse>> =>
    axiosInstance.post(Endpoints.AUTH.LOGIN, params);

// export const logout = () : AxiosPromise => {
//     return axiosInstance.get(Endpoints.AUTH.LOGOUT);
// }
