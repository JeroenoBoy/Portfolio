import React, {DetailedHTMLProps, HTMLAttributes, PropsWithChildren} from 'react'
import clsx from 'clsx'


export default function Center({ className, ...props }: DetailedHTMLProps<HTMLAttributes<HTMLDivElement>, HTMLDivElement>) {
  return (
	<div className={clsx('flex justify-center items-center m-auto', className)} {...props} />
  )
}
