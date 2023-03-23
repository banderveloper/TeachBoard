import React, {useEffect} from 'react';
import {LoginPage, PrivateRoute} from "../pages";
import {Routes, Route} from "react-router-dom";
import {EnumUserRole} from "../entities";
import StudentHomePage from "../pages/student/StudentHomePage/StudentHomePage";
function App() {

    console.log('help me')

    return (

        <div>
            <h1>Home page</h1>
            <Routes>
                <Route path="login" element={<LoginPage />} />
                <Route
                    path="student"
                    element={
                        <PrivateRoute requiredRole={EnumUserRole.student}>
                            <StudentHomePage/>
                        </PrivateRoute>
                    }
                />
            </Routes>
        </div>
    );
}

export default App;
