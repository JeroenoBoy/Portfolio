import {DetailedHTMLProps, HTMLAttributes} from "react";


export default function Row({ className, ...props }: DetailedHTMLProps<HTMLAttributes<HTMLDivElement>, HTMLDivElement>) {
    return (
        <div className={`flex flex-row ${className}`} {...props} />
    );
}