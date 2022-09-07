import { IReferralFilters, IReferralRequest } from "common/interfaces/referral.interface"
import FiltersDropdown from "components/FiltersDropdown/FiltersDropdown";
import Select from "react-select";

const pathwaysOpts = [
    'Dx000',
    'Dx001',
    'Dx0102',
    'Dx0109'
].map(opt => ({label: opt, value: opt}));

const specialitiesOpts = [
    'GENERAL SURGERY',
    'CARDIOLOGY',
    'UROLOGY',
    'NEUROSURGERY'
].map(opt => ({label: opt, value: opt}));

interface WorklistFiltersProps {
    filters: IReferralFilters;
    setFilters: (filters: IReferralFilters) => void;
}

const WorklistFilters: React.FC<WorklistFiltersProps> = ({filters, setFilters}) => {

    const updateFilters = (field: keyof IReferralFilters, value: string | string[] | undefined) => {
        setFilters({...filters, [field]: value});
    };
    
    return (
        <FiltersDropdown>
            <div className='row mb-3'>
                <div className='col-md-3 mb-3'>
                    <label className='form-label'>Pathway</label>
                    <Select
                        options={pathwaysOpts}
                        isMulti
                        onChange={opts => updateFilters('meditechPathway', opts.map(opt => opt.value))} />
                </div>
                <div className='col-md-3 mb-3'>
                    <label className='form-label'>Speciality</label>
                    <Select
                        options={specialitiesOpts}
                        isMulti
                        onChange={opts => updateFilters('refReqSpecialty', opts.map(opt => opt.value))} />
                </div>
                <div className='col-md-3 mb-3'>
                    <label className='form-label'>Consultant</label>
                    <input className='form-control' onChange={(e) => updateFilters('consultant', e.target.value)}></input>
                </div>
                <div className='col-md-3 mb-3'>
                    <label className='form-label'>eRS Service</label>
                    <input className='form-control' onChange={(e) => updateFilters('ersService', e.target.value)}></input>
                </div>
            </div>
        </FiltersDropdown>
    );
}

export default WorklistFilters;