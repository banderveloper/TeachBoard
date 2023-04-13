import {create} from "zustand";
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";
import {IServerResponse, ITeacherCurrentLessonFullInfo} from "../../../entities";

interface ICurrentLessonStore{
    current: ITeacherCurrentLessonFullInfo | null,
    isLoading: boolean,
    loadCurrentLesson: () => Promise<void>,
    sendUpdatedTopic: (topic: string) => Promise<void>
}

export const useCurrentLessonStore = create<ICurrentLessonStore>((set, get) => ({
    isLoading: false,
    current: null,

    loadCurrentLesson: async() => {
        set({isLoading: true});
        const apiCurrent = await $api.get<IServerResponse<ITeacherCurrentLessonFullInfo>>(Endpoints.TEACHER.GET_CURRENT_LESSON);

        set({current: apiCurrent.data.data});

        set({isLoading: false});
    },

    sendUpdatedTopic: async(topic) => {

        const {current} = get();

        const request = {
            lessonId: current?.lesson.id,
            topic: topic
        };

        await $api.put(Endpoints.TEACHER.UPDATE_LESSON_TOPIC, request);
    }
}));