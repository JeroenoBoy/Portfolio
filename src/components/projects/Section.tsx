import {createContext, PropsWithChildren, useCallback, useContext, useEffect, useRef, useState} from "react";
import {faArrowDown} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import styles from '../../styles/components/projects/Section.module.scss';
import clsx from "clsx";


interface SectionProps {
    title: string;
    startOpen?: boolean;
}


export default function ProjectSection({children, title, startOpen = false} : PropsWithChildren<SectionProps>) {
    const [isOpen, setIsOpen] = useState(startOpen);
    const [height, setHeight] = useState(44);

    const [childHeight, setChildHeight] = useState(0);

    const headerRef = useRef<HTMLDivElement>();
    const contentRef = useRef<HTMLDivElement>();
    

    useEffect(() => {
        const headerHeight = (headerRef.current?.clientHeight ?? 44) + 4;
        const contentHeight = contentRef.current?.clientHeight ?? 0;

        const newHeight = headerHeight + (isOpen ? contentHeight : 0);

        setHeight(newHeight);
    }, [isOpen, childHeight]);


    useEffect(() => {
        if (!contentRef.current) return;

        const listener = () => {
            setChildHeight(contentRef.current?.clientHeight ?? 0);
        }

        let observer = new ResizeObserver(listener);
        observer.observe(contentRef.current);
        return () => observer.disconnect()
    }, [contentRef]);


    return (
        <section className='w-full transition-all duration-300 overflow-y-hidden overflow-x-clip relative' style={{height}}>
            <div
                ref={headerRef}
                className={styles['button']}
                onClick={() => setIsOpen(!isOpen)}
            >
                <FontAwesomeIcon className={clsx('transform transition-transform duration-300', {
                    'rotate-0': isOpen,
                    'rotate-180': !isOpen
                })} size='lg' icon={faArrowDown}/>
                <h2 className='text-xl'>{title}</h2>
            </div>

            <div className='ml-4 flex flex-col gap-2 pt-2 absolute w-[calc(100%-16px)]' ref={contentRef}>
                {children}
            </div>
        </section>
    )
}