import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Menu from "@mui/material/Menu";
import MenuIcon from "@mui/icons-material/Menu";
import Container from "@mui/material/Container";
import Avatar from "@mui/material/Avatar";
import Button from "@mui/material/Button";
import Tooltip from "@mui/material/Tooltip";
import MenuItem from "@mui/material/MenuItem";
import {Link} from "react-router-dom";
import {useEffect, useState} from "react";
import {EnumUserRole, useAuthStore} from "../../entities";
import './Navbar.module.scss'

interface INavItem {
    pathName: string;
    path: string;
}

export function Navbar() {

    const {role, isLoggedIn} = useAuthStore();

    const [mainNavItems, setMainNavItems] = useState<INavItem[]>([]);
    const [profileNavItems, setProfileNavItems] = useState<INavItem[]>([
        {pathName: 'Profile', path: '/profile'},
        {pathName: 'Logout', path: '/logout'}
    ]);

    const updateRoleNavbarItems = () => {
        switch (role) {
            case EnumUserRole.student:
                setMainNavItems([
                    {path: '/student/homeworks', pathName: 'Homeworks'},
                    {path: '/student/lessons', pathName: 'Schedule'},
                    {path: '/student/activity', pathName: 'Activity'},
                    {path: '/student/examinations', pathName: 'Examinations'}
                ])
                break;
            case EnumUserRole.teacher:
                setMainNavItems([
                    {path: '/teacher/current-lesson', pathName: 'Lesson'},
                    {path: '/teacher/lessons', pathName: 'Schedule'},
                    {path: '/teacher/homeworks', pathName: 'Homeworks'},
                    {path: '/teacher/set-homework', pathName: 'Set homework'}
                ])
                break;
            case EnumUserRole.administrator:
                setMainNavItems([
                    {path: '/administrator/create-pending', pathName: 'Create user'},
                    {path: '/administrator/create-lesson', pathName: 'Create lesson'},
                    {path: '/administrator/create-examination', pathName: 'Create examination'},
                    {path: '/administrator/users', pathName: 'Users'},
                    {path: '/administrator/unchecked-homeworks', pathName: 'Unchecked homeworks'}
                ])
                break;
            default:
                setMainNavItems([
                    {path: '/login', pathName: 'Login'},
                    {path: '/register', pathName: 'Register'},
                ])
                break;

        }
    }

    useEffect(() => {
        updateRoleNavbarItems();
    }, []);

    useEffect(() => {
        updateRoleNavbarItems();
    }, [role, isLoggedIn]);

    const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElNav(event.currentTarget);
    };
    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElUser(event.currentTarget);
    };

    const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(
        null
    );
    const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(
        null
    );

    const handleCloseNavMenu = () => {
        setAnchorElNav(null);
    };

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
    };

    return (
        <AppBar position="static">
            <Container maxWidth="xl">
                <Toolbar disableGutters>
                    <Typography
                        className='logo'
                        variant="h6"
                        noWrap
                        sx={{
                            color: 'white',
                            mr: 2,
                            display: {xs: "none", md: "flex"},
                            fontFamily: "monospace",
                            fontWeight: 700,
                            letterSpacing: ".3rem",
                            textDecoration: "none",
                        }}
                    >
                        <Link to='/' style={{color: 'white'}}>TEACHBOARD</Link>
                    </Typography>

                    <Box sx={{flexGrow: 1, display: {xs: "flex", md: "none"}}}>
                        <IconButton
                            size="large"
                            aria-label="account of current user"
                            aria-controls="menu-appbar"
                            aria-haspopup="true"
                            onClick={handleOpenNavMenu}
                            color="inherit"
                        >
                            <MenuIcon/>
                        </IconButton>
                        <Menu
                            id="menu-appbar"
                            anchorEl={anchorElNav}
                            anchorOrigin={{
                                vertical: "bottom",
                                horizontal: "left",
                            }}
                            keepMounted
                            transformOrigin={{
                                vertical: "top",
                                horizontal: "left",
                            }}
                            open={Boolean(anchorElNav)}
                            onClose={handleCloseNavMenu}
                            sx={{
                                display: {xs: "block", md: "none"},
                            }}
                        >
                            {mainNavItems.map((item) => (
                                <Link to={item.path} key={item.pathName}>
                                    <MenuItem onClick={handleCloseNavMenu}>
                                        <Typography textAlign="center">{item.pathName}</Typography>
                                    </MenuItem>
                                </Link>
                            ))}
                        </Menu>
                    </Box>
                    <Typography
                        className='logo'
                        variant="h5"
                        noWrap
                        component="a"
                        href=""
                        sx={{
                            mr: 2,
                            display: {xs: "flex", md: "none"},
                            flexGrow: 1,
                            fontFamily: "monospace",
                            fontWeight: 700,
                            letterSpacing: ".3rem",
                            color: "inherit",
                            textDecoration: "none",
                        }}
                    >
                        TEACHBOARD
                    </Typography>
                    <Box sx={{flexGrow: 1, display: {xs: "none", md: "flex"}}}>
                        {mainNavItems.map((item) => (
                            <Link to={item.path} key={item.pathName}>
                                <Button
                                    key={item.pathName}
                                    onClick={handleCloseNavMenu}
                                    sx={{my: 2, color: "white", display: "block"}}
                                >
                                    {item.pathName}
                                </Button>
                            </Link>
                        ))}
                    </Box>

                    {
                        isLoggedIn &&
                        <Box sx={{flexGrow: 0}}>
                            <Tooltip title="Open settings">
                                <IconButton onClick={handleOpenUserMenu} sx={{p: 0}}>
                                    <Avatar alt="Remy Sharp" src="/static/images/avatar/2.jpg"/>
                                </IconButton>
                            </Tooltip>
                            <Menu
                                sx={{mt: "45px"}}
                                id="menu-appbar"
                                anchorEl={anchorElUser}
                                anchorOrigin={{
                                    vertical: "top",
                                    horizontal: "right",
                                }}
                                keepMounted
                                transformOrigin={{
                                    vertical: "top",
                                    horizontal: "right",
                                }}
                                open={Boolean(anchorElUser)}
                                onClose={handleCloseUserMenu}
                            >
                                {profileNavItems.map((item) => (
                                    <MenuItem key={item.pathName} onClick={handleCloseUserMenu}>
                                        <Link to={item.path} key={item.path}>
                                            {item.pathName}
                                        </Link>
                                    </MenuItem>
                                ))}
                            </Menu>
                        </Box>
                    }

                </Toolbar>
            </Container>
        </AppBar>
    );
}