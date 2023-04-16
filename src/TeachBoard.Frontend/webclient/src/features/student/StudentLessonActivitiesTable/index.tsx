import React from 'react';
import {IStudentLessonActivityItem} from "../../../entities";
import {Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";
import './index.css'
import {UTCToLocalStringTime} from "../../../shared";

interface IStudentLessonActivitiesTableProps{
    lessonActivities: IStudentLessonActivityItem[];
}

export const StudentLessonActivitiesTable: React.FC<IStudentLessonActivitiesTableProps> = ({lessonActivities}) => {
    return (
        <TableContainer component={Paper}>
            <Table sx={{minWidth: 650}} aria-label="simple table">
                <TableHead className='schedule-table-heading'>
                    <TableRow>
                        <TableCell className='table-heading-item'>#</TableCell>
                        <TableCell className='table-heading-item'>Subject</TableCell>
                        <TableCell className='table-heading-item'>Topic</TableCell>
                        <TableCell className='table-heading-item'>Status</TableCell>
                        <TableCell className='table-heading-item'>Grade</TableCell>
                        <TableCell className='table-heading-item'>Activity time</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {lessonActivities.map((item, index) => (
                        <TableRow
                            key={item.lessonId}
                            sx={{'&:last-child td, &:last-child th': {border: 0}}}
                            className={`${item.attendanceStatus.toLowerCase()}-activity-block`}
                        >
                            <TableCell component="th" scope="row">
                                {index+1}
                            </TableCell>
                            <TableCell>{item.subjectName}</TableCell>
                            <TableCell>{item.lessonTopic}</TableCell>
                            <TableCell>{item.attendanceStatus}</TableCell>
                            <TableCell>{item.grade}</TableCell>
                            <TableCell>{UTCToLocalStringTime(item.activityCreatedAt)}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

