import {PropsWithChildren, ReactNode} from "react";
import Center from "../alignment/Center";
import {HeaderHeight} from "../Navbar";
import Image from "next/image"


export default function Project({ children: [header, side, ...sections] }: { children: [ ReactNode, ReactNode, ...ReactNode[] ] }) {
    return (
        <div>
            <div className='flex justify-center m-10'>
                <div className='w-full lg:max-w-5xl min-h-screen'>
                    <div className='flex flex-col md:flex-row gap-4 h-full'>
                        <div className='w-full md:max-w-xs relative'>
                            {side}
                        </div>
                        <div className="w-full">
                            <HeaderHeight/>
                            <div className='flex-1 w-full flex flex-col gap-2'>
                                {header}
                                {sections}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}