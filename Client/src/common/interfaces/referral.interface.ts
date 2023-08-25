import { IPagedResult, IRequest } from "./api-common.interface";
import { IPatient } from './patient.interface';
import { IWorkflowHistory } from "./workflow-history.interface";

export interface IReferral {
    refReqUniqueId: string;
    hospitalId: string;
    refReqNhsno?: string;
    name: string;
    dob: Date;
    gender: string;
    referralDetails: string;
    refReqUbrn?: string;
    usrn: string;
    apptDetails: string;
    refReqStatus?: string;
    apptStDttm: Date;
    apptEndDttm: Date;
    meditechPathway?: string;
    refReqSpecialty?: string;
    consultant?: string;
    ersService?: string;
    patient?: IPatient;
    wfsHistory?: IWorkflowHistory;
}

export interface IReferralFilters {
    refReqUbrn?: string;
    meditechPathway?: string;
    refReqSpecialty?: string;
    consultant?: string;
    ersService?: string;
    investigationMode?: boolean;
}

export interface IReferralResult extends IPagedResult<IReferral> {

}

export interface IReferralRequest extends IRequest<IReferralFilters> {

}