import React, {PropsWithChildren, useEffect} from 'react';
import clsx from "clsx";
import styles from '../styles/components/TTP.module.scss';


export default function Ttp({ children, className, tooltip, onMouseEnter, onMouseLeave, ...props }: React.DetailedHTMLProps<React.HTMLAttributes<HTMLSpanElement>, HTMLSpanElement> & { tooltip: string }) {

    const [  isHovered, setIsHovered ] = React.useState(false);



    return (
        <span className={clsx(className, styles.ttp)}
              onMouseEnter={ // @ts-ignore
                (e) => setIsHovered(true) && onMouseEnter && onMouseEnter(e)}
              onMouseLeave={ // @ts-ignore
                (e) => setIsHovered(false) && onMouseLeave && onMouseLeave(e)}
              {...props}
        >
            {children}
            <span className={clsx(styles.content, isHovered ? styles.open : styles.closed)}>
                {tooltip}
            </span>
        </span>
    );
};