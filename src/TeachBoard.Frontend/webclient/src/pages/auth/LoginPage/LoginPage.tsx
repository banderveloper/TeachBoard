import React, {useEffect} from 'react';
import {useAuthStore} from "../useAuthStore";


export const LoginPage = () => {

    const store = useAuthStore();

    useEffect(() => {
        console.log(store);
    }, [store.isLoading])

    const onClicked: React.MouseEventHandler<HTMLButtonElement> = (event) => {
        event.preventDefault();
        store.login({userName: 'kalnitskiy', password: 'kalnitskiy'});
    }

    return (
        <div>
            <p>Login page</p>
            <button onClick={onClicked}>Login</button>
        </div>
    );
};
