import React, {useEffect} from 'react';
import {useUncheckedHomeworksCountStore} from "./store";
import {
    Box,
    CircularProgress,
    Table,
    TableCell,
    TableContainer, TableHead, TableBody, TableRow
} from "@mui/material";
import './index.css'

export const UncheckedHomeworksCountPage = () => {

    const {loadUncheckedHomeworksCount, uncheckedHomeworksCountItems, isLoading} = useUncheckedHomeworksCountStore();

    useEffect(() => {
        loadUncheckedHomeworksCount();
    }, []);

    if (isLoading)
        return <Box sx={{display: "flex", margin: "25px"}}>
            <CircularProgress/>
        </Box>

    return (
        <TableContainer className='unchecked-homeworks-count-table'>
            <Table>
                <TableHead>
                    <TableRow className='unchecked-homeworks-count-table-heading'>
                        <TableCell>Teacher</TableCell>
                        <TableCell>Unchecked homeworks count</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {uncheckedHomeworksCountItems
                        .sort((left, right) => right.homeworksCount - left.homeworksCount)
                        .map((item, index) => (
                        <TableRow key={index}>
                            <TableCell>{item.teacherFullName}</TableCell>
                            <TableCell>{item.homeworksCount}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};
