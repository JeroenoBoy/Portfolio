import clsx from 'clsx'
import React from 'react'


const types = {
	'C#': { color: 'bg-purple-500', text: 'text-slate-100' },
	'Unity': { color: 'bg-gray-900', text: 'text-slate-100' },
	'School': { color: 'bg-blue-500', text: 'text-slate-100' },
	'GameJam': { color: 'bg-orange-500', text: 'text-slate-100' },
	'AI': { color: 'bg-red-500', text: 'text-slate-100' },
	'FSM': { color: 'bg-red-500', text: 'text-slate-100' },
	'Group': { color: 'bg-green-500', text: 'text-slate-100' },
	'Solo': { color: 'bg-green-500', text: 'text-slate-100' },
	'Prototype': { color: 'bg-yellow-500', text: 'text-slate-100' },
	'Other': { color: 'bg-gray-500', text: 'text-slate-100' },
}


export type BadgeName = keyof typeof types | string


export default function Badge({ type }: { type: BadgeName }) {
	const { color, text } = types[type] || types['Other']

	return (
		<span className={clsx('rounded-full py-0.5 px-1 text-xs', color, text)}>{type}</span>
	)
}
