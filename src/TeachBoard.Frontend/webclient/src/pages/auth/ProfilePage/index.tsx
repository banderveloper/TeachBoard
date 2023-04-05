import React, {useEffect} from 'react';
import {EnumUserRole, useAuthStore} from "../../../entities";
import {Navigate} from "react-router-dom";
import {StudentProfileBlock} from "../../../features";
import {useStudentProfileStore} from "./store";
import {Box, CircularProgress} from "@mui/material";

export const ProfilePage = () => {

    const {isLoggedIn, role} = useAuthStore();
    const {loadProfileData, profileData, isLoading} = useStudentProfileStore();

    useEffect(() => {
        loadProfileData();
    }, []);

    if (!isLoggedIn)
        return <Navigate to='/login'/>

    return (
       profileData === null ?
            <Box sx={{display: 'flex', justifyContent: 'center'}}>
                <CircularProgress/>
            </Box>
            :
            <StudentProfileBlock user={profileData.user} group={profileData.group}/>
    )
};
