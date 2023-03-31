import React from 'react';
import {useAuthStore} from "../../../entities";
import {Navigate} from "react-router-dom";
import {EnumUserRole} from "../../../entities";
import {LoginForm} from "../../../features";


export const LoginPage = () => {

    const {role, isLoggedIn} = useAuthStore();

    if (isLoggedIn) {
        switch (role) {
            case EnumUserRole.student:
                return <Navigate to='/student'/>
            case EnumUserRole.teacher:
                return <Navigate to='/teacher'/>
            case EnumUserRole.administrator:
                return <Navigate to='/administrator'/>
            default:
                console.error('Unexpected user role');
                break;
        }
    }

    return (
        <LoginForm/>
    );
};
