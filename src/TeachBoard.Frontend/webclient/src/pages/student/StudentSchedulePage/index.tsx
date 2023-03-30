import React, {useEffect} from 'react';
import {StudentScheduleTable} from '../../../features';
import {useScheduleStore} from "./store";
import {Box, CircularProgress} from "@mui/material";

export const StudentSchedulePage = () => {

    const {scheduleItems, loadSchedule, isLoading} = useScheduleStore();

    useEffect(() => {
        loadSchedule();
    }, []);

    return (
        isLoading
            ?
            <Box sx={{display: 'flex', margin: '25px'}}>
                <CircularProgress/>
            </Box>
            :
            <StudentScheduleTable scheduleItems={scheduleItems}/>
    )
};
