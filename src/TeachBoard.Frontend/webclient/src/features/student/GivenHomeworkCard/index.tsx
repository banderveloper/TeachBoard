import * as React from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import {IGivenHomework, IServerResponse} from "../../../entities";
import fileDownload from 'js-file-download'
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";

const bull = (
    <Box
        component="span"
        sx={{display: 'inline-block', mx: '2px', transform: 'scale(0.8)'}}
    >
        •
    </Box>
);

export const GivenHomeworkCard = (homework: IGivenHomework) => {

    const handleDownloadClick: React.MouseEventHandler<HTMLButtonElement> = async (event) => {
        event.preventDefault();

        try {
            const response = await $api.get<Blob>(Endpoints.STUDENT.GET_HOMEWORK_TASK_FILE + homework.homeworkId,
                {
                    responseType: "blob",
                }
            );

            const header = response.headers['content-disposition'];
            const filename = header.split(';')[1].trim().split('=')[1];
            fileDownload(response.data as Blob, filename);

        } catch (error) {
            console.error("Error downloading file: ", error);
        }
    }

    return (
        <Card>
            <CardContent>
                <Typography variant="h5" component="h2">
                    {homework.subjectName}
                </Typography>
                <Typography variant="body2" component="p">
                    Created on: {new Date(homework.createdAt).toLocaleTimeString()}
                </Typography>
                <Button onClick={handleDownloadClick}>Download</Button>
            </CardContent>
        </Card>
    );
};
