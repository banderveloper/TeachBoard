import React, {useEffect} from 'react';
import './index.css'
import {useLessonActivitiesStore} from "./store";
import {StudentLessonActivitiesTable} from "../../../features";
import {Box, CircularProgress} from "@mui/material";

export const StudentLessonActivitiesPage = () => {

    const {lessonActivities, loadLessonActivities, isLoading} = useLessonActivitiesStore();

    useEffect(() => {
        loadLessonActivities();
    }, []);

    return (
        isLoading
            ?
            <Box sx={{display: 'flex', margin: '25px'}}>
                <CircularProgress/>
            </Box>
            :
            <StudentLessonActivitiesTable lessonActivities={lessonActivities}/>
    );
};
