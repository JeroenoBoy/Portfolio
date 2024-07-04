import {PropsWithChildren, ReactNode} from "react";
import {HeaderHeight} from "../Navbar";


export default function ProjectSidebar({ children: [image, ...children] }: { children: [ReactNode, ...ReactNode[]] }) {
    return (
        <div className="sticky top-0">
            <div className="md:pb-4 flex flex-row gap-4 md:gap-1 md:flex-col w-full">
                <HeaderHeight />
                <div className='flex-1'>{image}</div>
                <div className='flex-1'>{children}</div>
            </div>
        </div>
    )
}


export function ProjectSidebarItem({ title, value }: { title: string, value: string }) {
    return (
        <p className="flex items-center">
            <span className="font-bold">{title}</span>
            <span className="ml-auto">{value}</span>
        </p>
    )
}