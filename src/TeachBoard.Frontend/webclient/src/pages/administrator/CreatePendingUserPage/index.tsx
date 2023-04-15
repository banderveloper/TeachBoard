import React, {useState} from 'react';
import {
    Box,
    Button, CircularProgress,
    FormControl,
    Grid,
    InputLabel,
    MenuItem,
    Modal,
    Select,
    SelectChangeEvent,
    TextField
} from "@mui/material";
import {ICreatePendingUserRequestModel, ICreatePendingUserResponse, IServerResponse} from "../../../entities";
import './index.css'
import {$api} from "../../../shared";
import ENDPOINTS from "../../../shared/api/endpoints";

type Role = "Student" | "Teacher";

export const CreatePendingUserPage = () => {

    const [user, setUser] = useState<ICreatePendingUserRequestModel>({
        role: "",
        firstName: "",
        lastName: "",
        patronymic: "",
        phoneNumber: "",
        homeAddress: "",
        email: "",
        dateOfBirth: "",
    });
    const [error, setError] = useState<string | null>(null);
    const [registerCode, setRegisterCode] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const handleRoleChange = (e: SelectChangeEvent) => {
        setUser({
            ...user,
            [e.target.name]: e.target.value,
        });
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setUser({
            ...user,
            [e.target.name]: e.target.value,
        });
    };

    const handleDateChange = (date: string | null) => {
        setUser({
            ...user,
            dateOfBirth: date,
        });
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {

        e.preventDefault();

        setIsLoading(true);

        const response = await $api.post<IServerResponse<ICreatePendingUserResponse>>(ENDPOINTS.ADMINISTRATOR.CREATE_PENDING_USER, user);

        if (response.data.error) {
            setError(response.data.error.errorCode);
        } else {
            const registerCode = response.data.data!.registerCode;

            console.log(registerCode);
            setRegisterCode(registerCode);
        }

        setIsLoading(false);
    };

    return (
        <form onSubmit={handleSubmit}>
            <Grid container spacing={2} className='create-pending-block'>
                <Grid item xs={6}>
                    <FormControl fullWidth className='create-pending-block-input'>
                        <InputLabel id="role-label">Role</InputLabel>
                        <Select
                            labelId="role-label"
                            id="role"
                            name="role"
                            value={user.role}
                            onChange={handleRoleChange}
                        >
                            <MenuItem value="Student">Student</MenuItem>
                            <MenuItem value="Teacher">Teacher</MenuItem>
                        </Select>
                    </FormControl>
                    <TextField
                        fullWidth
                        label="First Name"
                        name="firstName"
                        value={user.firstName}
                        onChange={handleInputChange}
                        className='create-pending-block-input'
                    />
                    <TextField
                        fullWidth
                        label="Last Name"
                        name="lastName"
                        value={user.lastName}
                        onChange={handleInputChange}
                        className='create-pending-block-input'
                    />
                    <TextField
                        fullWidth
                        label="Patronymic"
                        name="patronymic"
                        value={user.patronymic}
                        onChange={handleInputChange}
                        className='create-pending-block-input'
                    />
                </Grid>
                <Grid item xs={6}>
                    <TextField
                        fullWidth
                        label="Phone Number"
                        name="phoneNumber"
                        value={user.phoneNumber}
                        onChange={handleInputChange}
                        className='create-pending-block-input'
                    />
                    <TextField
                        fullWidth
                        label="Home Address"
                        name="homeAddress"
                        value={user.homeAddress}
                        onChange={handleInputChange}
                        className='create-pending-block-input'
                    />
                    <TextField
                        fullWidth
                        label="Email"
                        name="email"
                        value={user.email}
                        onChange={handleInputChange}
                        className='create-pending-block-input'
                    />
                    <TextField
                        fullWidth
                        label="BirthDate"
                        name="dateOfBirth"
                        value={user.dateOfBirth}
                        onChange={handleInputChange}
                        className='create-pending-block-input'
                    />
                </Grid>
                <Grid item xs={12} className='create-pending-send-container'>
                    <Button variant="contained" color="primary" type="submit">
                        Create User
                    </Button>
                    {
                        isLoading ?
                            <Box sx={{display: 'flex', margin: '25px'}}>
                                <CircularProgress/>
                            </Box>
                            :
                            <span className='create-pending-send-label'>
                                {
                                    error ?? registerCode
                                }
                            </span>
                    }
                </Grid>
            </Grid>
        </form>
    );
};
