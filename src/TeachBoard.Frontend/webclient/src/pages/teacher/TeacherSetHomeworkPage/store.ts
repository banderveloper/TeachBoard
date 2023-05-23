import {ICreateLessonRequest, IGroup, IServerResponse, ISubject} from "../../../entities";
import {create} from "zustand";
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";

interface IGroupsSubjectsBundle {
    groups: IGroup[];
    subjects: ISubject[],
}

interface ITeacherSetHomeworkStore {
    isSelectDataLoading: boolean;
    isHomeworkUploading: boolean;

    error: string | null,

    selectedSubjectId: number;
    selectedGroupId: number;
    selectedFile: File | null;

    selectData: IGroupsSubjectsBundle,

    setSubjectId: (subjectId: number) => void;
    setGroupId: (groupId: number) => void;
    setSelectedFile: (file: File) => void;

    loadHomework: () => Promise<void>;
    loadSelectData: () => Promise<void>;
}

export const useTeacherSetHomeworkStore = create<ITeacherSetHomeworkStore>((set, get) => ({

    isSelectDataLoading: false,
    isHomeworkUploading: false,

    error: null,
    selectData: {groups: [], subjects: []},


    selectedGroupId: 1,
    selectedSubjectId: 1,
    selectedFile: null,

    setGroupId: (groupId) => {
        set({selectedGroupId: groupId})
    },
    setSubjectId: (subjectId) => {
        set({selectedSubjectId: subjectId})
    },
    setSelectedFile: (file) => {
        set({selectedFile: file})
    },

    loadSelectData: async () => {
        set({isSelectDataLoading: true});

        const response = await $api.get<IServerResponse<IGroupsSubjectsBundle>>(Endpoints.TEACHER.GET_SUBJECTS_GROUPS);

        if (response.data.error) {
            set({error: response.data.error.errorMessage});
        } else {
            set({selectData: response.data.data});
        }

        set({isSelectDataLoading: false});
    },

    loadHomework: async() => {

        const {selectedFile, selectedGroupId, selectedSubjectId} = get();

        let formData = new FormData();
        formData.append('groupId', selectedGroupId.toString());
        formData.append('subjectId', selectedSubjectId.toString());
        formData.append('file', selectedFile!);

        set({isHomeworkUploading: true});

        const response = await $api.post<IServerResponse<any>>(Endpoints.TEACHER.CREATE_HOMEWORK, formData);

        if (response.status === 200) {
            set({isHomeworkUploading: false});
        } else {
            console.error(response);
        }
    }
}));