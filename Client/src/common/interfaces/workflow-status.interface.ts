export interface IWorkflowStatusUpdate {
    wfsmCode?: string;
    date?: Date;
    comments?: string;
    user?: string;
}

export interface IWorkflowStatus {
    wfsmCode?: string;
    wfsmDescription?: string;
    wfsmDisplayValue?: string;
    wfsmHierarchy?: number;
    wWfsmPrevHierarchy?: string;
    wfsmNextHierarchy?: string;
    recStatus?: string;
    recUpdated?: Date;
    recUpdatedBy?: string;
    recInserted?: Date;
    recInsertedBy?: string;
    recPurgeDays?: number;
    errorStatus?: boolean;
}

export interface IWorkflowStatusResponse {
    refReqStates?: IWorkflowStatus[];
    refDocStates?: IWorkflowStatus[];
}