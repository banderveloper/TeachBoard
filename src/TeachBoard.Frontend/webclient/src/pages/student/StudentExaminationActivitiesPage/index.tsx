import React, {useEffect} from 'react';
import {useExaminationActivitiesStore} from "./store";
import {StudentExaminationActivitiesTable} from "../../../features";
import {Box, CircularProgress} from "@mui/material";
yexport const StudentExaminationActivitiesPage = () => {

    const {loadExaminationActivities, isLoading, examinations} = useExaminationActivitiesStore();

    useEffect(() => {
        loadExaminationActivities();
    }, []);

    return (
        isLoading
            ?
            <Box sx={{display: 'flex', margin: '25px'}}>
                <CircularProgress/>
            </Box>
            :
            <StudentExaminationActivitiesTable examinationActivities={examinations}/>
    );
};
