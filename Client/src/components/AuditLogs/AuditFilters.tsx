import { IAuditFilters } from "common/interfaces/audit.interface";
import FiltersDropdown from "components/FiltersDropdown";
import DatePicker from "react-datepicker";

interface AuditFiltersProps {
    filters: IAuditFilters;
    setFilters: (filters: IAuditFilters) => void;
}

const AuditFilters: React.FC<AuditFiltersProps> = ({filters, setFilters}) => {

    const updateFilters = (field: keyof IAuditFilters, value: string | Date | undefined) => {
        setFilters({...filters, [field]: value});
    };

    return (
        <FiltersDropdown>
            <div className='row mb-3'>
                <div className='col-md-4'>
                    <div className='row mb-2'>
                        <div>
                            <label className='form-label'>Date From</label>
                            <DatePicker className='form-control'
                                selected={filters.recInsertedFrom}
                                onChange={(date: Date) => updateFilters('recInsertedFrom', date)}
                            />
                        </div>
                    </div>
                    <div className='row mb-2'>
                        <div>
                            <label className='form-label'>Date To</label>
                            <DatePicker className='form-control'
                                selected={filters.recInsertedTo}
                                onChange={(date: Date) => updateFilters('recInsertedTo', date)}
                            />
                        </div>
                    </div>
                </div>
                <div className='col-md-4'>
                    <div className='row mb-2'>
                        <div>
                            <label className='form-label'>User</label>
                            <input className='form-control'
                                value={filters.recInsertedBy}
                                onChange={(e) => updateFilters('recInsertedBy', e.target.value)}>
                            </input>
                        </div>
                    </div>
                    <div className='row mb-2'>
                        <div>
                            <label className='form-label'>Patient</label>
                            <input className='form-control'
                                value={filters.nhsNo}
                                onChange={(e) => updateFilters('nhsNo', e.target.value)}>
                            </input>
                        </div>
                    </div>
                </div>
                <div className='col-md-4'>
                    <div className='row mb-2'>
                        <div>
                            <label className='form-label'>Referral ID</label>
                            <input className='form-control'
                                value={filters.refReqUid}
                                onChange={(e) => updateFilters('refReqUid', e.target.value)}>
                            </input>
                        </div>
                    </div>
                    <div className='row mb-2'>
                        <div>
                            <label className='form-label'>Attachment ID</label>
                            <input className='form-control'
                                value={filters.refDocUid}
                                onChange={(e) => updateFilters('refDocUid', e.target.value)}>
                            </input>
                        </div>
                    </div>
                </div>
            </div>
        </FiltersDropdown>
    );
}

export default AuditFilters;