import { IWorkflowStatus } from "common/interfaces/workflow-status.interface";
import { createContext, useContext, useEffect, useState } from "react";
import { getWorkflowStates } from "services/worklist-service";

const WorkflowStatesContext = createContext< IWorkflowStatus[]>([]);

interface WorkflowStatesContextProviderProps {
    children: React.ReactNode;
}

const WorkflowStatesContextProvider: React.FC<WorkflowStatesContextProviderProps> = ({children}) => {
    const [workflowStates, setWorkflowStates] = useState<IWorkflowStatus[]>([]);

    useEffect(() => {
        getWorkflowStates().then((response: IWorkflowStatus[]) => setWorkflowStates(response));
    }, []);

    return (
        <WorkflowStatesContext.Provider value={workflowStates}>
            {children}
        </WorkflowStatesContext.Provider>
    );
}

export const useWorkflowStates = () => {
    const context = useContext(WorkflowStatesContext);
    if (context === undefined) {
        throw new Error('useWorkflowStates must be used within a CaseContextProvider');
    }
    return context;
}

export default WorkflowStatesContextProvider;