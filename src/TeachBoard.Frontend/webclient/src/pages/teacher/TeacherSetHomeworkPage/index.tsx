import React, {useEffect} from 'react';
import {
    Box,
    Button,
    CircularProgress,
    FormControl,
    Grid,
    IconButton,
    InputLabel,
    MenuItem,
    Select,
    TextField
} from "@mui/material";
import {DateTimePicker} from "@mui/x-date-pickers";
import {useTeacherSetHomeworkStore} from "./store";
import FileUploadIcon from "@mui/icons-material/FileUpload";

export const TeacherSetHomeworkPage = () => {

    const store = useTeacherSetHomeworkStore();

    useEffect(() => {
        store.loadSelectData();
    }, []);

    if (store.isSelectDataLoading)
        return <Box sx={{display: "flex", margin: "25px"}}>
            <CircularProgress/>
        </Box>

    return (
        <Grid container spacing={2} className="create-lesson-block">
            <Grid item xs={12}>
                <FormControl fullWidth className="create-lesson-block-input">
                    <InputLabel variant='standard' id="subject-label"
                                className='create-lesson-label'>Subject</InputLabel>
                    <Select
                        labelId="subject-label"
                        id="subject"
                        name="subject"
                        value={store.selectedSubjectId}
                        onChange={(e) => store.setSubjectId(e.target.value as number)}
                    >
                        {store.selectData.subjects.map((subject) => (
                            <MenuItem key={subject.id} value={subject.id}>
                                {subject.name}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <FormControl fullWidth className="create-lesson-block-input">
                    <InputLabel variant='standard' id="group-label" className='create-lesson-label'>Group
                        Name</InputLabel>
                    <Select
                        labelId="group-label"
                        id="group"
                        name="group"
                        value={store.selectedGroupId}
                        onChange={(e) => store.setGroupId(e.target.value as number)}
                    >
                        {store.selectData.groups.map((group) => (
                            <MenuItem key={group.id} value={group.id}>
                                {group.name}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <input
                    accept="image/*, application/pdf"
                    style={{display: 'none'}}
                    id={'create-hw'}
                    type="file"
                    onChange={(e) => store.setSelectedFile(e.target.files![0])}
                />
                <label htmlFor={"create-hw"}>
                    <IconButton
                        color="primary"
                        aria-label="upload file"
                        component="span"
                    >
                        <FileUploadIcon/>
                    </IconButton>
                    <span>{store.selectedFile?.name}</span>
                </label>
                <Grid item xs={12} className="create-lesson-send-container">
                    <Button variant="contained" color="primary" onClick={store.loadHomework}>
                        Create homework
                    </Button>
                    {store.isHomeworkUploading &&
                        <Box sx={{display: "flex", margin: "25px"}}>
                            <CircularProgress/>
                        </Box>
                    }
                </Grid>
            </Grid>
        </Grid>
    );
}
