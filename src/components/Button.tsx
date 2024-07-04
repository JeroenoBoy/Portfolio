import React from 'react'
import clsx from 'clsx';
import styles from '../styles/components/button.module.scss'
import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import NLink, {LinkProps} from 'next/link';


export interface IButtonProps extends React.DetailedHTMLProps<React.ButtonHTMLAttributes<HTMLButtonElement>, HTMLButtonElement> {
	color: 'primary' | 'secondary' | 'white' | 'blue' | 'red' | 'teal' | string,
	icon?: IconProp,
	to?: string,
}


export default function Button({ color, className, icon, children, to = null, ...props }: IButtonProps) {

	if (to && to.startsWith('http')) return (
		<a href={to} target='_blank' rel='noopener noreferrer' className={clsx(styles.btn, styles[color], className)}>
			{icon && <FontAwesomeIcon icon={icon} className="mr-1" />}
			{children}
		</a>
	)
	else if (to) return (
		<NLink href={to} {...(props as any as LinkProps)}>
			<a className={clsx(className, styles.btn, styles[color])}>
				{icon && <FontAwesomeIcon icon={icon} className="mr-1" />}
				{children}
			</a>
		</NLink>
	)
	else return (
		<button className={clsx(className, styles.btn, styles[color])} {...props}>
			{icon && <FontAwesomeIcon icon={icon} className="mr-1" />}
			{children}
		</button>
	)
}


export function IconButton({ color, className, icon, children, to = null, ...props }: IButtonProps) {

	if (to && to.startsWith('http')) return (
		<a href={to} target='_blank' rel='noopener noreferrer' className={clsx(styles.iconbtn, styles[color], className)}>
			<FontAwesomeIcon icon={icon} />
		</a>
	)

	else if (to) return (
		<NLink href={to} {...(props as any as LinkProps)} >
			<a className={clsx(styles.iconbtn, styles[color], className)}>
				<FontAwesomeIcon icon={icon} />
			</a>
		</NLink>
	)

	else return (
			<button className={clsx(styles.iconbtn, styles[color], className)} {...props}>
				<FontAwesomeIcon icon={icon} />
			</button>
		)
}