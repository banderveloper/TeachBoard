import React from 'react';
import {Route, Routes} from "react-router-dom";
import {
    LoginPage,
    ProfilePage,
    StudentExaminationActivitiesPage,
    StudentLessonActivitiesPage,
    StudentSchedulePage
} from "../../pages";
import {PrivateRoute} from "./PrivateRoute";
import {EnumUserRole} from "../../entities";
import {StudentHomePage} from "../../pages";
import {StudentHomeworksPage} from "../../pages";
import {RegisterPage} from "../../pages";
import {LogoutPage} from "../../pages";

export const Routing = () => {
    return (
        <Routes>
            <Route path="/" element={<LoginPage/>}/>
            <Route path="login" element={<LoginPage/>}/>
            <Route path="register" element={<RegisterPage/>}/>
            <Route path="logout" element={<LogoutPage/>}/>
            <Route path="profile" element={<ProfilePage/>}/>

            <Route path="student" element={
                <PrivateRoute requiredRole={EnumUserRole.student}>
                    <StudentHomePage/>
                </PrivateRoute>
            }/>

            <Route path="student/homeworks" element={
                <PrivateRoute requiredRole={EnumUserRole.student}>
                    <StudentHomeworksPage/>
                </PrivateRoute>
            }/>

            <Route path="student/lessons" element={
                <PrivateRoute requiredRole={EnumUserRole.student}>
                    <StudentSchedulePage/>
                </PrivateRoute>
            }/>

            <Route path="student/activity" element={
                <PrivateRoute requiredRole={EnumUserRole.student}>
                    <StudentLessonActivitiesPage/>
                </PrivateRoute>
            }/>

            <Route path="student/examinations" element={
                <PrivateRoute requiredRole={EnumUserRole.student}>
                    <StudentExaminationActivitiesPage/>
                </PrivateRoute>
            }/>
        </Routes>
    );
};