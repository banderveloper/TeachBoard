import {
    Avatar,
    Box,
    Button,
    Checkbox, CircularProgress,
    Container,
    FormControlLabel, Grid,
    TextField,
    Typography
} from "@mui/material";
import React, {useEffect} from "react";
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import {useAuthStore} from "../../../entities";

export function LoginForm() {

    const {login, resetErrorInfo, isLoading, errorMessage} = useAuthStore();

    useEffect(() => {
        resetErrorInfo();
    }, []);

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);

        if (data.get('userName') && data.get('password')) {
            const userNameStr = data.get('userName')!.toString();
            const passwordStr = data.get('password')!.toString();

            login({userName: userNameStr, password: passwordStr});
        }
    };


    return (
        <Container component="main" maxWidth="xs">
            <Box
                sx={{
                    marginTop: 8,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}
            >
                <Avatar sx={{m: 1, bgcolor: 'secondary.main'}}>
                    <LockOutlinedIcon/>
                </Avatar>
                <Typography component="h1" variant="h5">
                    Sign in
                </Typography>
                <Box component="form" onSubmit={handleSubmit} noValidate sx={{mt: 1}}>
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        id="userName"
                        label="Username"
                        name="userName"
                        autoFocus
                    />
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        name="password"
                        label="Password"
                        type="password"
                        id="password"
                        autoComplete="current-password"
                    />
                    {errorMessage && (
                        <Typography variant="body2" color="error">
                            {errorMessage}
                        </Typography>
                    )}
                    <FormControlLabel
                        control={<Checkbox value="remember" color="primary"/>}
                        label="Remember me"
                    />
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{mt: 3, mb: 2}}
                    >
                        Sign In
                    </Button>
                    {
                        isLoading &&
                        <Box sx={{display: 'flex', justifyContent: 'center'}}>
                            <CircularProgress/>
                        </Box>
                    }
                </Box>
            </Box>
        </Container>
    );
}