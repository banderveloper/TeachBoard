import axios, {AxiosResponse} from "axios";
import {API_URL} from "./endpoints";
import {useAuthStore} from "../../entities";

const $api = axios.create({
    baseURL: API_URL,
});

export interface IApiResponse<T> {
    data?: T,
    error?: any;
}

$api.interceptors.response.use((response) => {

    console.log('RESPONSE: ', response);
    return response;
}, (error) => {

    console.log('ERROR RESPONSE');
    console.log(error);

    if (error.response.status === 401 || error.response.status === 403) {
        localStorage.removeItem('useAuthUser');
        window.location.href = '/login';
    }


    return Promise.reject(error.message);
});

$api.interceptors.request.use(function (config) {
    config.headers.Authorization = `Bearer ${localStorage.getItem('accessToken')}`;
    config.headers.set("Access-Control-Allow-Origin", "*");
    config.headers.set("Access-Control-Allow-Headers", "*");
    config.headers.set("Access-Control-Expose-Headers: *");
    return config;
}, function (error) {
    // Do something with request error
    return Promise.reject(error);
});

export default $api;