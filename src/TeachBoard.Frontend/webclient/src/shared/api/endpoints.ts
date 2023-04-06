export const API_URL = 'http://localhost:5000/api';

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
        GET_SCHEDULE: `${API_URL}/teacher/future-lessons`
    }
}

export default ENDPOINTS;
