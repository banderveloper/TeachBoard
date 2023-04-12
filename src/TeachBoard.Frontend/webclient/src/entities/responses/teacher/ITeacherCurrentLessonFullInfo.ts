import {ITeacherCurrentLesson} from "../../models/teacher/ITeacherCurrentLesson";
import {IStudentCurrentLessonActivityItem} from "../../models/teacher/IStudentCurrentLessonActivityItem";

export interface ITeacherCurrentLessonFullInfo{
    lesson: ITeacherCurrentLesson;
    students: IStudentCurrentLessonActivityItem[];
}