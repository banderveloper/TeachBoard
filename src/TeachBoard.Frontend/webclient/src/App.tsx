import React, {useEffect} from 'react';
import {useAuthStore} from "./state/useAuthStore";

function App() {

    const store = useAuthStore();

    useEffect(() => {
        store.login({'userName': 'kalnitskiy', 'password': 'kalnitskiy'});
    }, []);

    useEffect(() => {
        console.log('Store:', store);
    }, [store.isLoading])



    return (
        <div>hello world</div>
    );
}

export default App;
