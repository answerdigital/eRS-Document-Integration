import { IPagedResult, IRequest } from "./api-common.interface";
import { IReferral } from "./referral.interface";

export interface IAuditLog {
    auditRowId: number;
    ersRefReqDetail?: IReferral;
    eventDttm?: Date;
    fromEventCode?: string;
    fromStatusComments?: string;
    toEventCode?: string;
    toStatusComments?: string;
    recStatus?: string;
    recInserted?: Date;
    recInsertedBy?: string;
    userReference?: string;
    erstrnsUid?: string;
    doctrnsUid?: string;
    host?: string;
    nhsNo?: string;
    patName?: string;
}

export interface IAuditFilters {
    eventCode?: string;
    nhsNo?: string;
    refDocUid?: string;
    refReqUid?: string;
    recInsertedFrom?: Date;
    recInsertedTo?: Date;
    recInsertedBy?: string;
}

export interface IAuditResult extends IPagedResult<IAuditLog> {

}

export interface IAuditRequest extends IRequest<IAuditFilters> {

}