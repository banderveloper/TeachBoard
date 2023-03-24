import React from 'react';
import {EnumUserRole, useAuthStore} from "../../entities";
import {UnauthorizedNavbar} from "./UnauthorizedNavbar";
import {StudentNavbar} from "./StudentNavbar";
import {TeacherNavbar} from "./TeacherNavbar";
import {AdministratorNavbar} from "./AdministratorNavbar";

export const Navbar = () => {

    const {isLoggedIn, role} = useAuthStore();

    const getRoleNavbar = () => {
        if (!isLoggedIn) return <UnauthorizedNavbar/>

        switch (role) {
            case EnumUserRole.student:
                return <StudentNavbar/>
            case EnumUserRole.teacher:
                return <TeacherNavbar/>
            case EnumUserRole.administrator:
                return <AdministratorNavbar/>
            default:
                return <UnauthorizedNavbar/>
        }
    }

    return (
        <div>
            {getRoleNavbar()}
        </div>
    );
};
