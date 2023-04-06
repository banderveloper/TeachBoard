import React from 'react';
import {IStudentExaminationActivityItem} from "../../../entities";
import {Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";

interface IStudentExaminationActivitiesTableProps{
    examinationActivities: IStudentExaminationActivityItem[];
}

export const StudentExaminationActivitiesTable: React.FC<IStudentExaminationActivitiesTableProps> = ({examinationActivities}) => {
    return (
        <TableContainer component={Paper}>
            <Table sx={{minWidth: 650}} aria-label="simple table">
                <TableHead className='schedule-table-heading'>
                    <TableRow>
                        <TableCell className='table-heading-item'>#</TableCell>
                        <TableCell className='table-heading-item'>Subject</TableCell>
                        <TableCell className='table-heading-item'>Grade</TableCell>
                        <TableCell className='table-heading-item'>Status</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {examinationActivities.map((item, index) => (
                        <TableRow
                            key={item.examinationId}
                            sx={{'&:last-child td, &:last-child th': {border: 0}}}
                        >
                            <TableCell component="th" scope="row">
                                {index+1}
                            </TableCell>
                            <TableCell>{item.subjectName}</TableCell>
                            <TableCell>{item.grade}</TableCell>
                            <TableCell>{item.status}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

