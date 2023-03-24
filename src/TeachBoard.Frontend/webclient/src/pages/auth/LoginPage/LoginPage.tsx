import React, {useEffect} from 'react';
import {useAuthStore} from "../../../entities";
import {Navigate, useLocation} from "react-router-dom";
import {EnumUserRole} from "../../../entities";


export const LoginPage = () => {

    const {isLoading, login, role, isLoggedIn} = useAuthStore();

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

    const onClicked: React.MouseEventHandler<HTMLButtonElement> = (event) => {
        event.preventDefault();
        login({userName: 'kalnitskiy', password: 'kalnitskiy'});
    }

    return (
        <div>
            <p>Login page</p>
            <button onClick={onClicked}>Login</button>
        </div>
    );
};
