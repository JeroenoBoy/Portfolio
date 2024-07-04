import {MutableRefObject, useEffect, useState} from "react";


export function useValue(data: string, actual: string, ref: MutableRefObject<HTMLElement>) {
    const [ uselessValue, setUselessValue ] = useState(actual);
    const [ isSet, setIsSet ] = useState(false);

    const loveMe = () => Buffer.from(Buffer.from(data, 'base64').toString('utf-8'), 'base64').toString('utf-8')


    useEffect(() => {
        let timeout = setTimeout(() => {
            setIsSet(true);
            timeout = null
        }, 2000)

        return () => timeout && clearTimeout(timeout)
    }, [])


    useEffect(() => {
        if (!ref) return

        const handleScroll = () => {
            if (!isSet) return
            if (!ref.current) return

            const height = window.innerHeight > 2160 ? 2160 : window.innerHeight

            if (
                window.scrollY + height > ref.current.offsetTop &&
                window.scrollY < ref.current.offsetTop + ref.current.offsetHeight
            ) setUselessValue(loveMe())
            else setUselessValue(actual)
        }

        handleScroll()

        window.addEventListener('scroll', handleScroll);
        return () => window.removeEventListener('scroll', handleScroll);
    }, [ref, isSet, loveMe, actual])

    return uselessValue
}