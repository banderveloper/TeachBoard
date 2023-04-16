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
export type {ICreatePendingUserRequestModel} from './requests/administrator/ICreatePendingUserRequestModel'

////////////////////////////////////
//////////// RESPONSES /////////////
////////////////////////////////////

export type {IServerResponse} from './responses/IServerResponse';
export type {ILoginResponse} from './responses/auth/ILoginResponse';
export type {IStudentProfileData} from './responses/student/IStudentProfileData'
export type {ITeacherCurrentLessonFullInfo} from './responses/teacher/ITeacherCurrentLessonFullInfo'
export type {ICreatePendingUserResponse} from './responses/administrator/ICreatePendingUserResponse'

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
export type {IStudentCurrentLessonActivityItem} from './models/teacher/IStudentCurrentLessonActivityItem'
export type {ITeacherCurrentLesson} from './models/teacher/ITeacherCurrentLesson'
export type {IGroup} from './models/administrator/IGroup'
export type {ISubject} from './models/administrator/ISubject'
export type {ITeacherPresentation} from './models/administrator/ITeacherPresentation'
export type {ICreateLessonRequest} from './requests/administrator/ICreateLessonRequest'