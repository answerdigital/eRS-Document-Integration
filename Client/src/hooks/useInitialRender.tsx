import { useEffect, useRef } from "react";

export const useInitialRender = () => {
    const ref = useRef(false);
    useEffect(() => {
      ref.current = true;
    }, []);
    return ref.current;
};