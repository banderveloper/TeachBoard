export const SERVER_URL = 'http://localhost:5000/api'

const Endpoints = {
    AUTH: {
        LOGIN: `${SERVER_URL}/auth/login`,
        LOGOUT: `${SERVER_URL}/auth/logout`,
        REFRESH: `${SERVER_URL}/auth/refresh`,
    }
}

export default Endpoints;