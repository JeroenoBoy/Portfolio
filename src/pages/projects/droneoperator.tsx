import React from 'react';
import Project from "../../components/projects/Project";
import ProjectHeader from "../../components/projects/Header";
import Badge from "../../components/Badge";
import ProjectSidebar, {ProjectSidebarItem} from "../../components/projects/Side";
import ProjectSection from "../../components/projects/Section";
import Image, {Video} from '../../components/Image';
import styles from '../../styles/components/section.module.scss';
import Head from "next/head";
import Codeblock from "../../components/projects/Codeblock";
import NImage from 'next/image'
import ProjectTeamSection, {ProjectTeamMember} from "../../components/projects/TeamSection";

export default function Lastpipe() {
    const dir = '/projects/droneoperator';

    return (
        <Project>
            <ProjectHeader>
                <Head>
                    <title>Drone Operator | Jeroen VDG</title>
                    <meta name="description" content="Defend your convoy against the enemies, as a drone operator! This game was made in 8 weeks and released on Itch.io"/>
                </Head>

                <h1>Drone Operator</h1>
                <div className="flex gap-1">
                    <Badge type="C#" />
                    <Badge type="Unity" />
                    <Badge type="VFX" />
                    <Badge type="School" />
                    <Badge type="Group" />
                </div>
                <p>
                    Drone operator is a first person shooter game, where you play as a drone operator. You have to defend your convoy against the enemies. This game was made in 8 weeks and released on Itch.io. The game was made in a team of 8 people and I was the scrum master.
                </p>
            </ProjectHeader>
            <ProjectSidebar>
                <Video src={dir+'/video.mp4'}/>

                <ProjectSidebarItem title='Started'  value='November 5th 2022' />
                <ProjectSidebarItem title='Time'     value='8 weeks' />
                <ProjectSidebarItem title='Language' value='C#' />
                <ProjectSidebarItem title='Engine'   value='Unity' />
            </ProjectSidebar>
            <ProjectTeamSection>
                <ProjectTeamMember role='Developer' name='Jeroen van de Geest' task='Scrum Master, VFX, Convoy, Editor Tools' />
                <ProjectTeamMember role='Developer' name='Onne Krol' task='Ammo, Drone Shooting, Glitch shader' />
                <ProjectTeamMember role='Developer' name='Scott Gerretsen' task='Camera juice, Tank AI, UI Programming' />
                <ProjectTeamMember role='Artist' name='Dailyn Sastropawiro' task='Level design, Audio, Post Processing' />
                <ProjectTeamMember role='Artist' name='Julian Hoogendoorn' task='Houses, Flats models, Trees' />
                <ProjectTeamMember role='Artist' name='Thobias Meerhof' task='Tank models' />
            </ProjectTeamSection>
            <ProjectSection title='The Convoy' startOpen>
                <p>
                    My main task was creating the convoy, and making it move to the end based on a bezier path. I also made the editor tools for the convoy, so that the level designer could easily create a path for the convoy.
                </p>
                <div className={styles['row']}>
                    <div className={styles['col']}>
                        <p>
                            For the editor tools, I partially followed <a className="colored" target="_blank" rel="noreferrer" href="https://www.youtube.com/playlist?list=PLFt_AvWsXl0d8aDaovNztYf6iTChHzrHP">this tutorial series</a> mainly for how to do the handles withing the Unity Editor. I mostly made the rest of the code myself. The code below is used for editing the handles of the path in the editor.
                        </p>
                        <Codeblock title="Bezier Path Editor" file={`${dir}/code/Convoy_Editor.cs`} language='cs'/>
                    </div>
                    <Video width={260} src={`${dir}/video_convoy_patheditor.mp4`}/>
                </div>
                <div className={styles['row']}>
                    <div className={styles['col']}>
                        <p>
                            Now that we have the path, we can't move the convoy yet since it best wants evenly spaced points. But sampling every .1s on the curve (for example) will give varying results. So we need to sample the curve at a fixed distance. This is done with a method that checks the distance between the last and current point. It first tries at a fixed value, if its not far enough it doubles, if its too far it halves. This is done until the distance is within a certain threshold.
                        </p>
                        <Codeblock title="Bezier Path Sampler" file={`${dir}/code/Convoy_Sampler.cs`} language='cs'/>
                    </div>
                    <Image width={1200} height={1440} src={`${dir}/screenshot_convoy_equalPoints.png`}/>
                </div>
                <ProjectSection title='Generating Road Mesh'>
                    <p>
                        The road meshes are automatically generated based on the equally spaced points, 2 quads are generated between each point. Then the uv's gets properly set so the texture doesn't have to repeat and cause a big difference on how different roads look.
                    </p>
                    <Codeblock title="Road Generator" file={`${dir}/code/RoadGenerator.cs`} language="cs" />
                </ProjectSection>
                <div className={styles['row']}>
                    <div className={styles['col']}>
                        <p>
                            So, How does the convoy move along the path? Well that is quite simple, we just move the transform, and if its close enough to the next point, it moves to the next point. It also checks the distance between this unit, and the one in front of it. if its too close, it will slow down, and if its too far, it will speed up.
                        </p>
                        <Codeblock title="Convoy Movement" file={`${dir}/code/Convoy_Move.cs`} language='cs'/>
                    </div>
                    <Video width={260} src={`${dir}/video_convoy_move.mp4`}/>
                </div>
                <ProjectSection title='Tank Shooting'>
                    <div className={styles['row']}>
                        <div className={styles['col']}>
                            <p>
                                There are multiple scripts required for the turret to work, First off we have the turret vision script. This script looks for all enemies within a sphere collider configured as a trigger, when an enemy enters the trigger, it will be added to a list. When an enemy leaves the trigger, it will be removed from the list. The turret will then find the closest enemy it has a direct line of fire with, and set that at its target.
                            </p>
                            <Codeblock title="Finding Closest Visible Target" file={`${dir}/code/Convoy_Vision.cs`} language="cs" />
                        </div>
                        <Video width={260} src={`${dir}/video_convoy_aiming.mp4`}/>
                    </div>
                    <div className={styles['row']}>
                        <div className={styles['col']}>
                            <p>
                                The Turret will then rotate towards the target, and shoot a projectile at it when its directly infront of the barrel. To make the game a bit more balanced, there can only be so many shots fired per second. This is getting handled by the ShootManager, which will keep track of the last time a shot was fired, and if the turret is allowed to shoot again. It also keeps tokens for each turret, this makes sure all tanks can fire a shot, and don't have to worry about only a couple of tanks firing.
                            </p>
                            <Codeblock title="Shoot Manager" file={`${dir}/code/ShootManager.cs`} language="cs" />
                        </div>
                        <Video width={260} src={`${dir}/video_shootmanager.mp4`}/>
                    </div>
                </ProjectSection>
            </ProjectSection>
            <ProjectSection title='VFX' startOpen>
                <p>
                    For this section, I've also got the joy of making the Visual Effects, For this I used Unity's VFX Graph. I made a few different effects, like the explosion, smoke & sparks.
                </p>
                <div className={styles['row']}>
                    <p>
                        The explosion had to have a big impact to make the game fun to play, so I had to experiment a lot with how the explosion should look. The final version is made off four parts: The explosion & smoke itself, The flare, The sparks and the debris. Having never used VFX graph before, it took a bit of time to get familiar with it. But I got the hang of it pretty quickly.
                    </p>
                    <Video width={260} src={`${dir}/video_vfx_explosion_small.mp4`}/>
                </div>
                <div className={styles['row']}>
                    <p>
                        The big explosion works a bit different, it has 2 main parts, and many smaller parts that work together to create the explosion. The first 2 parts are the explosion and the smoke itself. One shoots outwards like a donut, and the other upwards in a line. Then there is also just like the previous explosion, a flare, spark and debris.
                    </p>
                    <Video width={260} src={`${dir}/video_vfx_explosion_big.mp4`}/>
                </div>
                <div className={styles['row']}>
                    <p>
                        The last explosion I made is the EMP explosion, This uses the same techniques as the others, with a couple extra: The sparks in the centre are created via GPU events, firstly a random particle spawns at the surface of a imaginary sphere, then that particle emits the spark particle strips which move randomly from that point. The explosion ring is a normal particle that gets emitted as a decal, so its always visible to the player.
                    </p>
                    <Video width={260} src={`${dir}/video_vfx_emp.mp4`}/>
                </div>
            </ProjectSection>
            <ProjectSection title="Exploding Houses" startOpen>
                <div className={styles['row']}>
                    <p>
                        The exploding houses work in quite an interesting way, All house prefabs are set to static, so they are more preformant in the game. But when the house gets destroyed, it gets replaced with a dynamic version of the house, which is a copy of the static version, but with a rigidbody and a mesh collider on all of its individual parts. Thanks to this, the game can still run well with many houses & they give a cool effect when exploding.
                    </p>
                    <Video width={260} src={`${dir}/video_house_1.mp4`}/>
                </div>
                <div className={styles['row']}>
                    <div className={styles['col']}>
                        <p>
                            For creating the houses, I used a custom editor scripts that allows for quick creation of the exploding variant of the house. First, the object gets cleaned up. When the artist created the house prefabs & models, there were many empty GameObjects sitting there without a reason, This button removes them. Then the script creates a collider for the main house, so when you hit a roof, it doesn't just look like you hit a box. Then the last button clones the house to a new object, and adds a rigidbody and meshcollider to each MeshRenderer.
                        </p>
                        <Codeblock title="Exploding House Generator" file={`${dir}/code/House_Editor.cs`} language="cs" />
                    </div>
                    <Image width={1500} height={1500} src={`${dir}/screenshot_house_editor.png`}/>
                </div>
            </ProjectSection>
        </Project>
    );
}