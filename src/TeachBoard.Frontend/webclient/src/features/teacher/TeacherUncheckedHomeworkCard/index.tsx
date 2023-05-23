import {
    Button,
    Card,
    CardActions,
    CardContent,
    CardHeader,
    CircularProgress,
    TextField,
    Typography
} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {ICheckHomework, IServerResponse, ITeacherUncheckedHomework} from "../../../entities";
import Download from '@mui/icons-material/Download';
import CloudDownload from '@mui/icons-material/CloudDownload';
import './index.css'
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";
import fileDownload from "js-file-download";


export const TeacherUncheckedHomeworkCard: React.FC<ITeacherUncheckedHomework> = ({
                                                                                      id,
                                                                                      homeworkId,
                                                                                      studentComment,
                                                                                      studentId,
                                                                                      createdAt
                                                                                  }) => {
    const [isLoading, setIsLoading] = useState(false);
    const [mark, setMark] = useState<number>(0);
    const [comment, setComment] = useState('');
    const [isChecked, setIsChecked] = useState(false);

    const handleDownloadTask = async () => {
        setIsLoading(true);

        const response = await $api.get<Blob>(Endpoints.TEACHER.GET_HOMEWORK_TASK_FILE + homeworkId,
            {
                responseType: "blob",
            }
        );

        const header = response.headers['content-disposition'];
        const filename = header.split(';')[1].trim().split('=')[1];
        fileDownload(response.data as Blob, filename);

        setIsLoading(false);
    };

    const handleDownloadSolution = async () => {
        setIsLoading(true);

        const response = await $api.get<Blob>(Endpoints.TEACHER.GET_HOMEWORK_SOLUTION_FILE + homeworkId + '/' + studentId,
            {
                responseType: "blob",
            }
        );

        const header = response.headers['content-disposition'];
        const filename = header.split(';')[1].trim().split('=')[1];
        fileDownload(response.data as Blob, filename);

        setIsLoading(false);
    };

    const handleSend = async () => {

        setIsLoading(true);

        const request: ICheckHomework = {
            comment: comment,
            completedHomeworkId: homeworkId,
            grade: mark
        };

        const response = await $api.post<IServerResponse<any>>(Endpoints.TEACHER.CHECK_HOMEWORK, request);

        if(response.status === 200){
            setIsLoading(false);
            setIsChecked(true);
        }
    };

    useEffect(() => {

        if(isChecked){
            const parent = document.querySelector('.unchecked-homeworks-container');
            const child = document.getElementById('hw-' + homeworkId.toString());

            console.log('parent', parent);
            console.log('child', child);

            if (parent && child) {
                parent.removeChild(child);
            }
        }

    }, [isChecked]);


    return (
        <Card className='unchecked-homework-card' id={'hw-'+ homeworkId.toString()}>
            <CardHeader title={`Homework ${id}`}/>
            <CardContent>
                <Button disabled={isLoading} startIcon={<CloudDownload/>} onClick={handleDownloadTask}>
                    Download Task
                </Button>
                <Button disabled={isLoading} startIcon={<Download/>} onClick={handleDownloadSolution}>
                    Download Solution
                </Button>
                {isLoading && <CircularProgress size={24}/>}
                <TextField
                    type="number"
                    label="Mark"
                    value={mark}
                    onChange={(e) => setMark(parseInt(e.target.value))}
                    InputLabelProps={{shrink: true}}
                    inputProps={{min: 1, max: 12}}
                    className='grade-input'
                />
                <TextField
                    label="Comment"
                    value={comment}
                    onChange={(e) => setComment(e.target.value)}
                    InputLabelProps={{shrink: true}}
                    placeholder={studentComment ?? ''}
                />
            </CardContent>
            <CardActions>
                <Button variant="contained" color="primary" onClick={handleSend} disabled={!mark}>
                    Send
                </Button>
            </CardActions>
        </Card>
    );
};
