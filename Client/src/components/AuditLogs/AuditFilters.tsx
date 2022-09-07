import { IAuditFilters } from "common/interfaces/audit.interface";
import FiltersDropdown from "components/FiltersDropdown/FiltersDropdown";
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
                <div className='col-md-2 mb-2'>
                    <label className='form-label'>Date From</label>
                    <DatePicker className='form-control'
                        selected={filters?.recInsertedFrom}
                        onChange={(date: Date) => updateFilters('recInsertedFrom', date)}
                    />
                </div>
                <div className='col-md-2 mb-2'>
                    <label className='form-label'>Date To</label>
                    <DatePicker className='form-control'
                        selected={filters?.recInsertedTo}
                        onChange={(date: Date) => updateFilters('recInsertedTo', date)}
                    />
                </div>
                <div className='col-md-2 mb-2'>
                    <label className='form-label'>User</label>
                    <input className='form-control'></input>
                </div>
                <div className='col-md-3 mb-2'>
                    <label className='form-label'>Patient Ref.</label>
                    <input className='form-control'></input>
                </div>
                <div className='col-md-3 mb-2'>
                    <label className='form-label'>Document Ref.</label>
                    <input className='form-control'></input>
                </div>
            </div>
        </FiltersDropdown>
    );
}

export default AuditFilters;