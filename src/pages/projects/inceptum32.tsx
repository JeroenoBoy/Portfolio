import ProjectHeader from "../../components/projects/Header";
import Project from "../../components/projects/Project";
import Head from "next/head";
import Badge from "../../components/Badge";
import ProjectSidebar, {ProjectSidebarItem} from "../../components/projects/Side";
import {Video} from "../../components/Image";
import React from "react";
import ProjectSection from "../../components/projects/Section";
import ProjectTeamSection, {ProjectTeamMember} from "../../components/projects/TeamSection";
import styles from '../../styles/components/section.module.scss';
import Codeblock from "../../components/projects/Codeblock";

export default function Inceptum32() {
    const dir = '/projects/inceptum32'

    return (
        <Project>
            <ProjectHeader>
                <Head>
                    <title>Inceptum 32 | Jeroen VDG</title>
                    <meta name="description" content="In this puzzle game, you play as a human that can solve puzzles by switching between a human and a slime, each with their own unique abilities."/>
                </Head>

                <h1>Inceptum 32</h1>
                <div className="flex gap-1">
                    <Badge type={'C#'} />
                    <Badge type={'Unity'} />
                    <Badge type={'School'} />
                    <Badge type={'Group'} />
                    <Badge type={'Puzzle'} />
                </div>
                <p>
                    Inceptum 32 is a puzzle game where you play as a human that can solve puzzles by switching between a human and a slime, each with their own unique abilities. I mainly worked on the player and its abilities. I also did some smaller parts on the UI and game logic.
                </p>
            </ProjectHeader>
            <ProjectSidebar>
                <Video src={dir+'/video.mp4'}/>

                <ProjectSidebarItem title='Started'  value='March 14th 2022' />
                <ProjectSidebarItem title='Time'     value='8 weeks' />
                <ProjectSidebarItem title='Language' value='C#' />
                <ProjectSidebarItem title='Engine'   value='Unity' />
            </ProjectSidebar>
            <ProjectTeamSection title="Team">
                <ProjectTeamMember role='Developer' name='Jeroen G'/>
                <ProjectTeamMember role='Artist' name='Boy B'/>
                <ProjectTeamMember role='Developer' name='Kellin S'/>
                <ProjectTeamMember role='Artist' name='Luuk S'/>
            </ProjectTeamSection>
            <ProjectSection title="Player Controller">
                <div className={styles['row']}>
                    <div className={styles['col'] + ' flex-1'}>
                        <p>
                            The PlayerController was for me the most difficult part of this project, it was required to have good and smooth feeling movement, while also being able to walk on walls & upside down.
                        </p>
                        <p>
                            I achieved this by using a character controller & doing some math with quaternions for the rotations. I handle the velocity in 2 separate functions, one for movement, and the other for gravity. They both get rotated and affected based on the rotation of the ground / climbable wall.
                        </p>
                        <p>
                            The rotation gets send in from the WallStick component, this component checks if the player is standing on a wall with the ClimbableWall component, then it grabs the normal of the component and sends in the rotation.
                        </p>

                        <div className={styles['row'] + ' flex-wrap'}>
                            <Codeblock title="PlayerMovement" file={dir+'/code/PlayerMovement.cs'} language="cs" />
                            <Codeblock title="WallStick" file={dir+'/code/WallStick.cs'} language="cs" />
                            <Codeblock title="ClimbableWall" file={dir+'/code/ClimbableWall.cs'} language="cs" />
                        </div>
                    </div>

                    <div className={styles['col'] + ' w-64'}>
                        <Video src={dir+'/movement.mp4'}/>
                    </div>
                </div>
            </ProjectSection>
            <ProjectSection title="State Controller">
                <div className={styles['row']}>
                    <div className={styles['col'] + ' flex-1'}>
                        <p>
                            The StateController is what allows the player to switch between the human and the slime. It enabled / disabled abilities and sets the values in PlayerMovement. It also activates the animation when switching states.
                        </p>

                        <div className={styles['row'] + ' flex-wrap'}>
                            <Codeblock title="StateController" file={dir+'/code/StateController.cs'} language="cs" />
                        </div>
                    </div>

                    <div className={styles['col'] + ' w-64'}>
                        <Video src={dir+'/statecontroller.mp4'}/>
                    </div>
                </div>
            </ProjectSection>
            <ProjectSection title="Rotate Chaotically">
                <div className={styles['row']}>
                    <div className={styles['col'] + ' flex-1'}>
                        <p>
                            At the end of the game, the last room starts rotating chaotically. This rotation is done with multiple types of rotation types, which allows precise control over the rotation.
                        </p>

                        <table>
                            <tbody>
                                <tr>
                                    <td className="font-bold pr-2 align-text-top">Accelerate</td>
                                    <td>Accelerate towards a certain speed over time.</td>
                                </tr>
                                <tr>
                                    <td className="font-bold pr-2">Constant</td>
                                    <td>Constantly rotate a number of times.</td>
                                </tr>
                                <tr>
                                    <td className="font-bold pr-2">StopAtRotation</td>
                                    <td>Stop at a exact rotation.</td>
                                </tr>
                            </tbody>
                        </table>

                        <div className={styles['row'] + ' flex-wrap'}>
                            <Codeblock title="RotateChaotically" file={dir+'/code/RotateChaotically.cs'} language="cs" />
                        </div>
                    </div>

                    <div className={styles['col'] + ' w-64'}>
                        <Video src={dir+'/RotateChaotically.mp4'}/>
                    </div>
                </div>
            </ProjectSection>
        </Project>
    )
}