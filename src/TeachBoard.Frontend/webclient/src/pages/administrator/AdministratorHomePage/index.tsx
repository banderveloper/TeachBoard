import React from 'react';
import {Navigate} from "react-router-dom";

export const AdministratorHomePage = () => {
    return (
        <Navigate to='/administrator/create-lesson'/>
    );
};
