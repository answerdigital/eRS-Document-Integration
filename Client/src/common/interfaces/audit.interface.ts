import { IPagedResult, IRequest } from "./api-common.interface";

export interface IAuditLog {
    auditRowId: number;
    eventDttm?: Date;
    eventCode?: string;
    eventDescription?: string;
    eventDetails?: string;
    recStatus?: string;
    recInserted?: Date;
    recInsertedBy?: string;
}

export interface IAuditFilters {
    eventCode?: string;
    recInsertedFrom?: Date;
    recInsertedTo?: Date;
    recInsertedBy?: string;
}

export interface IAuditResult extends IPagedResult<IAuditLog> {

}

export interface IAuditRequest extends IRequest<IAuditFilters> {

}