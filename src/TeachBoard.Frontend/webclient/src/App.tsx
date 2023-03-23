import React from 'react';
import {useAuthStore} from "./state/useAuthStore";

function App() {

    const {isLoggedIn, role} = useAuthStore();
    
    return (
        <div>hello world</div>
    );
}

export default App;
