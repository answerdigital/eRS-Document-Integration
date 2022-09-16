import { IWorkflowStatus, IWorkflowStatusResponse } from "common/interfaces/workflow-status.interface";
import { createContext, useContext, useEffect, useState } from "react";
import { getWorkflowStates } from "services/worklist-service";

interface IWorkflowStatesContext {
    states: IWorkflowStatusResponse;
    getStatus: (wfsmCode?: string) => IWorkflowStatus | undefined;
}

const WorkflowStatesContext = createContext<IWorkflowStatesContext>({
    states: {},
    getStatus: () => undefined
});

interface WorkflowStatesContextProviderProps {
    children: React.ReactNode;
}

const WorkflowStatesContextProvider: React.FC<WorkflowStatesContextProviderProps> = ({children}) => {
    const [workflowStates, setWorkflowStates] = useState<IWorkflowStatusResponse>({});

    useEffect(() => {
        getWorkflowStates().then((response: IWorkflowStatusResponse) => setWorkflowStates(response));
    }, []);

    const getStatus = (wfsmCode?: string): IWorkflowStatus | undefined => {
        let allStates = workflowStates.refReqStates?.concat(workflowStates.refDocStates ?? []);
        return allStates?.find(wfs => wfs.wfsmCode === wfsmCode);
    };

    return (
        <WorkflowStatesContext.Provider value={{
            states: workflowStates,
            getStatus: getStatus
        }}>
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