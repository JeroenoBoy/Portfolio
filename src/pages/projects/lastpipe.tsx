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

export default function Lastpipe() {
    const dir = '/projects/lastpipe';

    return (
        <Project>
            <ProjectHeader>
                <Head>
                    <title>Last Pipe of Defence | Jeroen VDG</title>
                    <meta name="description" content="Defend your base against the intruders with turrets, pipes and extractors. This is a intense prototype made in 4 weeks."/>
                </Head>

                <h1>Last Pipe of Defence</h1>
                <div className="flex gap-1">
                    <Badge type="C#" />
                    <Badge type="Unity" />
                    <Badge type="Maya" />
                    <Badge type="School" />
                    <Badge type="Solo" />
                    <Badge type="Prototype" />
                </div>
                <p>
                    Last Pipe of Defence is Tower-Defense prototype. The goal is to survive as long as possible against waves of enemies. For this project I needed to make a "contract" with the teachers, then when they approved the idea, I started working on the game.
                </p>
            </ProjectHeader>
            <ProjectSidebar>
                <Video src={dir+'/video.mp4'}/>

                <ProjectSidebarItem title='Started'  value='May 25th 2022' />
                <ProjectSidebarItem title='Time'     value='4 weeks' />
                <ProjectSidebarItem title='Language' value='C#' />
                <ProjectSidebarItem title='Engine'   value='Unity' />
            </ProjectSidebar>
            <ProjectSection title="Idea">
                <p>
                    You live on a small island. But suddenly, you see boats in the distance full of intruders. You need to start defending your island by building turrets.
                </p>
                <p>
                    The player can build turrets, pipes and extractors. Turrets use up energy which gets provided via the pipes. Extractors are placed on crystals to provide additional energy. Turrets automatically aim and shoot towards the nearest enemy.
                </p>
                <p>
                    The player can get more buildings by picking a card at the end of each wave. These cards are of different rarity to try and make the game harder.
                </p>
                <p>
                    The player plays on randomly generated islands based on seeds. Each seed will provide the same kind of island, crystals, waves and cards. This is so the player can try again on the same island.
                </p>
            </ProjectSection>
            <ProjectSection title="Pipes System">
               <p>
                   The pipes system is the most important mechanic of the game. It is used to provide energy to turrets. The pipes are placed on the map and are connected to each other. The player has different types of pipes to make the game a bit more challenging.
               </p>
               <p>
                   The pipes system works like this:
                   <ul>
                       <li>
                           When a pipe gets created:
                           <ol>
                               <li>It checks all tiles it can connect to.</li>
                               <li>If it can connect to a tile, it will connect to it.</li>
                               <li>If one connection is powered, the new pipe will also be powered.</li>
                               <li>It will then power all its neighbors if they are unpowered.</li>
                           </ol>
                       </li>
                       <li>
                           When a pipe gets destroyed:
                           <ol>
                               <li>It un-powers all connected turrets.</li>
                               <li>If it can connect to a tile, it will disconnect from it.</li>
                               <li>Then the main base will power all buildings its connected to.</li>
                           </ol>
                       </li>
                   </ul>
               </p>
               <div className={styles['row']}>
                   <Codeblock title="PowerBasedBuilding" file={dir+'/code/PowerBasedBuilding.cs'} language="cs" />
                   <Codeblock title="Pipe" file={dir+'/code/Pipe.cs'} language="cs" />
                   <Codeblock title="PipeManager" file={dir+'/code/PipeManager.cs'} language="cs" />
               </div>
               <div className={styles['row']}>
                   <Image alt="Powered pipes" src={dir+'/powered.png'} width={800} height={400} className="object-cover rounded-lg shadow"/>
                   <Image alt="Unpowered pipes" src={dir+'/unpowered.png'} width={900} height={400} className="object-cover rounded-lg shadow"/>
               </div>
            </ProjectSection>
            <ProjectSection title="Building System">
                <div className={styles['row']}>
                    <div className={styles['col']}>
                        <p>
                            This is the mechanic that forms the base of the game. When the player selects an item from the inventory, it will be selected and a "skeleton" of the item will be placed at the selected tile. When the player clicks on a tile, the item will be placed and removed from the inventory.
                        </p>
                        <p>
                            When the player wants to delete an item. It will be selected the same way as normal items. But there won't be a skeleton made. When the player clicks a tile, the item will be destroyed. There is also a random chance based on the health that the item won't be returned to the inventory.
                        </p>

                        <div className={styles['row']}>
                            <Codeblock title="BuildManager" file={dir+'/code/BuildManager.cs'} language="cs" />
                            <Codeblock title="BuildableObject" file={dir+'/code/BuildableObject.cs'} language="cs" />
                        </div>
                    </div>
                    <div className={styles['col']}>
                        <Video src={dir+'/build.mp4'}/>
                        <Video src={dir+'/destroy.mp4'}/>
                    </div>
                </div>
            </ProjectSection>
            <ProjectSection title="Waves System">
                <p>
                    The waves system spawns enemies based on a difficulty score, this score is based on the amount of waves the player fought. The spawner will spawn templates, each template has its own difficulty, minimum wave and maximum wave to spawn in. The spawner will then spawn the enemies based on the template.
                </p>
                <div className={styles['row']}>
                    <Codeblock title="WaveSpawner" file={dir+'/code/WaveSpawner.cs'} language="cs" />
                    <Codeblock title="WaveTemplate" file={dir+'/code/WaveTemplate.cs'} language="cs" />

                </div>
                <div className={styles['row']}>
                    <NImage alt="Boats" src={dir+'/ships.png'} width={800} height={600} className="object-cover rounded-lg shadow"/>
                    <NImage alt="Enemies" src={dir+'/enemies.png'} width={800} height={600} className="object-cover rounded-lg shadow"/>
                    <NImage alt="Waves" src={dir+'/waves.png'} width={600} height={800} className="object-cover rounded-lg shadow"/>
                </div>
            </ProjectSection>
        </Project>
    );
}