import React from 'react';
import {EnumUserRole, useAuthStore} from "../../../entities";
import {Navigate} from "react-router-dom";
import {RegisterForm} from "../../../features";

export const RegisterPage = () => {

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
        }
    }

    return (<RegisterForm/>);
};
