import Project from "../../components/projects/Project";
import ProjectHeader from "../../components/projects/Header";
import ProjectSection from "../../components/projects/Section";
import ProjectSidebar, {ProjectSidebarItem} from "../../components/projects/Side";
import NImage from 'next/image'
import styles from '../../styles/components/section.module.scss';
import Codeblock from "../../components/projects/Codeblock";
import ProjectTeamSection, {ProjectTeamMember} from "../../components/projects/TeamSection";
import Badge from "../../components/Badge";
import Image, {Video} from "../../components/Image";
import {IconButton} from "../../components/Button";
import { faItchIo } from "@fortawesome/free-brands-svg-icons";
import Head from "next/head";
import React from "react";

export default function Nebula() {
    const dir = '/projects/nebula';

    return (
        <Project>
            <ProjectHeader>
                <Head>
                    <title>Nebula: Lost Friend | Jeroen VDG</title>
                    <meta name="description" content="Fly through a beautiful galaxy and try to find your lost friend. This beautiful game was made in 3 days for the IEGJ 2022 gamejam."/>
                </Head>

                <h1>Nebula: Lost Friend</h1>
                <div className="flex gap-1">
                    <Badge type="C#"/>
                    <Badge type="Unity"/>
                    <Badge type="School"/>
                    <Badge type="GameJam"/>
                    <Badge type="Group"/>
                </div>
                <p>
                    This game was made for the IEGJ Gamejam 2022, I was in a group of 5 other people. We made a beautiful game around the theme "Signal". The goal is to to find the parts of your lost friends space ship. Players can use the radar to retrieve signals of the ship parts & boost these signals with the satellites.
                </p>
            </ProjectHeader>

            <ProjectSidebar>
                <Video autoPlay controls muted  src={dir+'/video.mp4'}/>

                <ProjectSidebarItem title='Started'  value='Feb 21st 2022' />
                <ProjectSidebarItem title='Time'     value='3 Days' />
                <ProjectSidebarItem title='Language' value='C#' />
                <ProjectSidebarItem title='Engine'   value='Unity' />

                <div className="flex gap-1 flex-wrap my-3">
                    <IconButton color="white" icon={faItchIo} to="https://nieke-dijst.itch.io/nebula-lost-friend"/>
                </div>
            </ProjectSidebar>


            <ProjectTeamSection>
                <ProjectTeamMember role='Developer' name='Jeroen G' task='Signal, Parallax & Radio' />
                <ProjectTeamMember role='Artist' name='Nieke D' task='Background, Player & Animations' />
                <ProjectTeamMember role='Developer' name='Tim G' task='Movement & Game loop' />
                <ProjectTeamMember role='Artist' name='Toni K' task='Story & Artwork for intro / outro' />
                <ProjectTeamMember role='Artist' name='Vilma G' task='Rocks & Radio' />
            </ProjectTeamSection>


            <ProjectSection title='Radar'>
                <div className={styles['row-rev']}>
                    <div>
                        <p>
                            For the radar mechanic, I am casting a circle around the player, Then I check for a certain angle. This way I remove the need to do a bunch of raycasts.
                        </p>
                        <p>
                            For the repeaters, when they recieve a signal, They simply check the angle from the player to the repeater. Then it casts its signal in that direction.
                        </p>
                        <div className='flex gap-2 flex-wrap'>
                            <Codeblock title="SignalEmitter"  file={dir+'/code/SignalEmitter.cs'} language="cs" />
                            <Codeblock title="SignalSource" file={dir+'/code/SignalSource.cs'} language="cs" />
                            <Codeblock title="SignalController" file={dir+'/code/SignalController.cs'} language="cs" />
                        </div>
                    </div>
                    <div>
                        <Image src={dir+'/radar.png'} width="800" height="606" alt="Radar" className="rounded object-cover"/>
                    </div>
                </div>
            </ProjectSection>


            <ProjectSection title="Parallax">
                <div className={styles['row-rev']}>
                    <div>
                        <p>
                            The parallax system gives the player a feeling of being in the game. There are 2 main scripts that are used. The first is for the background, It loops when its no longer visible. The second parallax script objects in the background, like planets. The scripts together give the player a lot of immersion.
                        </p>
                    </div>
                    <div>
                        <Video autoPlay controls muted src={dir+'/parallax.mp4'}/>
                    </div>
                        {/*<iframe src={dir+'/parallax.mp4'} allowFullScreen className="object-center"/>*/}
                </div>
                <div className='flex gap-2 flex-wrap pt-4'>
                    <Codeblock title="RepeatingParallax" file={dir+'/code/RepeatingParallax.cs'} language="cs" />
                    <Codeblock title="StationaryParallax" file={dir+'/code/StationaryParallax.cs'} language="cs" />
                </div>
            </ProjectSection>

            <ProjectSection title="Result">
                <div className={styles['row-rev']}>
                    <div>
                        <p>
                            I think the game is quite fun and became very beautiful. Especially for our 3 days of development. Our game got 1st place overall in the gamejam. We also got 1st place in theme, audio & story.
                        </p>
                    </div>

                    <div>
                        <Image src={dir+'/gamejam.png'} width="1000" height="400" alt="Radar" className="rounded object-cover"/>
                    </div>
                </div>
            </ProjectSection>
        </Project>
    )
}