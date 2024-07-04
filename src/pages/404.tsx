import React from 'react'
import Center from '../components/alignment/Center'

export default function Error404() {
	return (
		<main>
			<Center className='h-page'>
				<h1>404</h1>
				<div className="mx-2 w-1 h-10 rounded-full bg-slate-200" />
				<p className='text-2xl'>This page was not found</p>
			</Center>
		</main>
	)
}
