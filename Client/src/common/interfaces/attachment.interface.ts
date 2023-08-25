import { IPatient } from "./patient.interface";
import { IWorkflowHistory } from "./workflow-history.interface";

export interface IAttachment {
    refrequestRowId?: number;
    refDocSrlno?: number;
    refDocUniqueId?: string; //UID of referral
    refDocStatus?: string;
    attachId?: string; //UID of document
    attachInsertedBy?: string;
    attachContentType?: string;
    attachUrl?: string;
    attachSize?: string;
    attachTitle?: string;
    attachCrtdDttm?: Date;
    docDownloadUrl?: string;
    docLocationUri?: string;
    recStatus?: string;
    recUpdated?: Date;
    recUpdatedBy?: string;
    recInserted?: Date;
    recInsertedBy?: string;
    wfsHistory?: IWorkflowHistory;
    patient?: IPatient;
}