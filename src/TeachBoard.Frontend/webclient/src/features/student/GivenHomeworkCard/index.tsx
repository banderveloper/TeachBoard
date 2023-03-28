import React, {ChangeEvent, useEffect, useState} from 'react';
import {IGivenHomework, IServerResponse} from "../../../entities";
import {
    Box,
    Button,
    Card,
    CardContent,
    CircularProgress,
    Divider,
    Grid,
    IconButton,
    TextField,
    Typography
} from "@mui/material";
import DownloadIcon from '@mui/icons-material/Download';
import FileUploadIcon from '@mui/icons-material/FileUpload';
import './index.css'
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";
import fileDownload from 'js-file-download';

export const GivenHomeworkCard: React.FC<IGivenHomework> = ({homeworkId, teacherId, subjectName, createdAt}) => {
    const [comment, setComment] = useState('');
    const [file, setFile] = useState<File>();
    const [isFileUploading, setIsFileUploading] = useState(false);
    const [isFileDownloading, setIsFileDownloading] = useState(false);
    const [isCompleted, setIsCompleted] = useState(false);

    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files) {
            setFile(e.target.files[0]);
        }
    };

    const handleCommentChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setComment(event.target.value);
    };

    const handleDownloadClick: React.MouseEventHandler<HTMLButtonElement> = async (event) => {
        event.preventDefault();

        setIsFileDownloading(true);

        const response = await $api.get<Blob>(Endpoints.STUDENT.GET_HOMEWORK_TASK_FILE + homeworkId,
            {
                responseType: "blob",
            }
        );

        const header = response.headers['content-disposition'];
        const filename = header.split(';')[1].trim().split('=')[1];
        fileDownload(response.data as Blob, filename);

        setIsFileDownloading(false);
    }

    const handleUploadClick: React.MouseEventHandler<HTMLButtonElement> = async (event) => {

        let formData = new FormData();
        formData.append('file', file!);
        formData.append('homeworkId', homeworkId.toString());
        formData.append('studentComment', comment);

        setIsFileUploading(true);

        const response = await $api.post<IServerResponse<any>>(Endpoints.STUDENT.UPLOAD_HOMEWORK_FILE, formData);
        console.log(response);

        if (response.status === 200) {
            setIsFileUploading(false);
            setIsCompleted(true);
        } else {
            console.error(response);
        }
    }

    useEffect(() => {

        if (isCompleted) {
            const parent = document.querySelector('.given-homework-card-container');
            const child = document.getElementById('hw-' + homeworkId.toString());

            console.log('parent', parent);
            console.log('child', child);

            if (parent && child) {
                parent.removeChild(child);
            }
        }

    }, [isCompleted]);

    return (
        <Card className='given-homework-card' id={'hw-' + homeworkId.toString()}>
            <CardContent>
                <Typography variant="h6">
                    {subjectName}
                </Typography>
                <Typography variant="body1">{new Date(createdAt).toUTCString()}</Typography>
                <Grid container justifyContent='space-between'>
                    <Button
                        variant="contained"
                        color="primary"
                        startIcon={<DownloadIcon/>}
                        onClick={handleDownloadClick}
                    >
                        Download
                    </Button>
                    {
                        isFileDownloading &&
                        <Box sx={{display: 'flex'}}>
                            <CircularProgress/>
                        </Box>
                    }
                </Grid>
                <TextField
                    id="comment"
                    label="Comment"
                    multiline
                    rows={4}
                    variant="outlined"
                    value={comment}
                    onChange={handleCommentChange}
                    fullWidth
                    margin="normal"
                />
                <input
                    accept="image/*, application/pdf"
                    style={{display: 'none'}}
                    id={"file-upload-" + homeworkId.toString()}
                    type="file"
                    onChange={handleFileChange}
                />
                <label htmlFor={"file-upload-" + homeworkId.toString()}>
                    <IconButton
                        color="primary"
                        aria-label="upload file"
                        component="span"
                    >
                        <FileUploadIcon/>
                    </IconButton>
                    <span>{file?.name}</span>
                </label>
                <Grid container={true} justifyContent='space-between'>
                    <Button variant="contained" color="primary" onClick={handleUploadClick}>
                        Send
                    </Button>
                    {
                        isFileUploading &&
                        <Box sx={{display: 'flex'}}>
                            <CircularProgress/>
                        </Box>
                    }

                </Grid>
            </CardContent>
        </Card>
    );
};