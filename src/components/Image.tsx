import NImage, { ImageProps } from 'next/image';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {faExpand, faSpinner, faXmark} from '@fortawesome/free-solid-svg-icons';
import React, {DetailedHTMLProps, forwardRef, useState, VideoHTMLAttributes} from "react";
import Center from "./alignment/Center";
import styles from "../styles/components/projects/Image.module.scss";
import clsx from "clsx";


export default function Image({width, height, ...props}: ImageProps) {
    const [ open, setOpen ] = useState(false);


    return (<>
        <div className={styles['img']} onClick={() => setOpen(true)}>
            <NImage
                width={width}
                height={height}
                {...props}
            />
            <div className="absolute bottom-2 right-2 text-white hover:scale-125 active:scale-110 active:rotate-12 transition-transform">
                <FontAwesomeIcon icon={faExpand} />
            </div>
        </div>

        {open && (
            <Center className='fixed top-0 right-0 p-4 w-screen h-screen bg-black bg-opacity-50 z-50' onClick={() => setOpen(false)}>
                <div className="shadow-lg shadow-slate-600 rounded-xl flex">
                    <NImage src={props.src} width={width} height={height} className="rounded-xl" />
                </div>
                <Center className="fixed top-3 right-3 rounded-full p-6 w-5 h-5 bg-red-500 hover:bg-red-600 active:bg-red-700 cursor-pointer transition-color duration-150 text-white">
                    <FontAwesomeIcon size='2x' icon={faXmark} />
                </Center>
            </Center>
        )}
    </>);
}


export const Video = forwardRef(function Video({
    src,
    type = "video/mp4",
    autoPlay = true,
    controls = true,
    muted = true,
    loop = true,
    shadow = true,
    className,
    ...props
} : {
    src: string,
    type?: string,
    shadow?: boolean
  } & DetailedHTMLProps<VideoHTMLAttributes<HTMLVideoElement>, HTMLVideoElement>, ref: React.Ref<HTMLVideoElement>)
{
    return (
        <video className={clsx(styles['img'], shadow ?? styles['shadow'], className)}
                  {...{autoPlay, controls, muted, loop, ref, ...props}}>
            <source src={src} type={type}/>
        </video>
    )
});