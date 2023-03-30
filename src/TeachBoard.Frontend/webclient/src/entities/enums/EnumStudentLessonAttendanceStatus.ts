interface IEnumStudentLessonAttendanceStatus {
    attended: string;
    late: string;
    absent: string;
}

export const EnumStudentLessonAttendanceStatus: IEnumStudentLessonAttendanceStatus = {
    attended: "Attended",
    late: "Late",
    absent: "Absent"
}

