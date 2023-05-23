import React, {useEffect, useState} from 'react';
import {Button, MenuItem, Select, TableCell, TableRow, TextField} from "@mui/material";
import {EnumStudentLessonAttendanceStatus, IStudentCurrentLessonActivityItem} from "../../../entities";
import './index.css'
import {$api} from "../../../shared";
import ENDPOINTS from "../../../shared/api/endpoints";

interface CurrentLessonStudentActivityItemProps {
    activity: IStudentCurrentLessonActivityItem;
    index: number;
    lessonId: number;
}

export const CurrentLessonStudentActivityItem: React.FC<CurrentLessonStudentActivityItemProps> = (props) => {

    const [grade, setGrade] = useState(props.activity.grade ?? 0);
    const [attendanceStatus, setAttendanceStatus] = useState(props.activity.attendanceStatus ?? EnumStudentLessonAttendanceStatus.absent);
    const [isLoading, setIsLoading] = useState(false);

    const handleSendActivity = async () => {
        setIsLoading(true);

        await $api.post(ENDPOINTS.TEACHER.SET_STUDENT_LESSON_ACTIVITY, {
            studentId: props.activity.studentId,
            lessonId: props.lessonId,
            attendanceStatus: attendanceStatus,
            grade: grade
        });

        setIsLoading(false);
    }

    return (
        <TableRow key={props.activity.studentId} className='current-lesson-student'>
            <TableCell>{props.index + 1}</TableCell>
            <TableCell>
                <img className='current-lesson-student-avatar' src={props.activity.avatarImagePath ?? ''}
                     alt={`${props.activity.firstName}'s avatar`}/>
            </TableCell>
            <TableCell>{props.activity.firstName}</TableCell>
            <TableCell>{props.activity.lastName}</TableCell>
            <TableCell>{props.activity.patronymic}</TableCell>
            <TableCell>
                <TextField
                    type="number"
                    value={grade}
                    disabled={attendanceStatus === EnumStudentLessonAttendanceStatus.absent}
                    onChange={(event) => {
                        setGrade(parseInt(event.target.value));
                    }}
                    InputProps={{
                        inputProps: {
                            min: 1,
                            max: 12
                        }
                    }}
                />
            </TableCell>
            <TableCell>
                <Select
                    labelId={`status-select-label-${props.index}`}
                    id={`status-select-${props.index}`}
                    value={attendanceStatus}
                    onChange={(event) => {
                        setAttendanceStatus(event.target.value as string);
                    }}
                >
                    <MenuItem value={EnumStudentLessonAttendanceStatus.absent}>
                        {EnumStudentLessonAttendanceStatus.absent}
                    </MenuItem>
                    <MenuItem value={EnumStudentLessonAttendanceStatus.late}>
                        {EnumStudentLessonAttendanceStatus.late}
                    </MenuItem>
                    <MenuItem value={EnumStudentLessonAttendanceStatus.attended}>
                        {EnumStudentLessonAttendanceStatus.attended}
                    </MenuItem>
                </Select>
            </TableCell>
            <TableCell>
                <Button variant="contained" color="primary" disabled={isLoading}
                        onClick={handleSendActivity}>
                    Send
                </Button>
            </TableCell>
        </TableRow>
    );
};
