interface ICheckboxProps {
    label?: string;
    clickableLabel?: boolean;
    checked: boolean;
    setChecked: (check: boolean) => void;
}

const Checkbox: React.FC<ICheckboxProps> = ({label, clickableLabel, checked, setChecked}) => {
    const toggleChecked = () => setChecked(!checked);

    return (
        <div className='form-check'>
            <input className='form-check-input' type='checkbox' checked={checked} onChange={toggleChecked} />
            <label
                className={`form-check-label ${clickableLabel && 'cursor-pointer'}`}
                onClick={() => clickableLabel && toggleChecked()}>
                {label}
            </label>
        </div>
    );
}
export default Checkbox;