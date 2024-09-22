import React, {useRef} from 'react'
import {IconProp} from "@fortawesome/fontawesome-svg-core";
import clsx from "clsx";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import NLink from "next/link";
import {faGithub, faItchIo} from "@fortawesome/free-brands-svg-icons";
import {faEnvelope} from "@fortawesome/free-solid-svg-icons";
import {useEmail} from "./hooks/useEmail";


export default function Footer() {
	const ref   = useRef<HTMLDivElement>()
	const email = useEmail(ref);

	return (
		<footer>
			<img src="/images/layered-waves.svg" className="object-cover object-bottom w-full"/>
			<div className="pb-8 xl:pb-12 mt-0 md:mt-[-2rem] lg:mt-[-4rem]">
				<p>Jeroen van de Geest</p>
				<div ref={ref} className='flex gap-6 justify-center items-center'>
					<Link icon={faEnvelope} link={'mailto:'+email} />
					<Link icon={faItchIo} link='https://jeroeno-boy.itch.io' />
					<Link icon={faGithub} link='https://github.com/JeroenoBoy' />
				</div>
				<p>© 2021 - {new Date().getFullYear()}</p>
			</div>
		</footer>
	)
}


function Link({ icon, link, className }: { icon: IconProp, link: string, className?: string }) {
	const name = clsx('w-8 h-8 text-slate-400 hover:text-slate-500 active:text-slate-600', className)

	if (link.startsWith('http')) return (
			<a href={link} target='_blank' rel='noopener noreferrer'>
				<FontAwesomeIcon icon={icon} size="2x" className={name} />
			</a>
	)

	return (
		<NLink href={link}>
			<FontAwesomeIcon icon={icon} size="2x" className={name} />
		</NLink>
	)
}