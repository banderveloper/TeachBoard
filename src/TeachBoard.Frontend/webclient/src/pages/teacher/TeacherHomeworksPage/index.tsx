import React, {useEffect} from 'react';
import {useTeacherHomeworksStore} from "./store";
import {Box, CircularProgress} from "@mui/material";
import {TeacherUncheckedHomeworkCard} from "../../../features";
import './index.css'

export const TeacherHomeworksPage = () => {

    const {homeworks, loadHomeworks, isLoading} = useTeacherHomeworksStore();

    useEffect(() => {
        loadHomeworks();
    }, []);

    return (
        <div className='unchecked-homeworks-container'>
            {
                isLoading
                    ?
                    <Box sx={{display: 'flex', margin: '25px'}}>
                        <CircularProgress/>
                    </Box>
                    :
                    homeworks.map(homework => (
                        <TeacherUncheckedHomeworkCard
                            key={homework.homeworkId}
                            id={homework.id}
                            homeworkId={homework.homeworkId}
                            studentId={homework.studentId}
                            studentComment={homework.studentComment}
                            createdAt={homework.createdAt}/>
                    ))
            }
        </div>
    );
};
