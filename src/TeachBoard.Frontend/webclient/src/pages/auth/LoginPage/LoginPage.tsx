import React, {useEffect} from 'react';
import {useAuthStore} from "../useAuthStore";


export const LoginPage = () => {

    const store = useAuthStore();

    useEffect(() => {
        store.login({userName : 'kalnitskiy', password: 'kalnitskiy'});
    }, []);

    useEffect(() => {
        console.log(store);
    }, [store.isLoading])

    return (
        <div>
            I love .NET but not react
        </div>
    );
};
