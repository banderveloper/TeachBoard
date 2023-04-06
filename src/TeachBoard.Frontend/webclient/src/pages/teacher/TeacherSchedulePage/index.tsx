import React, {useEffect} from 'react';
import {useTeacherScheduleStore} from "./store";
import {Box, CircularProgress} from "@mui/material";
import {TeacherScheduleTable} from "../../../features";

export const TeacherSchedulePage = () => {

    const {scheduleItems, loadSchedule, isLoading} = useTeacherScheduleStore();

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
            <TeacherScheduleTable lessons={scheduleItems}/>
    )
};
