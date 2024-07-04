import {ReactNode} from "react";

export interface HeaderProps {
  children: ReactNode;
}


export default function ProjectHeader({ children }: HeaderProps) {
  return (
      <div className='flex flex-col gap-2 w-full'>
          {children}
      </div>
  )
}