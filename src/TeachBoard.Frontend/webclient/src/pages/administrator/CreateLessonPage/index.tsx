import {Box, Button, CircularProgress, FormControl, Grid, InputLabel, MenuItem, Select, TextField} from '@mui/material';
import React, {useEffect} from 'react';
import {useCreateLessonStore} from "./store";
import './index.css'
import '@bevacqua/rome'
import {DateTimePicker} from "@mui/x-date-pickers";


export const CreateLessonPage = () => {

    const store = useCreateLessonStore();

    useEffect(() => {
        store.loadSelectData();
    }, []);


    if(store.isSelectDataLoading)
        return <Box sx={{display: "flex", margin: "25px"}}>
            <CircularProgress/>
        </Box>

    return (
        <Grid container spacing={2} className="create-lesson-block">
            <Grid item xs={12}>
                <FormControl fullWidth className="create-lesson-block-input">
                    <InputLabel variant='standard' id="subject-label" className='create-lesson-label'>Subject</InputLabel>
                    <Select
                        labelId="subject-label"
                        id="subject"
                        name="subject"
                        value={store.selectedSubjectId}
                        onChange={(e) => store.setSubjectId(e.target.value as number)}
                    >
                        {store.selectData.subjects.map((subject) => (
                            <MenuItem key={subject.id} value={subject.id}>
                                {subject.name}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <FormControl fullWidth className="create-lesson-block-input">
                    <InputLabel variant='standard' id="teacher-label" className='create-lesson-label'>Teacher Name</InputLabel>
                    <Select
                        labelId="teacher-label"
                        id="teacher"
                        name="teacher"
                        value={store.selectedTeacherId}
                        onChange={(e) => store.setTeacherId(e.target.value as number)}
                    >
                        {store.selectData.teachers.map((teacher) => (
                                <MenuItem key={teacher.teacherId} value={teacher.teacherId}>
                                    {[teacher.lastName, teacher.firstName, teacher.patronymic].join(' ')}
                                </MenuItem>
                            ))}
                    </Select>
                </FormControl>
                <FormControl fullWidth className="create-lesson-block-input">
                    <InputLabel variant='standard'  id="group-label" className='create-lesson-label'>Group Name</InputLabel>
                    <Select
                        labelId="group-label"
                        id="group"
                        name="group"
                        value={store.selectedGroupId}
                        onChange={(e) => store.setGroupId(e.target.value as number)}
                    >
                        {store.selectData.groups.map((group) => (
                            <MenuItem key={group.id} value={group.id}>
                                {group.name}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <TextField
                    fullWidth
                    label="Classroom"
                    name="classroom"
                    value={store.classroom}
                    onChange={(e) => store.setClassroom(e.target.value)}
                    className="create-lesson-block-input"
                />
                <DateTimePicker
                    label="Start time"
                    value={store.startsAt}
                    onChange={(e) => {
                        store.setStartAt(e);
                        console.log('Changed time to', e);
                    }}
                    className='create-pending-block-input'
                />
            </Grid>
            <Grid item xs={12} className="create-lesson-send-container">
                <Button variant="contained" color="primary" onClick={store.sendCreateLessonRequest}>
                    Create Lesson
                </Button>
                {store.isLessonCreateLoading &&
                    <Box sx={{display: "flex", margin: "25px"}}>
                        <CircularProgress/>
                    </Box>
                }
            </Grid>
        </Grid>
    );
};
