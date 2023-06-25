import { StoreProjectActionType, IStoreProject } from "./types";

const initState: IStoreProject = {
    isProjectStored: false
}

export const ProjectReducer = (state = initState, action: any): IStoreProject => {
    switch (action.type) {
        case StoreProjectActionType.STORE_CREATE_PROJECT: {
            return {
                ...state,
                isProjectStored: true,
                project: action.payload // зберігаємо інфо про проект, який обрано
            }
        }
        case StoreProjectActionType.STORE_REMOVE_PROJECT: {
            return {
                ...state,
                isProjectStored: false,
                project: undefined
            }
        }
    }
    return state;
}