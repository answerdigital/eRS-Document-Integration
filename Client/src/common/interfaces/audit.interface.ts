import { IPagedResult, IRequest } from "./api-common.interface";

export interface IAuditLog {
    auditRowId : number;
    eventDttm? : Date;
    eventCode? : string;
    eventDescription? : string;
    eventDetails? : string;
    recStatus? : string;
    recInserted? : Date;
    recInsertedBy? : string;
}

export interface IAuditFilters {
    searchByEventCode? : string;
    searchByEventDescription? : string;
    searchByEventDetails? : string;
    filterByRecInsertedFrom? : Date;
    filterByRecInsertedTo? : Date;
    searchByRecInsertedBy? : string;
}

export interface IAuditResult extends IPagedResult<IAuditLog> {

}

export interface IAuditRequest extends IRequest<IAuditFilters> {

}