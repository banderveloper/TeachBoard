import React, {useEffect} from 'react';
import {useUserEditStore} from "./store";
import {
    Avatar,
    Box,
    Button,
    CircularProgress, FormControl,
    Grid, InputLabel, MenuItem, Select,
    TextField, Typography
} from "@mui/material";
import './index.css'

export const UserEditPage = () => {

    const urlLast = new URL(window.location.href).pathname.split('/').pop();
    const userId = parseInt(urlLast!);

    const store = useUserEditStore();

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            store.setSelectedAvatar(event.target.files[0]);
            store.sendNewAvatar();
        }
    };

    useEffect(() => {
        store.setUserId(userId);
        store.loadUserData();
    }, []);

    if (store.isLoading)
        return <Box sx={{display: 'flex', margin: '25px'}}>
            <CircularProgress/>
        </Box>

    else if (store.error)
        return <div className='user-edit-avatar-block'>
            <Typography variant='h4' color='red'>{store.error}</Typography>
        </div>

    return (
        <div>
            <div className='user-edit-avatar-block'>
                <input
                    accept="image/*"
                    style={{display: 'none'}}
                    id={'avatar-file-input'}
                    type="file"
                    onChange={handleFileChange}
                />
                <label htmlFor='avatar-file-input'>
                    <Avatar
                        alt="Avatar"
                        src={store.user?.avatarImagePath ?? 'R'}
                        sx={{width: 100, height: 100}}
                        className='user-edit-avatar'
                    />
                </label>
            </div>

            <Grid container spacing={2} className='create-pending-block'>
                <Grid item xs={6}>
                    <TextField
                        fullWidth
                        label="Username"
                        name="userName"
                        value={store.user?.userName}
                        className='create-pending-block-input'
                        disabled={true}
                    />
                    <TextField
                        fullWidth
                        label="First Name"
                        name="firstName"
                        value={store.user?.firstName}
                        onChange={(e) => {
                            store.setUser({...store.user!, firstName: e.target.value});
                            console.log(store.user);
                        }}
                        className='create-pending-block-input'
                    />
                    <TextField
                        fullWidth
                        label="Last Name"
                        name="lastName"
                        value={store.user?.lastName}
                        onChange={(e) => {
                            store.setUser({...store.user!, lastName: e.target.value});
                            console.log(store.user);
                        }}
                        className='create-pending-block-input'
                    />
                    <TextField
                        fullWidth
                        label="Patronymic"
                        name="patronymic"
                        value={store.user?.patronymic}
                        onChange={(e) => {
                            store.setUser({...store.user!, patronymic: e.target.value});
                            console.log(store.user);
                        }}
                        className='create-pending-block-input'
                    />
                </Grid>
                <Grid item xs={6}>
                    <TextField
                        fullWidth
                        label="Phone Number"
                        name="phoneNumber"
                        value={store.user?.phoneNumber}
                        onChange={(e) => {
                            store.setUser({...store.user!, phoneNumber: e.target.value});
                            console.log(store.user);
                        }}
                        className='create-pending-block-input'
                    />
                    <TextField
                        fullWidth
                        label="Home Address"
                        name="homeAddress"
                        value={store.user?.homeAddress}
                        onChange={(e) => {
                            store.setUser({...store.user!, homeAddress: e.target.value});
                            console.log(store.user);
                        }}
                        className='create-pending-block-input'
                    />
                    <TextField
                        fullWidth
                        label="Email"
                        name="email"
                        value={store.user?.email}
                        onChange={(e) => {
                            store.setUser({...store.user!, email: e.target.value});
                            console.log(store.user);
                        }}
                        className='create-pending-block-input'
                    />
                    {
                        store.userAsStudent && store.userAsStudent.studentId &&
                        <FormControl fullWidth className="create-lesson-block-input">
                            <InputLabel variant='standard'  id="group-label" className='create-lesson-label'>Group Name</InputLabel>
                            <Select
                                labelId="group-label"
                                id="group"
                                name="group"
                                value={store.selectedGroupId}
                                onChange={(e) => store.setSelectedGroupId(e.target.value as number)}
                            >
                                {store.groups.map((group) => (
                                    <MenuItem key={group.id} value={group.id}>
                                        {group.name}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    }
                </Grid>
                <Grid item xs={12} className='create-pending-send-container'>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={store.sendUpdatePresentationRequest}
                        disabled={store.isSendingData}
                    >
                        Update user
                    </Button>
                    {
                        store.isSendingData &&
                        <Box sx={{display: 'flex', margin: '25px'}}>
                            <CircularProgress/>
                        </Box>
                    }
                </Grid>
            </Grid>
        </div>
    );
};
