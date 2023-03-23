import axios, {AxiosResponse} from "axios";
import {SERVER_URL} from "./endpoints";

const $api = axios.create({
    baseURL: SERVER_URL,
    //withCredentials: true
});

export interface IApiResponse<T> {
    data?: T,
    error?: any;
}

$api.interceptors.response.use(
    (response: AxiosResponse<any>): AxiosResponse<IApiResponse<any>> => {
        if (response.data && response.data.hasOwnProperty('data') && response.data.hasOwnProperty('error')) {
            return response;
        }
        return {
            ...response,
            data: {error: 'Unexpected response from server.'}
        };
    },
    (error) => Promise.reject(error)
);

$api.interceptors.request.use(function (config) {
    config.headers.Authorization = `Bearer ${localStorage.getItem('accessToken')}`;
    config.headers.set("Access-Control-Allow-Origin", "*");
    config.headers.set("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    return config;
}, function (error) {
    // Do something with request error
    return Promise.reject(error);
});

export default $api;