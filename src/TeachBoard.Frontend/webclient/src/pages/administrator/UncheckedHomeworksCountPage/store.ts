import {create} from "zustand";
import {IServerResponse, IUncheckedHomeworksCountItem} from "../../../entities";
import {$api} from "../../../shared";
import ENDPOINTS from "../../../shared/api/endpoints";

interface IUncheckedHomeworksCountStore {
    uncheckedHomeworksCountItems: IUncheckedHomeworksCountItem[];
    isLoading: boolean;
    loadUncheckedHomeworksCount: () => Promise<void>;
}

export const useUncheckedHomeworksCountStore = create<IUncheckedHomeworksCountStore>((set, get) => ({

    isLoading: false,
    uncheckedHomeworksCountItems: [],

    loadUncheckedHomeworksCount: async () => {

        set({isLoading: true});

        const response = await $api.get<IServerResponse<IUncheckedHomeworksCountItem[]>>(ENDPOINTS.ADMINISTRATOR.GET_UNCHECKED_HOMEWORKS_COUNT);
        set({uncheckedHomeworksCountItems: response.data.data});

        set({isLoading: false});
    }
}));