import React from 'react';
import {useAuthStore} from "./state/useAuthStore";
import {login} from "./api/auth";

function App() {

    const {isLoggedIn, role} = useAuthStore();

    const a = async() => {
        const result = await login({userName:'hello', password:'hello'});
        console.log(result.data);
    }

    a();

    return (
        <div>hello world</div>
    );
}

export default App;
