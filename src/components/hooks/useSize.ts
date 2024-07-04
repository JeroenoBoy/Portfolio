import {useEffect, useState} from "react";


export enum ResponsiveSize {
    xs, sm, md, lg, xl, xxl
}

export function useSize() {
    const [ size, setSize ] = useState(ResponsiveSize.md);

    useEffect(() => {
        function getSize() {
            // @ts-ignore
            const size = window.innerWidth;

            if      (size > 1536) setSize(ResponsiveSize.xxl);
            else if (size > 1284) setSize(ResponsiveSize.xl);
            else if (size > 1024) setSize(ResponsiveSize.lg);
            else if (size > 768)  setSize(ResponsiveSize.md);
            else if (size > 640)  setSize(ResponsiveSize.sm);
            else                  setSize(ResponsiveSize.xs);
        }

        getSize();

        window.addEventListener('resize', getSize);
        return () => window.removeEventListener('resize', getSize);
    }, []);

    return size
}