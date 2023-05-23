import {
    ISearchedUserPresentation, IServerResponse,
} from "../../../entities";
import {create} from "zustand";
import {$api, ENDPOINTS} from "../../../shared";



interface IUsersSearchStore {
    searchQuery: string;
    searchedUsers: ISearchedUserPresentation[];
    selectedUser: ISearchedUserPresentation | null;
    isLoading: boolean;
    setSearchQuery: (searchQuery: string) => void;
    setSelectedUser: (selectedUser: ISearchedUserPresentation | null) => void;
    loadSearchedUsers: () => Promise<void>;
}

export const useUsersSearchStore = create<IUsersSearchStore>((set, get) => ({

    searchQuery: '',
    searchedUsers: [],
    selectedUser: null,
    isLoading: false,

    setSearchQuery: (searchQuery) => {
        set({searchQuery: searchQuery});
    },
    setSelectedUser: (selectedUser) => {
        set({selectedUser: selectedUser});
    },

    loadSearchedUsers: async () => {

        set({isLoading: true});
        const {searchQuery} = get();

        const response = await $api.get<IServerResponse<ISearchedUserPresentation[]>>(ENDPOINTS.ADMINISTRATOR.GET_USERS_BY_PARTIAL_NAME + searchQuery);
        set({searchedUsers: response.data.data});

        set({isLoading: false});
    }
}));