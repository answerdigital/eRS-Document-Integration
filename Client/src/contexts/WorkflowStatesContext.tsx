import { IWorkflowStatus, IWorkflowStatusResponse } from "common/interfaces/workflow-status.interface";
import { useSession } from "hooks/useSession";
import { createContext, useContext, useEffect, useState } from "react";
import { getWorkflowStates } from "services/worklist-service";

interface IWorkflowStatesContext {
    states: IWorkflowStatusResponse;
    fetchStatusList: () => void;
    getStatus: (wfsmCode?: string) => IWorkflowStatus | undefined;
}

const WorkflowStatesContext = createContext<IWorkflowStatesContext>({
    states: {},
    fetchStatusList: () => undefined,
    getStatus: () => undefined
});

interface IWorkflowStatesContextProviderProps {
    children: React.ReactNode;
}

const WorkflowStatesContextProvider: React.FC<IWorkflowStatesContextProviderProps> = ({children}) => {
    const [workflowStates, setWorkflowStates] = useState<IWorkflowStatusResponse>({});
    const { hasJWT } = useSession();

    useEffect(() => {
        if (hasJWT()) {
            fetchStatusList();
        }
    }, []);

    const fetchStatusList = () => {
        getWorkflowStates().then((response: IWorkflowStatusResponse) => setWorkflowStates(response));
    };

    const getStatus = (wfsmCode?: string): IWorkflowStatus | undefined => {
        let allStates = workflowStates.refReqStates?.concat(workflowStates.refDocStates ?? []);
        return allStates?.find(wfs => wfs.wfsmCode === wfsmCode);
    };

    return (
        <WorkflowStatesContext.Provider value={{
            states: workflowStates,
            fetchStatusList: fetchStatusList,
            getStatus: getStatus
        }}>
            {children}
        </WorkflowStatesContext.Provider>
    );
}

export const useWorkflowStates = () => {
    const context = useContext(WorkflowStatesContext);
    if (context === undefined) {
        throw new Error('useWorkflowStates must be used within a WorkflowStatesContextProvider');
    }
    return context;
}

export default WorkflowStatesContextProvider;