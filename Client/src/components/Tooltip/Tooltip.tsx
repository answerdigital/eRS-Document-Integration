import * as React from 'react';
import { useState } from 'react';

interface ITooltipProps {
    children: React.ReactNode;
    title?: string;
    text?: string;
}

const Tooltip: React.FC<ITooltipProps> = ({ children, text, title}) => {  
    const [displayTooltip, setDisplayTooltip] = useState<boolean>(false);

    return (
        <div className="tooltip-container">
            <div className={displayTooltip ? 'tooltip-box visible' : 'tooltip-box'}>
                <h4>{title}</h4>
                <p>{text}</p>
            </div>
            <div
                onMouseEnter={() => setDisplayTooltip(true)}
                onMouseLeave={() => setDisplayTooltip(false)}
            >
                {children}
            </div>
        </div>
    )
}

export default Tooltip;