/* eslint-disable react-hooks/rules-of-hooks */
import clsx from 'clsx'
import React, {PropsWithChildren, useEffect, useState} from 'react'
import styles from 'styles/components/navbar.module.scss'
import Link from 'next/link';
import {useRouter} from 'next/router';
import Image from "next/image";
import {ResponsiveSize, useSize} from "./hooks/useSize";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faBars} from "@fortawesome/free-solid-svg-icons";
import {Section, useScrollAgent, useScrollAgentState} from "./hooks/ScrollAgent";

//	Component

const links = [
	{href: '/', label: 'Home', name: 'home'},
	{href: '/#aboutme', label: 'About Me', name: 'about'},
	{href: '/#projects', label: 'Projects', name: 'projects'},
	{href: '/#contact', label: 'Contact', name: 'contact'},
	{href: '/Resume.pdf', label: 'Resume', name: 'resume'},
]

export default function Navbar() {
	const { pathname } = useRouter();
	const size = useSize();
	const [ isHidden, setIsHidden ] = useState(true);


	useEffect(() => {
		const handleScroll = () => {
			setIsHidden(window.scrollY <= 0);
		};

		handleScroll()

		window.addEventListener('scroll', handleScroll);
		return () => window.removeEventListener('scroll', handleScroll);
	}, [pathname]);


	const hidden = pathname === '/' && isHidden;


	return (
		<nav className={clsx(styles.nav, hidden && '!bg-opacity-0 !shadow-none')}>
			<NavbarGroup align={0}>
				<Image src={"/images/me.jpg"} alt="Image of me" width={32} height={32} className="rounded" />
			</NavbarGroup>

			{size < ResponsiveSize.sm
				? <Burgir isHidden={isHidden} />: (<>
					<NavbarGroup align={1}>
						{links.map(({href, label, name}, i) => (
							<NavLink name={name} to={href} key={i}>{label}</NavLink>
						))}
					</NavbarGroup>
					<NavbarGroup align={2} />
				</>)
			}
		</nav>
	)
}


function Burgir({ isHidden }: { isHidden: boolean }) {
	const [ open, setOpen ] = useState(false);

	return (<>
		<div className='flex gap-1 items-center relative'>
			<button
				className={styles.iconbtn}
				onClick={() => setOpen(!open)}
			>
				<FontAwesomeIcon icon={faBars} />
			</button>
		</div>
		<div className={clsx(styles.dropDown, isHidden ? 'top-10' : 'top-12', open ? 'h-auto' : 'h-0')}>
			{links.map(({href, label}, i) => (
				<DDLink to={href} key={i} onClick={() => setOpen(false)}>{label}</DDLink>
			))}
		</div>
	</>)
}


export function HeaderHeight() {
	return (
		<div style={{height: '50px'}} />
	)
}


//	Helper component types


export function NavbarGroup({ align, children }: PropsWithChildren<{ align: 0 | 1 | 2 }>) {
	return (
		<div className={clsx('flex gap-4 items-center relative', {
			'justify-start flex-1': align == 0,
			'justify-center': align == 1,
			'justify-end flex-1': align == 2
		})}>
			{children}
		</div>
	)
}


export function NavLink({ name, to, children }: PropsWithChildren<{ name: string, to: string }>) {
	const [ isHovering, setIsHovering ] = useState(false);
	const isCurrent = useScrollAgentState(name);

	return (
		<Link href={to}>
			<a
				className={clsx(styles.link, (isHovering || isCurrent) && styles.hover)}
				onMouseEnter={() => setIsHovering(true)}
				onMouseLeave={() => setIsHovering(false)}
			>{children}</a>
		</Link>
	)
}


//	Dropdown components



function DDLink({ to, ...props }: PropsWithChildren<{ to: string }> & React.DetailedHTMLProps<React.AnchorHTMLAttributes<HTMLAnchorElement>, HTMLAnchorElement>) {
	return (
		<Link href={to}>
			<a className={styles.ddLink} {...props} />
		</Link>
	)
}