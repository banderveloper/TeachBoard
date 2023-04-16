import React from 'react';
import {Route, Routes} from "react-router-dom";
import {
    AdministratorHomePage, CreateExaminationPage,
    CreateLessonPage,
    CreatePendingUserPage,
    LoginPage,
    ProfilePage,
    StudentExaminationActivitiesPage,
    StudentLessonActivitiesPage,
    StudentSchedulePage,
    TeacherCurrentLessonPage,
    TeacherHomePage,
    TeacherHomeworksPage,
    TeacherSchedulePage,
    UncheckedHomeworksCountPage
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


            <Route path="teacher" element={
                <PrivateRoute requiredRole={EnumUserRole.teacher}>
                    <TeacherHomePage/>
                </PrivateRoute>
            }/>
            <Route path="teacher/lessons" element={
                <PrivateRoute requiredRole={EnumUserRole.teacher}>
                    <TeacherSchedulePage/>
                </PrivateRoute>
            }/>
            <Route path="teacher/homeworks" element={
                <PrivateRoute requiredRole={EnumUserRole.teacher}>
                    <TeacherHomeworksPage/>
                </PrivateRoute>
            }/>
            <Route path="teacher/current-lesson" element={
                <PrivateRoute requiredRole={EnumUserRole.teacher}>
                    <TeacherCurrentLessonPage/>
                </PrivateRoute>
            }/>


            <Route path="administrator" element={
                <PrivateRoute requiredRole={EnumUserRole.administrator}>
                    <AdministratorHomePage/>
                </PrivateRoute>
            }/>
            <Route path="administrator/create-pending" element={
                <PrivateRoute requiredRole={EnumUserRole.administrator}>
                    <CreatePendingUserPage/>
                </PrivateRoute>
            }/>
            <Route path="administrator/create-lesson" element={
                <PrivateRoute requiredRole={EnumUserRole.administrator}>
                    <CreateLessonPage/>
                </PrivateRoute>
            }/>
            <Route path="administrator/unchecked-homeworks" element={
                <PrivateRoute requiredRole={EnumUserRole.administrator}>
                    <UncheckedHomeworksCountPage/>
                </PrivateRoute>
            }/>
            <Route path="administrator/create-examination" element={
                <PrivateRoute requiredRole={EnumUserRole.administrator}>
                    <CreateExaminationPage/>
                </PrivateRoute>
            }/>

        </Routes>
    );
};