import create from 'zustand'
import {Role} from "../types";
import jwtDecode from "jwt-decode";

interface IAuthState {
    token: string | null;
    role: Role | null;
    isLoggedIn: boolean;
    setToken: (token: string | null) => void;
    setRole: (role: Role | null) => void;
}

export const useAuthStore = create<IAuthState>((set) => ({
    token: null,
    role: null,
    isLoggedIn: false,
    setToken: (token) => {
        set((state) => ({...state, token}));
        // Decode the JWT token and set the user's role in the store
        if (token) {
            const decodedToken = decodeJwtToken(token);
            console.log("Decoded token:", decodedToken);
            if (decodedToken) {
                set((state) => ({...state, role: decodedToken.role, isLoggedIn: true}));
            }
        } else {
            set((state) => ({...state, role: null, isLoggedIn: false}));
        }
    },
    setRole: (role) => set((state) => ({...state, role}))
}));

// Function to decode JWT token and extract user's role
function decodeJwtToken(token: string): { role: Role } | null {
    try {
        const decoded = jwtDecode(token) as { role: Role };
        return decoded;
    } catch (error) {
        console.error('Error decoding JWT token:', error);
        return null;
    }
}