export interface IPatient {
    patRowID: number;
    patMrn?: string;
    patNhs?: string;
    patFamilyName?: string;
    patGivenName?: string;
    patSex?: string;
    patDob?: Date;
    patAddressOne?: string;
    patAddressTwo?: string;
    patAddressThree?: string;
    patPostCode?: string;
    patContactNumber?: number;
    recUpdated?: Date;
    recUpdatedBy?: string;
}