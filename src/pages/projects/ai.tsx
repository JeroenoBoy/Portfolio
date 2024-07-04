import React from 'react';
import Project from "../../components/projects/Project";
import ProjectHeader from "../../components/projects/Header";
import Badge from "../../components/Badge";
import ProjectSidebar, {ProjectSidebarItem} from "../../components/projects/Side";
import ProjectSection from "../../components/projects/Section";
import NImage from "next/image";
import Image, {Video} from '../../components/Image';
import styles from '../../styles/components/section.module.scss';
import Codeblock from "../../components/projects/Codeblock";
import ProjectTeamSection, {ProjectTeamMember} from "../../components/projects/TeamSection";
import TTP from '../../components/TTP';
import Head from 'next/head';

export default function Ai() {
    const dir = '/projects/ai';

    return (
        <Project>
            <ProjectHeader>
                <Head>
                    <title>AI Playground | Jeroen VDG</title>
                    <meta name="description" content="Interesting AI Behaviour using the finite state machine & strategy Pattern. This page also features a war & creature simulation."/>
                </Head>

                <h1>AI Playground</h1>
                <div className="flex gap-1">
                    <Badge type="C#" />
                    <Badge type="Unity" />
                    <Badge type="School" />
                    <Badge type="AI" />
                    <Badge type="FSM" />
                    <Badge type="Solo" />
                    <Badge type="Group" />
                </div>
                <p>
                    This is a project about learning and experimenting with AI. I needed to create a bunch of different AI steering behaviours. And at the end, implement them into a game or simulation. For the second part of this Project we worked in groups of three, picked the best steering behaviour (which was mine) and implemented it into a game.
                </p>
            </ProjectHeader>
            <ProjectSidebar>
                <Video src={dir + '/video.mp4'} className="mb-2" />

                <ProjectSidebarItem title='Started'  value='November 29th 2021' />
                <ProjectSidebarItem title='Time'     value='6 weeks' />
                <ProjectSidebarItem title='Language' value='C#' />
                <ProjectSidebarItem title='Engine'   value='Unity' />
            </ProjectSidebar>
            <ProjectSection title="States">
                <div className={styles['row']}>
                    <div className={styles['col']}>
                        <p>
                            For this project, we needed to make a bunch of different steering behaviours, then control them via an enum in a script. But I did it a bit different. About 2 weeks in, I changed my base class to a StateMachineBehaviour. This made it easier for me to create the state machines.
                        </p>
                    </div>
                    <div>
                        <Image width={808} height={398} src={dir+'/behaviour.png'} alt="behaviours" />
                    </div>
                </div>

                <p>
                    When a state first gets created, It will try and get all references it wants from the gameobject it is attached to. Then when it gets activated, it will be added to the StateController. This controller will call FixedUpdate & OnDrawGizmos on all active states.
                </p>

                <div className={styles['row']}>
                    <Codeblock title="AIBehaviour" file={dir+'/code/AIBehaviour.cs'} language="cs" />
                    <Codeblock title="Flocking" file={dir+'/code/Flocking.cs'} language="cs" />
                    <Codeblock title="StateController" file={dir+'/code/StateController.cs'} language="cs" />
                </div>
            </ProjectSection>
            <ProjectSection title="Simulation">
                <h3 className="text-2xl">Ecosystem</h3>
                <div className={styles['row']}>
                    <div className={styles['col']}>
                        <p>
                            To finish off the solo part of this project. I tried to simulate a small ecosystem of about 200 animals. There are 2 types of animals: Rabbit and the Cat. The rabbit survives by eating berries and the cat by eating the rabbits. I think the result is quite interesting to look at.
                        </p>
                    </div>
                    <div>
                        <Video src={dir + '/simulation.mp4'} />
                    </div>
                </div>

                <h3 className="text-2xl">Brains</h3>
                <div className={styles['row']}>
                    <div className={styles['col']}>
                        <p>
                            This script is responsible for sending all the values to the <TTP tooltip="The animator is used as the Finite State Machine">animator</TTP>. There are also a couple of seperate components that are responsible for the movement, hunger, sight, etc. The animator is responsible for managing all the states & animations.
                        </p>
                    </div>
                    <div>
                        <Image width={808} height={398} src={dir+'/brain.png'} alt="fsm - variables & brain" className="rounded-lg"/>
                    </div>
                </div>
                <div className={styles['row']}>
                    <Codeblock title="AnimalBrain" file={dir+'/code/AnimalBrain.cs'} language="cs" />
                    <Codeblock title="BaseBrain" file={dir+'/code/BaseBrain.cs'} language="cs" />
                </div>
            </ProjectSection>
            <ProjectSection title="Group - Castle Siege">
                <ProjectTeamSection>
                    <ProjectTeamMember role="Developer" name="Jari"   task="UI, unit stats" />
                    <ProjectTeamMember role="Developer" name="Jeroen" task="Unit AI, Enemy AI" />
                    <ProjectTeamMember role="Developer" name="Joep"   task="Unit Spawning" />
                </ProjectTeamSection>

                <h3 className="text-2xl">Castle Sige</h3>
                <div className={styles['row']}>
                    <div className={styles['col']}>
                        <p>
                            For this exercise, we worked on a small castle siege game. The player could spawn units with stats that add up to 100% total, and attack the enemy castle. The enemy would spawn units with random stats, and attack the player's castle. Units had 6 behaviours, which would determine what they would do. The player would have to kill the enemy castle to win.
                        </p>
                    </div>
                    <div>
                        <Video src={dir + '/video.mp4'} />
                    </div>
                </div>

                <h3 className="text-2xl">Behaviours</h3>
                <div className={styles['row']}>
                    <div className={styles['col']}>
                        <p>
                            For Castle Siege, we added a few different behaviours. Some for attacking, others for walking towards different points on the map. For the AI I Inherited the BaseBrain and extended it so it could see where the enemy castle is, the healing & damage ring, etc.
                        </p>
                    </div>
                    <div>
                        <Image alt="The unit FSM" src={dir+'/thegreatfsm.png'} width={1450} height={750} />
                    </div>
                </div>
                <div className={styles['row']}>
                    <Codeblock title="UnitBrain" file={dir+'/code/UnitBrain.cs'} language="cs" />
                    <Codeblock title="AttackBehaviour" file={dir+'/code/AttackBehaviour.cs'} language="cs" />
                </div>
            </ProjectSection>
        </Project>
    );
}