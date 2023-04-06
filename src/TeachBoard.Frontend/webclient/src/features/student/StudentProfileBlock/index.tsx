import React from 'react';
import './index.css'
import {Avatar, Grid, Typography} from "@mui/material";
import {IStudentProfileData} from "../../../entities";


export const StudentProfileBlock: React.FC<IStudentProfileData> = (data) => {

    const user = data.user;
    const group = data.group;

    return (
        <div className="root">
            <Grid container direction="column" alignItems="center">
                <Avatar alt="Profile Picture" src={user.avatarImagePath} className="avatar" />
                <h4 className="name"> {user.lastName} {user.firstName} {user.patronymic}</h4>
                <h5 className="groupName">{group.name}</h5>
                <h6 className="info"><strong>Username:</strong> {user.userName}</h6>
                <h6 className="info"><strong>Role:</strong> {user.role}</h6>
                <h6 className="info"><strong>Phone Number:</strong> {user.phoneNumber}</h6>
                <h6 className="info"><strong>Home Address:</strong> {user.homeAddress}</h6>
                <h6 className="info"><strong>Email:</strong> {user.email}</h6>
                <h6 className="info"><strong>Date of Birth:</strong> {new Date(user.dateOfBirth!).toLocaleDateString()}</h6>
            </Grid>
        </div>
    );
}
