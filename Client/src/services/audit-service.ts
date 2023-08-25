import { IAuditRequest, IAuditResult } from './../common/interfaces/audit.interface';
import api, { printApiError } from 'services/api';
import { AxiosError, AxiosResponse } from 'axios';
import { ICsvFile } from 'common/interfaces/files.interface';

export const getAudits = async (request: IAuditRequest)  : Promise<IAuditResult | undefined> => {
    return api.post('/AuditLog', request)
        .then((response: AxiosResponse) => {
            return response.data;
        }).catch((error: AxiosError) => {
            printApiError(error);
        });
};

export const generateCsv = async (request: IAuditRequest)  : Promise<ICsvFile | undefined> => {
    return api.post('/AuditLog/export', request)
        .then((response: AxiosResponse) => {
            return response.data;
        }).catch((error: AxiosError) => {
            printApiError(error);
        });
};