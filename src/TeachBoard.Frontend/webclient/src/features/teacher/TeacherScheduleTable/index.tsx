import React from 'react';
import {ITeacherScheduleItem} from "../../../entities";
import {Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";
import './index.css'

interface ITeacherScheduleTableProps {
    lessons: ITeacherScheduleItem[]
}

export const TeacherScheduleTable: React.FC<ITeacherScheduleTableProps> = ({lessons}) => {

    return (
        <TableContainer component={Paper}>
            <Table sx={{minWidth: 650}} aria-label="simple table">
                <TableHead className='schedule-table-heading'>
                    <TableRow>
                        <TableCell className='table-heading-item'>#</TableCell>
                        <TableCell className='table-heading-item'>Subject</TableCell>
                        <TableCell className='table-heading-item'>Group id</TableCell>
                        <TableCell className='table-heading-item'>Classroom</TableCell>
                        <TableCell className='table-heading-item'>Start time</TableCell>
                        <TableCell className='table-heading-item'>End time</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {lessons.map((item, index) => (
                        <TableRow
                            key={index}
                            sx={{'&:last-child td, &:last-child th': {border: 0}}}
                        >
                            <TableCell component="th" scope="row">
                                {index + 1}
                            </TableCell>
                            <TableCell>{item.subjectName}</TableCell>
                            <TableCell>{item.groupId}</TableCell>
                            <TableCell>{item.classroom}</TableCell>
                            <TableCell>{new Date(item.startsAt).toUTCString()}</TableCell>
                            <TableCell>{new Date(item.endsAt).toUTCString()}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};
