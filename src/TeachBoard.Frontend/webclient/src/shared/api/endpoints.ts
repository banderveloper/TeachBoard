export const API_URL = 'https://teachboard.1gb.ua/api';

export const ENDPOINTS = {
    AUTH : {
        LOGIN: `${API_URL}/auth/login`,
        REFRESH: `${API_URL}/auth/refresh`,
        LOGOUT: `${API_URL}/auth/logout`
    },
    STUDENT: {
        GET_UNCOMPLETED_HOMEWORKS: `${API_URL}/student/uncompleted-homeworks`,
        GET_HOMEWORK_TASK_FILE: `${API_URL}/student/homework-task-file/`,
        UPLOAD_HOMEWORK_FILE: `${API_URL}/student/complete-homework`,
        GET_SCHEDULE: `${API_URL}/student/all-lessons`,
        GET_LESSON_ACTIVITIES: `${API_URL}/student/lessons-activities`,
        APPROVE_PENDING: `${API_URL}/student/approve-pending`,
        GET_PROFILE_DATA: `${API_URL}/student/profile-data`,
        GET_EXAMINATION_ACTIVITIES: `${API_URL}/student/exam-activities`
    },
    TEACHER: {
        GET_SCHEDULE: `${API_URL}/teacher/future-lessons`,
        GET_HOMEWORK_TASK_FILE: `${API_URL}/teacher/homework-task-file/`,
        GET_HOMEWORK_SOLUTION_FILE: `${API_URL}/teacher/homework-solution-file/`,
        GET_UNCHECKED_HOMEWORKS: `${API_URL}/teacher/unchecked-homeworks`,
        CHECK_HOMEWORK: `${API_URL}/teacher/check-homework`,
        GET_CURRENT_LESSON: `${API_URL}/teacher/current-lesson`,
        SET_STUDENT_LESSON_ACTIVITY: `${API_URL}/teacher/student-lesson-activity`,
        UPDATE_LESSON_TOPIC: `${API_URL}/teacher/lesson-topic`,
        GET_SUBJECTS_GROUPS: `${API_URL}/teacher/groups-subjects`,
        CREATE_HOMEWORK: `${API_URL}/teacher/homework`
    },
    ADMINISTRATOR: {
        CREATE_PENDING_USER: `${API_URL}/administrator/pending-user`,
        CREATE_LESSON: `${API_URL}/administrator/lesson`,
        GET_CREATE_LESSON_SELECT_DATA: `${API_URL}/administrator/groups-teachers-subjects`,
        GET_UNCHECKED_HOMEWORKS_COUNT: `${API_URL}/administrator/teachers-unchecked-homeworks-count`,
        CREATE_EXAMINATION: `${API_URL}/administrator/examination`,
        GET_USERS_BY_PARTIAL_NAME: `${API_URL}/administrator/users-presentations/`,
        GET_MEMBER_DATA: `${API_URL}/administrator/memberData/`,
        UPDATE_USER_PUBLIC_DATA: `${API_URL}/administrator/user-public`,
        UPDATE_USER_AVATAR: `${API_URL}/administrator/user-avatar/`,
        GET_ALL_GROUPS: `${API_URL}/administrator/groups`,
        UPDATE_STUDENT_GROUP: `${API_URL}/administrator/student-group`
    }
}

export default ENDPOINTS;
