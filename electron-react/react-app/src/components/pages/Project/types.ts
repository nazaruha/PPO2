export interface IProjectItem {
    id: number,
    name: string
}

export interface IProjectCreateItem {
    name: string
}

export interface IProjectServiceResponse {
    success: boolean,
    errors: string[],
    message: string,
    payload: IProjectItem[]
}

export interface IStoreProject {
    isProjectStored: boolean,
    project?: IProjectItem
}

export enum StoreProjectActionType {
    STORE_CREATE_PROJECT = "STORE_CREATE_PROJECT",
    STORE_REMOVE_PROJECT = "STORE_REMOVE_PROJECT"
}