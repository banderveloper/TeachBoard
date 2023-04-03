// Function to decode JWT token
import jwtDecode from "jwt-decode";

interface IOriginDecodedJwtToken{
    jti: string;
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier' : number;
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' : string;
    'exp' : number;
}

export interface IDecodedJwtToken {
    jti: string;
    userId: number;
    userRole: string;
    expires: number;
}

export function decodeJwtToken(token: string): IDecodedJwtToken | null {
    try {
        const decodedObject = jwtDecode<IOriginDecodedJwtToken>(token);
        return {
            jti: decodedObject.jti,
            expires: decodedObject.exp,
            userId: decodedObject["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
            userRole: decodedObject["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
        }
    } catch (error) {
        console.error('Error decoding JWT token:', error);
        return null;
    }
}