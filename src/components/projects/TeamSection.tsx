import {PropsWithChildren, useEffect, useRef, useState} from "react";
import ProjectSection from "./Section";


export default function ProjectTeamSection({ children } : PropsWithChildren<any>) {
    return (
        <ProjectSection title="Team">
            <table>
                <tbody>
                    {children}
                </tbody>
            </table>
        </ProjectSection>
    )
}


export interface TeamMember {
    role: string;
    name: string;
    task?: string;
}


export function ProjectTeamMember({ role, name, task } : TeamMember) {
    const [split, shouldSplit] = useState(false);
    const ref = useRef<HTMLTableRowElement>();


    useEffect(() => {
        function handleResize() {
            const size = ref.current.clientWidth
            shouldSplit(size < 450)
        }

        handleResize()

        window.addEventListener('resize', handleResize);
        return () => window.removeEventListener('resize', handleResize);
    })


    return split
        ? (<>
            <tr ref={ref}>
                <td className='font-bold'>{role}</td>
            </tr>
            <tr>
                <td>{name}</td>
                <td>{task}</td>
            </tr>
            <tr className='text-xs'>
                <td>&nbsp;</td>
            </tr>
        </>) : (
            <tr ref={ref}>
                <td className='font-bold w-24'>{role}</td>
                <td>{name}</td>
                <td>{task}</td>
            </tr>
        )
}