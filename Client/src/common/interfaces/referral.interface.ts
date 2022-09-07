import { IPagedResult, IRequest } from "./api-common.interface";

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
}

export interface IReferralFilters {
    meditechPathway?: string[];
    refReqSpecialty?: string[];
    consultant?: string;
    ersService?: string;
}

export interface IReferralResult extends IPagedResult<IReferral> {

}

export interface IReferralRequest extends IRequest<IReferralFilters> {

}