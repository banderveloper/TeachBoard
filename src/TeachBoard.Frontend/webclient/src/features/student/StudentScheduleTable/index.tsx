import React from 'react';
import {IScheduleItem} from "../../../entities";
import {Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";
import './index.css';
import {UTCToLocalStringTime} from "../../../shared";

interface IStudentScheduleTableProps {
    scheduleItems: IScheduleItem[]
}

export const StudentScheduleTable: React.FC<IStudentScheduleTableProps> = ({scheduleItems}) => {

    return (
        <TableContainer component={Paper}>
            <Table sx={{minWidth: 650}} aria-label="simple table">
                <TableHead className='schedule-table-heading'>
                    <TableRow>
                        <TableCell className='schedule-table-heading-item'>Subject</TableCell>
                        <TableCell className='schedule-table-heading-item'>Topic</TableCell>
                        <TableCell className='schedule-table-heading-item'>Classroom</TableCell>
                        <TableCell className='schedule-table-heading-item'>Start time</TableCell>
                        <TableCell className='schedule-table-heading-item'>End time</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {scheduleItems.map((item, index) => (
                        <TableRow
                            key={index}
                            sx={{'&:last-child td, &:last-child th': {border: 0}}}
                        >
                            <TableCell component="th" scope="row">
                                {item.subjectName}
                            </TableCell>
                            <TableCell>{item.topic}</TableCell>
                            <TableCell>{item.classroom}</TableCell>
                            <TableCell>{UTCToLocalStringTime(item.startsAt)}</TableCell>
                            <TableCell>{UTCToLocalStringTime(item.endsAt)}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};
