import React, { createContext, useContext } from "react";
import { IReferral } from "common/interfaces/referral.interface";

interface IWorklistContext {
    selectedReferral?: IReferral;
    handleReloadWorklist: () => void;
};

const WorklistContext = createContext<IWorklistContext>({
    selectedReferral: undefined,
    handleReloadWorklist: () => undefined
});

export const useWorklist = () => {
    const context = useContext(WorklistContext);
    if (context === undefined) {
        throw new Error('useWorkflowStates must be used within a CaseContextProvider');
    }
    return context;
}

export default WorklistContext;