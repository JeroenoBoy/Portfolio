/* eslint-disable react/no-unescaped-entities */
import type { NextPage } from 'next'
import Head from 'next/head'
import Center from '../components/alignment/Center';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faGithub, faItchIo, faLinkedin } from "@fortawesome/free-brands-svg-icons";
import { faEnvelope, faUser, faDiceD6, faFile, faSpinner } from "@fortawesome/free-solid-svg-icons";
import NLink from 'next/link';
import { Project } from "./projects";
import Button from "../components/Button";
import { useEmail } from "../components/hooks/useEmail";
import Projects from "./projects/index";
import { FormEvent, MutableRefObject, ReactNode, useRef, useState } from "react";
import Carousel from "../components/Carousel";
import Image from 'next/image';
import styles from '../styles/Home.module.scss'
import { ScrollAgentMark } from "../components/hooks/ScrollAgent";


const Home: NextPage = () => {
    var age = new Date().getFullYear() - new Date('Jan 17 2003').getFullYear()

    return (
        <main className={styles['home']}>
            <Head>
                <title>Jeroen van de Geest</title>
                <meta name="description" content="A game developer's journey" />
                <link rel="icon" href="/favicon.ico" />
            </Head>


            <ScrollAgentMark name={'home'}>
                {(ref) => (
                    <section id="home" className={styles['hero']} ref={ref}>
                        <div className={styles['hero__carousel']}>
                            <Carousel
                                className={styles['full']}
                                stayFor={5000}
                                duration={1000}
                                images={[
                                    '/images/carousel/3.png',
                                    '/images/carousel/5.png',
                                    '/images/carousel/1.png',
                                    '/images/carousel/4.png',
                                    '/images/carousel/2.jpg',
                                ]}
                            />
                        </div>
                        <div className={styles['hero__text']}>
                            <h1 className='text-4xl md:text-5xl'>Jeroen van de Geest</h1>
                            <p className='text-2xl'>A game developer on a journey</p>
                            <div className='flex gap-4'>
                                <Button color='primary' to='/#projects' icon={faDiceD6}>Projects</Button>
                                <Button color='secondary' to='/Resume.pdf' icon={faUser}>Resume</Button>
                            </div>
                        </div>
                    </section>
                )}
            </ScrollAgentMark>


            <section id="featured" className={styles['featured']}>
                <Center>
                    <div className={styles['featured__container']}>
                        <h2 className='text-center'>Featured projects</h2>
                        <div className={styles['featured__row']}>

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
                        </div>
                        <div className='flex justify-end'>
                            <Button color="primary" to="/#projects" icon={faDiceD6}>All Projects</Button>
                        </div>
                    </div>
                </Center>
            </section>


            <ScrollAgentMark name={'about'}>
                {(ref) => (
                    <section id="aboutme" ref={ref}>
                        <img alt="About me top wave" src="/images/wave1.svg" className='w-full object-cove -mb-1' />
                        <div className='bg-slate-800 text-slate-200'>
                            <Center>
                                <div className={styles['aboutme__container']}>
                                    <div className={styles['aboutme__text'] + " text-lg"}>
                                        <h2>About Me</h2>
                                        <p>
                                            Hi there, I am Jeroen, a {age}-year-old developer with over {age - 12} years
                                            of experience in writing code. I love creating, writing, editing and
                                            testing code. I enjoy making games and applications and am not afraid
                                            to accept new challenges and experiment with new technologies. I love
                                            diving into the unkown and never say no to a challenge. I consider this
                                            to be one of my greatest strengths.
                                        </p>
                                        <p>
                                            I started writing code from a young age using the <Link link="https://github.com/SkriptLang/Skript" name="Skriptlang/Skript">Skript</Link> plugin for Minecraft servers.
                                            It helped me learn a lot of the fundementals of programming. Eventually I
                                            started making websites & webservers using JS/TS and eventually picked up
                                            React & NextJS. After some time I decided I wanted to study Game Development,
                                            so I applied at the <Link link="https://www.glu.nl/" name="GLU">Grafisch Lyceum Utecht</Link>.
                                            Since then I worked on many different projects and managed to
                                            get an Internship at <Link link="https://littlechicken.nl" name="Little Chicken">Little Chicken</Link> and worked on the awesome game <Link link="/projects/moonlightpeaks" name="Moonlight Peaks">Moonlight Peaks</Link>.
                                        </p>
                                        <p>
                                            I like programming all sort of things, from intricate systems to complete
                                            features. I always strive for creating readable, maintainable and extendable
                                            code. I am always eager to learn new technologies and programming languages.
                                            I never say no to a challenge and consider it to be one of my greatest
                                            strengths.
                                        </p>
                                    </div>
                                    <div className={styles['aboutme__profile']}>
                                        <Image width={160} height={160} alt='Image of me' src="/images/me.jpg" className='rounded object-cover' />
                                        <div>
                                            <p>Jeroen van de Geest</p>
                                            <h3 className='text-lg mt-2'>Area of Expertise</h3>
                                            <ul className='text-sm text-slate-300'>
                                                <li>C#/Unity</li>
                                                <li>Kotlin</li>
                                                <li>Paper/Spigot</li>
                                                <li>JS/TS</li>
                                                <li>NodeJS/ExpressJS</li>
                                                <li>HTML/CSS</li>
                                            </ul>
                                            <h3 className='text-lg mt-2'>Familiar with</h3>
                                            <ul className='text-sm text-slate-300'>
                                                <li>Java</li>
                                                <li>Go</li>
                                                <li>TSX/CRA/NextJS</li>
                                                <li>Python/Django</li>
                                                <li>PHP</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </Center>
                        </div>
                        <img alt="Aboutme top wave" src="/images/wave2.svg" className='w-full object-cover -mt-1' />
                    </section>
                )
                }
            </ScrollAgentMark >

            <ScrollAgentMark name={'projects'}>
                {(ref) => (
                    <section
                        id="projects"
                        ref={ref}
                    >
                        <Projects />
                    </section>
                )}
            </ScrollAgentMark>

            <ScrollAgentMark name={'contact'}>
                {(ref) => (
                    <Contact passRef={ref} />
                )}
            </ScrollAgentMark>
        </main >
    )
}

export default Home


function Contact({ passRef }: { passRef?: MutableRefObject<HTMLElement> }) {
    const ref = passRef;
    const email = useEmail(ref);
    const [sending, setSending] = useState(false);


    function onSubmit(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();
        const target = event.target;

        const name = target[0].value;
        const email = target[1].value;
        const subject = target[2].value;
        const message = target[3].value;

        setSending(true);

        fetch('https://formspree.io/f/mnqrnrgv', {
            method: 'POST',
            headers: {
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                email, name, subject, message
            })
        })
            .then(() => {
                alert('Message sent!');
            })
            .catch(() => {
                alert('Something went wrong, please try again later');
            })
            .finally(() => {
                setSending(false);
            });

        console.log(event);
    }


    return (
        <section ref={ref} className={styles['contact']} id="contact">
            <div className={styles['contact__container']}>
                <h2 className='text-center mb-5'>Contact & Links</h2>
                <hr className="mb-6" />
                <div className="flex gap-4">
                    <form
                        className="w-96 flex gap-2 flex-col items-start"
                        onSubmit={onSubmit}
                    >
                        <div className="flex gap-2">
                            <div>
                                <input
                                    type="text"
                                    id="name"
                                    name="name"
                                    placeholder="Name"
                                    className={styles['form__input']}
                                    required
                                />
                            </div>
                            <div>
                                <input
                                    type="email"
                                    id="email"
                                    name="email"
                                    placeholder="Email"
                                    className={styles['form__input']}
                                    required
                                />
                            </div>
                        </div>
                        <div className="w-full">
                            <input
                                type="text"
                                id="subject"
                                name="subject"
                                placeholder="Subject"
                                className={styles['form__input']}
                                required
                            />
                        </div>
                        <div className="w-full">
                            <textarea
                                className={styles['form__input'] + " min-h-[6rem]"}
                                placeholder="Message"
                                name="message"
                                required
                            />
                        </div>

                        {sending ? (
                            <button className={styles['form__button']} disabled={true}>
                                <FontAwesomeIcon icon={faSpinner} spin />
                            </button>
                        ) : (
                            <input className={styles['form__button']} type="submit" value="Submit" />
                        )}
                    </form>
                    <div className="w-1 bg-slate-700 rounded-full" />
                    <table className="h-min">
                        <tbody>
                            <tr>
                                <td className="pr-4 py-1"><FontAwesomeIcon size="lg" icon={faEnvelope} /></td>
                                <td><Link link={"mailto:" + email} text={email} name="Send a mail!" /></td>
                            </tr>
                            <tr>
                                <td className="pr-4 py-1"><FontAwesomeIcon size="lg" icon={faFile} /></td>
                                <td><Link link="/Resume.pdf" name="Resume">
                                    Resume
                                </Link></td>
                            </tr>
                            <tr>
                                <td className="pr-4 py-1"><FontAwesomeIcon size="lg" icon={faLinkedin} /></td>
                                <td><Link link="https://www.linkedin.com/in/jeroen-van-de-geest-0b6508241/" name="My github">
                                    Linkedin
                                </Link></td>
                            </tr>
                            <tr>
                                <td className="pr-4 py-1"><FontAwesomeIcon size="lg" icon={faItchIo} /></td>
                                <td><Link link="https://jeroeno-boy.itch.io" name="My itch.io">
                                    Itch.io
                                </Link></td>
                            </tr>
                            <tr>
                                <td className="pr-4 py-1"><FontAwesomeIcon size="lg" icon={faGithub} /></td>
                                <td><Link link="https://github.com/JeroenoBoy" name="My github">
                                    Github
                                </Link></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </section>
    )
}



function Link({ link, className, text, name: _name, children }: { link: string, className?: string, text?: string, name: string, children?: ReactNode }) {
    text ??= link

    if (link.startsWith('http')) {
        return (
            <a className="link" href={link} target='_blank' rel='noopener noreferrer' aria-label={_name}>
                {children ?? text}
            </a>
        )
    }

    return (
        <NLink href={link}>
            <a className="link" aria-label={_name}>
                {children ?? text}
            </a>
        </NLink>
    )
}
