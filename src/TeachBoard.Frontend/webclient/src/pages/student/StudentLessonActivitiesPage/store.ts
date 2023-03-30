import {IServerResponse, IStudentLessonActivityItem} from "../../../entities";
import {create} from "zustand";
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";

interface ILessonActivitiesStore {

    lessonActivities: IStudentLessonActivityItem[],
    isLoading: boolean,
    loadLessonActivities: () => Promise<void>

}

export const useLessonActivitiesStore = create<ILessonActivitiesStore>(set => ({
    lessonActivities: [],
    isLoading: false,

    loadLessonActivities: async () => {
        set({isLoading: true});
        const apiHomeworks = await $api.get<IServerResponse<IStudentLessonActivityItem[]>>(Endpoints.STUDENT.GET_LESSON_ACTIVITIES);

        set({lessonActivities: apiHomeworks.data.data});

        set({isLoading: false});
    }
}));