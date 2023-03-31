import React from 'react';
import {EnumUserRole, useAuthStore} from "../../../entities";
import {useNavigate} from "react-router-dom";
import {RegisterForm} from "../../../features";

export const RegisterPage = () => {

    const {role, isLoggedIn} = useAuthStore();
    const navigate = useNavigate();

    if (isLoggedIn) {
        switch (role) {
            case EnumUserRole.student:
                navigate({pathname: '/student'});
                break;
            case EnumUserRole.teacher:
                navigate({pathname: '/teacher'});
                break;
            case EnumUserRole.administrator:
                navigate({pathname: '/administrator'});
                break;
            default:
                console.error('Unexpected user role');
                break;
        }
    }

    return (<RegisterForm/>);
};
