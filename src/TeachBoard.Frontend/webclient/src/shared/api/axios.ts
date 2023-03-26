import axios, {AxiosResponse} from "axios";
import {API_URL} from "./endpoints";

const $api = axios.create({
    baseURL: API_URL,
});

export interface IApiResponse<T> {
    data?: T,
    error?: any;
}

$api.interceptors.response.use(
    (response: AxiosResponse<any>): AxiosResponse<IApiResponse<any>> => {
        return response;
    },
    (error) => Promise.reject(error)
);

$api.interceptors.request.use(function (config) {
    config.headers.Authorization = `Bearer ${localStorage.getItem('accessToken')}`;
    config.headers.set("Access-Control-Allow-Origin", "*");
    config.headers.set("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    config.headers.set("Access-Control-Expose-Headers: *");
    return config;
}, function (error) {
    // Do something with request error
    return Promise.reject(error);
});

export default $api;