import React, {useEffect} from 'react';
import {
    Avatar,
    Box,
    Button,
    Checkbox,
    CircularProgress,
    Container,
    FormControlLabel,
    TextField,
    Typography
} from "@mui/material";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import {useAuthStore} from "../../../entities";

export const RegisterForm = () => {

    const {register, resetErrorInfo, isLoading, errorMessage} = useAuthStore();

    useEffect(() => {
        resetErrorInfo();
    }, []);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);

        if (data.get('userName') && data.get('passwordHash') && data.get('registerCode')) {
            const userNameStr = data.get('userName')!.toString();
            const passwordStr = data.get('passwordHash')!.toString();
            const registerCodeStr = data.get('registerCode')!.toString();

            await register({registerCode: registerCodeStr, passwordHash: passwordStr, userName: userNameStr});

            if(errorMessage?.length == 0)
                window.location.href = '/login';
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
                    Approve account
                </Typography>
                <Box component="form" onSubmit={handleSubmit} noValidate sx={{mt: 1}}>
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        name="registerCode"
                        label="Register code"
                        type="text"
                        id="registerCode"
                        autoComplete="current-password"
                    />
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
                        name="passwordHash"
                        label="Password"
                        type="password"
                        id="passwordHash"
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
};
