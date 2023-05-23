import {Avatar, Box, Button, Grid, List, ListItem, ListItemAvatar, ListItemText, TextField} from '@mui/material';
import React from 'react';
import './index.css'
import {useUsersSearchStore} from "./store";
import {Link} from "react-router-dom";

const centerBoxStyles = {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    marginTop: 100
};


export const UsersSearchPage = () => {

    const store = useUsersSearchStore();

    return (
        <Box style={centerBoxStyles}>
            <Box>
                <Grid container spacing={2} alignItems="center">
                    <Grid item xs={12}>
                        <TextField
                            label="User name"
                            variant="outlined"
                            fullWidth
                            value={store.searchQuery}
                            onChange={(event) => store.setSearchQuery(event.target.value)}
                        />
                    </Grid>
                    <Grid item>
                        <Button variant="contained" color="primary" onClick={store.loadSearchedUsers}>
                            Search
                        </Button>
                    </Grid>
                </Grid>
                <List>
                    {store.searchedUsers.map((item) => (
                        <ListItem button component={Link} to={'../administrator/user/' + item.id} key={item.id}>
                            <ListItemAvatar>
                                <Avatar src={item.avatarImagePath!}/>
                            </ListItemAvatar>
                            <ListItemText primary={[item.lastName, item.firstName, item.patronymic].join(' ')}/>
                        </ListItem>
                    ))}
                </List>
            </Box>
        </Box>
    );
};
