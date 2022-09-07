export interface IWorkflowHistory {
    ersdocsRowid?: number;
    refReqRowId?: number;
    refDocRowId?: number;
    erstrnsUid?: string;
    doctrnsUid?: string;
    statusCode?: string;
    statusHierarchy?: number;
    statusComments?: string;
    statusEffdttm?: Date;
    statusPerformedBy?: string;
    statusCancelledDttm?: Date;
    statusCancelledBy?: string;
    recStatus?: string;
    recUpdated?: string;
    recUpdatedBy?: string;
    recInserted?: Date;
    recInsertedBy?: string;
}