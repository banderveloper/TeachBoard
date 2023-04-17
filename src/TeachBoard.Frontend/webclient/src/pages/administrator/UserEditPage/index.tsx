import React, {useEffect} from 'react';
import {useUserEditStore} from "./store";
import {Box, CircularProgress} from "@mui/material";

export const UserEditPage = () => {

    const urlLast = new URL(window.location.href).pathname.split('/').pop();
    const userId = parseInt(urlLast!);

    const store = useUserEditStore();

    useEffect(() => {
        store.setUserId(userId);
        store.loadUserData();
    }, []);

    if(store.isLoading)
        return <Box sx={{display: 'flex', margin: '25px'}}>
            <CircularProgress/>
        </Box>

    return (
        <div>
            {JSON.stringify(store.user)}
        </div>
    );
};
