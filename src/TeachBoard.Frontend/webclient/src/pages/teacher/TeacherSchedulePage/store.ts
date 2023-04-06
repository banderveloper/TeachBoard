import {create} from "zustand";
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";
import {IServerResponse, ITeacherScheduleItem} from "../../../entities";

interface ITeacherScheduleStore{
    scheduleItems: ITeacherScheduleItem[],
    isLoading: boolean,
    loadSchedule: () => Promise<void>
}

export const useTeacherScheduleStore = create<ITeacherScheduleStore>(set => ({
    isLoading: false,
    scheduleItems: [],

    loadSchedule: async() => {
        set({isLoading: true});
        const apiLessons = await $api.get<IServerResponse<ITeacherScheduleItem[]>>(Endpoints.TEACHER.GET_SCHEDULE);

        set({scheduleItems: apiLessons.data.data});

        set({isLoading: false});
    }
}));