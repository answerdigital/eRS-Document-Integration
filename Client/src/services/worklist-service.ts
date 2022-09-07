import { IWorkflowStatus } from 'common/interfaces/workflow-status.interface';
import { IAttachment } from './../common/interfaces/attachment.interface';
import { IWorkflowHistory } from 'common/interfaces/workflow-history.interface';
import { IReferralRequest, IReferralResult } from "common/interfaces/referral.interface";
import api from "./api";
import { toast } from 'react-toastify';
import { AxiosError, AxiosResponse } from 'axios';

const printError = (error: any) => {
    toast.error(`Server error! Code ${error.response?.status}`, { });
};

export const getWorklist = async (request: IReferralRequest): Promise<IReferralResult | undefined> => {
    return api.post('/Worklist', request)
        .then((response: AxiosResponse) => {
            return response.data;
        }).catch((error: AxiosError) => {
            printError(error);
        });
};

export const getWorkflowStates = async () : Promise<IWorkflowStatus[]>  => {
    return api.get('/Worklist/states')
        .then((response: AxiosResponse) => {
            return response.data;
        }).catch((error: AxiosError) => {
            printError(error);
            return [];
        });
};

export const getWorkflowHistory = async (refUid: string): Promise<IWorkflowHistory[]> => {
    return api.get(`/Worklist/history/${refUid}`)
        .then((response: AxiosResponse) => {
            return response.data;
        }).catch((error: AxiosError) => {
            printError(error);
            return [];
        });
};

export const addToWorkflowHistory = async (wfh: IWorkflowHistory): Promise<IWorkflowHistory[]> => {
    return api.post('/Worklist/history', wfh)
        .then((response: AxiosResponse) => {
            return response.data;
        }).catch((error: AxiosError) => {
            printError(error);
            return [];
        });
};

export const updateWorkflowHistory = async (wfh: IWorkflowHistory): Promise<IWorkflowHistory[]> => {
    return api.put('/Worklist/history', wfh)
        .then((response: AxiosResponse) => {
            return response.data;
        }).catch((error: AxiosError) => {
            printError(error);
            return [];
        });
};


export const getAttachments = async (refUid: string): Promise<IAttachment[]> => {
    return api.get(`/Worklist/attachments/${refUid}`)
        .then((response: AxiosResponse) => {
            return response.data;
        }).catch((error: AxiosError) => {
            printError(error);
            return [];
        });
};