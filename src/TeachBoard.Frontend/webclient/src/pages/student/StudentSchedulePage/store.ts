import {IScheduleItem, IServerResponse} from "../../../entities";
import {create} from "zustand";
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";

interface IStudentScheduleStore{
    scheduleItems: IScheduleItem[],
    isLoading: boolean,
    loadSchedule: () => Promise<void>
}

export const useScheduleStore = create<IStudentScheduleStore>(set => ({
    isLoading: false,
    scheduleItems: [],

    loadSchedule: async() => {
        set({isLoading: true});
        const apiHomeworks = await $api.get<IServerResponse<IScheduleItem[]>>(Endpoints.STUDENT.GET_SCHEDULE);

        set({scheduleItems: apiHomeworks.data.data});

        set({isLoading: false});
    }
}));