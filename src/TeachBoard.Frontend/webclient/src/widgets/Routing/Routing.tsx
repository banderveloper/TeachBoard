import React from 'react';
import {Route, Routes} from "react-router-dom";
import {LoginPage, StudentLessonActivitiesPage, StudentSchedulePage} from "../../pages";
import {PrivateRoute} from "./PrivateRoute";
import {EnumUserRole} from "../../entities";
import {StudentHomePage} from "../../pages";
import {StudentHomeworksPage} from "../../pages";

export const Routing = () => {
    return (
        <Routes>
            <Route path="/" element={<LoginPage/>}/>
            <Route path="login" element={<LoginPage/>}/>

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
        </Routes>
    );
};