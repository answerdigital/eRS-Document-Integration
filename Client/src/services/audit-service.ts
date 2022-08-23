import { IAuditRequest } from './../common/interfaces/audit.interface';
import api from 'services/api';

export const getAudits = async (request: IAuditRequest) => {
    try {
        const response = await api.post('/AuditLog', request);
        return response.data;
    } catch (e) {
        return [];
    }
}