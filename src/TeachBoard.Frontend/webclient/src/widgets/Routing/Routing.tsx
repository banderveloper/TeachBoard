import React from 'react';
import {Route, Routes} from "react-router-dom";
import {LoginPage} from "../../pages";
import {PrivateRoute} from "./PrivateRoute";
import {EnumUserRole} from "../../entities";
import {StudentHomePage} from "../../pages";

export const Routing = () => {
    return (
        <Routes>
            <Route path="/" element={<LoginPage/>}/>
            <Route path="login" element={<LoginPage/>}/>

            <Route path="student"
                   element={
                       <PrivateRoute requiredRole={EnumUserRole.student}>
                           <StudentHomePage/>
                       </PrivateRoute>
                   }
            />
        </Routes>
    );
};