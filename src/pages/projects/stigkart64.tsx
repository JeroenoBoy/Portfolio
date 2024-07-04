import Project from "../../components/projects/Project";
import ProjectHeader from "../../components/projects/Header";
import ProjectSection from "../../components/projects/Section";
import ProjectSidebar, {ProjectSidebarItem} from "../../components/projects/Side";
import styles from '../../styles/components/section.module.scss';
import Codeblock from "../../components/projects/Codeblock";
import ProjectTeamSection, {ProjectTeamMember} from "../../components/projects/TeamSection";
import Badge from "../../components/Badge";
import Image from "../../components/Image";
import {IconButton} from "../../components/Button";
import {faItchIo} from "@fortawesome/free-brands-svg-icons";
import Head from "next/head";
import React from "react";

export default function Nebulalostfriend() {
    const dir = '/projects/stigkart64';

    return (
        <Project>
            <ProjectHeader>
                <Head>
                    <title>Stig Kart 64 | Jeroen VDG</title>
                    <meta name="description" content="Race against AI or your friends with split screen. This retro styled game was made in 2 weeks with 3 artists and 3 developers."/>
                </Head>

                <h1>Bottom Gear: Stig Kart 64</h1>
                <div className="flex gap-1">
                    <Badge type="C#"/>
                    <Badge type="Unity"/>
                    <Badge type="School"/>
                    <Badge type="Split-screen"/>
                    <Badge type="Group"/>
                </div>
                <p>
                    For this project, we needed to make a Night Racer. We decided to make a game that looks like a Nintendo 64 game, inspired by Diddy Kong Racing. For the first half of the project, I was responsible for the checkpoints system & the directions system. For the second half of the project I was responsible for the Split-screen system.
                </p>
            </ProjectHeader>

            <ProjectSidebar>
                {/*<NImage width="235" height="145" src='/projects/nebula/logo.png' alt='Nebula: Lost Friend - logo' className="rounded" layout="responsive" />*/}
                <div className="rounded overflow-hidden">
                    <iframe width="235" height="145" src="https://www.youtube.com/embed/0gehGW9LS58" allowFullScreen className="w-full h-auto aspect-video" />
                </div>

                <ProjectSidebarItem title='Started'  value='Jan 18th 2022' />
                <ProjectSidebarItem title='Time'     value='2 Weeks' />
                <ProjectSidebarItem title='Language' value='C#' />
                <ProjectSidebarItem title='Engine'   value='Unity' />

                <div className={styles['row'] + ' py-2'}>
                    <IconButton color="white" icon={faItchIo} to="https://aafke.itch.io/bottom-gear-stig" />
                </div>
            </ProjectSidebar>


            <ProjectTeamSection>
                <ProjectTeamMember role='Artist' name='Aafke R' task='Environment & Props' />
                <ProjectTeamMember role='Artist' name='Dave B' task='Car & Props' />
                <ProjectTeamMember role='Artist' name='Dailyn S' task='Audio, UI & Props' />
                <ProjectTeamMember role='Developer' name='Jeroen G' task='Checkpoints, Split-screen & Game loop' />
                <ProjectTeamMember role='Developer' name='Joep V' task='UI & Game loop' />
                <ProjectTeamMember role='Developer' name='Scott G' task='Vehicle controller & AI' />
            </ProjectTeamSection>


            <ProjectSection title='Checkpoints'>
                <div className={styles['row-rev']}>
                    <div>
                        <p>
                            The checkpoint system had multiple nodes. The player had to follow the checkpoints in the correct order. If the player skipped a checkpoint the player would be send back to the previous one. I also gave the checkpoints a bunch of gizmos to make development easier.
                        </p>
                        <div className='flex gap-2 flex-wrap'>
                            <Codeblock title="LapsManager" file={dir+'/code/LapsManager.cs'} language="cs" />
                            <Codeblock title="Checkpoint" file={dir+'/code/Checkpoint.cs'} language="cs" />
                            <Codeblock title="CheckpointTracker" file={dir+'/code/CheckpointTracker.cs'} language="cs" />
                        </div>
                    </div>
                    <div>
                        <Image src={dir+'/checkpoints.png'} width="800" height="486" alt="Checkpoint gizmos" className="rounded object-cover"/>
                    </div>
                </div>
            </ProjectSection>


            <ProjectSection title="Split-screen">
                <h3>Managers</h3>
                <p>
                    The managers make sure that all the inputs are given correctly to the player. The base class, ScenePlayerManager makes sure that all the events are getting bound correctly. It also makes sure everything is cleaned properly.
                </p>
                <Codeblock title="ScenePlayerManager" file={dir+'/code/ScenePlayerManager.cs'} language="cs" />
                <div className={styles['row']}>
                    <div className={styles.col+" flex-1"}>
                        <Image src={dir+'/join.png'} alt="Collect Players image" width="800" height="451" className="rounded object-cover"/>
                        <h4>CollectPlayerManager</h4>
                        <p>
                            This manager allows for players to join & leave the game. It makes sure that the UI elements are initialized correctly. And it makes sure that the events are bound correctly to the controls.
                        </p>
                    </div>
                    <div className={styles.col+" flex-1"}>
                        <Image src={dir+'/game.png'} alt="Collected players inside of the game" width="800" height="451" className="rounded object-cover" />
                        <h4>GamePlayerManager</h4>
                        <p>
                            This manager makes sure the input controllers are bound correctly to each player. It also makes sure that the gameloop is running correctly.
                        </p>
                    </div>
                </div>
                <div className={styles['row']}>
                        <Codeblock title="CollectPlayerManager" file={dir+'/code/CollectPlayerManager.cs'} language="cs" />
                        <Codeblock title="GamePlayerManager" file={dir+'/code/GamePlayerManager.cs'} language="cs" />
                </div>

                <h3>Player Interface</h3>
                <div className="flex gap-4">
                    <div>
                        <p>
                            I think this script is a bit "Hacky", It binds all the events, components & values to where they need to be. With Flex Tape. But... it does work! This script is also responsible for replacing the player controller with the AI for when the player finishes.
                        </p>
                    </div>
                    <div>
                        <Image src={dir+'/4Player.png'} alt="Shows 4 players playing at the same time" width="800" height="451" className="rounded object-cover" />
                    </div>
                </div>
                <Codeblock title="PlayerInterface" file={dir+'/code/PlayerInterface.cs'} language="cs" />

                <h3>Input Handler</h3>
                <div className={styles.col}>
                    <p>
                        Before we added the split-screen, we used the old input system of unity. This caused tiny problems with migrating. So when we added the new input system, I mimicked the old one a bit.
                    </p>
                    <Codeblock title="InputHandler" file={dir+'/code/InputHandler.cs'} language="cs" />
                </div>
            </ProjectSection>

            <ProjectSection title="Result">
                <div className={styles.col}>
                    <div>
                        <p>
                            I think my team did very well considering the 2 weeks we got. We are especially happy that we got split-screen & ai working within a week.
                        </p>
                    </div>
                </div>
            </ProjectSection>
        </Project>
    )
}