import React, {useEffect, useState} from 'react';
import {useCurrentLessonStore} from "./store";
import {
    Box,
    Button, CircularProgress,
    Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
    TextField
}
    from '@mui/material';
import {CurrentLessonStudentActivityItem} from "../../../features";
import './index.css'


export const TeacherCurrentLessonPage = () => {

    const {current, loadCurrentLesson, isLoading, sendUpdatedTopic} = useCurrentLessonStore();
    const [topic, setTopic] = useState('');

    const handleTopicChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTopic(event.target.value);
    };

    useEffect(() => {
        loadCurrentLesson();
    }, []);
    useEffect(() => {
        setTopic(current?.lesson.topic ?? '');
    }, [current]);

    if (isLoading)
        return (
            <Box sx={{display: 'flex', margin: '25px'}}>
                <CircularProgress/>
            </Box>
        );
    else if (current == null)
        return <h1>You have not lesson now</h1>

    return (
        <Box>
            <Box className='current-lesson-subject-block'>
                <h1>{current.lesson.subjectName}</h1>
                <TextField
                    className='current-lesson-subject-block-input'
                    label="Lesson Topic"
                    value={topic}
                    onChange={handleTopicChange}

                />
                <Button
                    variant="contained"
                    color="primary"
                    onClick={() => sendUpdatedTopic(topic)}
                >
                    Send
                </Button>
            </Box>

            <TableContainer>
                <Table>
                    <TableHead>
                        <TableRow className='current-lesson-table-heading'>
                            <TableCell>#</TableCell>
                            <TableCell>Avatar</TableCell>
                            <TableCell>First Name</TableCell>
                            <TableCell>Last Name</TableCell>
                            <TableCell>Patronymic</TableCell>
                            <TableCell>Grade</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Send</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {current.students.map((student, index) => (
                            <CurrentLessonStudentActivityItem
                                key={student.studentId}
                                activity={student}
                                index={index}
                                lessonId={current?.lesson.id}
                            />
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    );
};
