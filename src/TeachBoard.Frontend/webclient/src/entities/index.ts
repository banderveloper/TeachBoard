////////////////////////////////////
//////////// ENUMERABLE ////////////
////////////////////////////////////

export {EnumUserRole} from './enums/EnumUserRole'
export {EnumStudentLessonAttendanceStatus} from './enums/EnumStudentLessonAttendanceStatus'

////////////////////////////////////
//////////// REQUESTS //////////////
////////////////////////////////////

export type {ILoginRequest} from './requests/auth/ILoginRequest';
export type {IApprovePendingStudentRequest} from './requests/auth/IApprovePendingStudentRequest'
export type {ICheckHomework} from './requests/teacher/ICheckHomework'

////////////////////////////////////
//////////// RESPONSES /////////////
////////////////////////////////////

export type {IServerResponse} from './responses/IServerResponse';
export type {ILoginResponse} from './responses/auth/ILoginResponse';
export type {IStudentProfileData} from './responses/student/IStudentProfileData'

////////////////////////////////////
//////////// SHARED STORES /////////////
////////////////////////////////////

export {useAuthStore} from './sharedStores/useAuthStore';

////////////////////////////////////
//////////// MODELS /////////////
////////////////////////////////////

export type {ITeacherScheduleItem} from './models/teacher/ITeacherScheduleItem'
export type {ITeacherUncheckedHomework} from './models/teacher/ITeacherUncheckedHomework'
export type {IGivenHomework} from './models/student/IGivenHomework';
export type {IScheduleItem} from './models/student/IScheduleItem';
export type {IStudentLessonActivityItem} from './models/student/IStudentLessonActivityItem'
export type {IStudentExaminationActivityItem} from './models/student/IStudentExaminationActivityItem'