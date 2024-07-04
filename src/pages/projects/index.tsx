import Badge, {BadgeName} from "../../components/Badge";
import {ReactNode, useEffect, useRef, useState} from "react";
import Button from "../../components/Button";
import {HeaderHeight} from "../../components/Navbar";
import Center from "../../components/alignment/Center";
import Image from "next/image";
import styles from '../../styles/project.module.scss';
import {Video} from "../../components/Image";
import clsx from "clsx";
import {faLink} from "@fortawesome/free-solid-svg-icons";

export default function Index() {
    return (
        <Center>
            <div className='m-16 sm:m-8'>
                <HeaderHeight />
                <h2 className="text-center mb-4">Projects</h2>
                <div className='max-w-5xl grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6'>

                    <Project
                        title='Moonlight Peaks'
                        page='moonlightpeaks'
                        description='Live the vampire life in the magical town of Moonlight Peaks! Master the art of potions and spells, manage your supernatural farm, and leave your mark on the magical town.'
                        features={['Internship', 'Professional', 'C#', 'Unity']}
                    />

                    <Project
                        title='Drone Operator'
                        page='droneoperator'
                        description='Defend your convoy against the enemies, as a drone operator! This game was made in 8 weeks and released on Itch.io'
                        features={['C#', 'Unity', 'VFX', 'School', 'Group']}
                    />

                    <Project
                        title='Last Pipe of Defence'
                        page='lastpipe'
                        description='Defend your base against the intruders with turrets, pipes and extractors. This is a intense prototype made in 4 weeks.'
                        features={['C#', 'Unity', 'Maya', 'School', 'Solo', 'Prototype']}
                    />

                    <Project
                        title='Inceptum 32'
                        page='inceptum32'
                        description='In this puzzle game, you play as a human that can solve puzzles by switching between a human and a slime, each with their own unique abilities.'
                        features={['C#', 'Unity', 'School', 'Group', 'Puzzle']}
                    />

                    <Project
                        title='Nebula: Lost Friend'
                        page='nebula'
                        description='Fly through a beautiful galaxy and try to find your lost friend. This beautiful game was made in 3 days for the IEGJ 2022 gamejam'
                        features={['C#', 'Unity', 'School', 'GameJam', 'Group']}
                    />

                    <Project
                        title='Bottom Gear - Stig Kart 64'
                        page='stigkart64'
                        description='Race against AI or your friends with split screen. This retro styled game was made in 2 weeks with 3 artists and 3 developers.'
                        features={['C#', 'Unity', 'School', 'SplitScreen', 'Group']}
                    />

                    <Project
                        title='AI Playground'
                        page='ai'
                        description='Interesting AI Behaviour using the finite state machine & strategy Pattern. This page also features a war & creature simulation.'
                        features={['C#', 'Unity', 'School', 'AI', 'FSM', 'Solo', 'Group']}
                    />
                </div>
            </div>
        </Center>
    )
}


export function Project({ title, page, description, features, buttons = []}: { title: string, page: string, description: string, features: BadgeName[], buttons?: ReactNode[]}) {

    const [fullScreen, setFullScreen] = useState(false);
    const [hover, setHover] = useState(false);
    const ref = useRef<HTMLVideoElement>();


    useEffect(() => {
        const video = ref.current;
        if (!video) return;

        const listener = () => {
            setFullScreen(window.innerHeight === screen.height);
        }

        video.addEventListener('fullscreenchange', listener);
        return () => video.removeEventListener('fullscreenchange', listener)
    }, [ref]);



    useEffect(() => {
        const video = ref.current;

        if (video == null || fullScreen) return;

        if (!hover)
            video.pause();
        else {
            video.currentTime = 0;
            video.play();
        }

    }, [ref, hover, fullScreen]);



    return (
        <div className={styles['project']}
             onMouseEnter={() => setHover(true)}
             onMouseLeave={() => setHover(false)}
        >
            <div className='flex flex-col h-full gap-2'>
                <h3 className='text-xl'>{title}</h3>

                <div className={styles['project__banner__container']}>
                    <Image className={clsx(styles['project__banner'], (hover && !fullScreen) && '!hidden')}
                           width={640} height={256}
                           alt={'Project Banner - ' + title}
                           src={`/projects/${page}/banner.png`}
                    />
                    <Video className={clsx(
                               styles['project__banner'], // @ts-ignore
                               (fullScreen + hover < 1) && '!hidden'
                           )}
                           src={`/projects/${page}/video.mp4`}
                           shadow={false}
                           preload="none"
                           autoPlay={false}
                           ref={ref}
                    />
                </div>

                <ul className='flex gap-1 flex-wrap pl-0'>
                    {features.map((feature, i) => <Badge key={i} type={feature} />)}
                </ul>
                <p className='my-2'>{description}</p>

                <div className='flex-1 h-full'/>

                <ul className='flex gap-2 flex-wrap pl-0'>
                    <Button className="overflow-hidden h-10 text-ellipsis whitespace-nowrap"
                            color='primary'
                            to={'/projects/'+page}
                            icon={faLink}
                    >
                        {title}
                    </Button>
                    {buttons}
                </ul>
            </div>
        </div>
    )
}