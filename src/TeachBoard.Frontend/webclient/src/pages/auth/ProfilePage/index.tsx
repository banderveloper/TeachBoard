import React from 'react';
import {EnumUserRole, useAuthStore} from "../../../entities";
import {Navigate} from "react-router-dom";
import {StudentProfileBlock} from "../../../features";

export const ProfilePage = () => {

    const {isLoggedIn, role} = useAuthStore();

    if(!isLoggedIn)
        return <Navigate to='/login'/>

   switch (role){
       case EnumUserRole.student:
           return <StudentProfileBlock/>
       default:
           console.error('Unknown user role');
           return <Navigate to='/login'/>
   }
};
