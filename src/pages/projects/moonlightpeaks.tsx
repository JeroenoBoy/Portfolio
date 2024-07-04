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
import {IconButton} from "../../components/Button";
import {faItchIo, faSteam} from "@fortawesome/free-brands-svg-icons";
import {faBuilding, faGlobe} from "@fortawesome/free-solid-svg-icons";

export default function MoonlightPeaks() {
    const dir = '/projects/moonlightpeaks';

    return (
        <Project>
            <ProjectHeader>
                <Head>
                    <title>Moonlight Peaks | Jeroen VDG</title>
                    <meta name="description" content="Live the vampire life in the magical town of Moonlight Peaks! Master the art of potions and spells, manage your supernatural farm, and leave your mark on the magical town. Make friends with the local werewolves, witches, and mermaids, and find your eternal love in the supernatural dating scene."/>
                </Head>

                <h1>Moonlight Peaks</h1>
                <div className="flex gap-1">
                    <Badge type="Professional" />
                    <Badge type="Internship" />
                    <Badge type="C#" />
                    <Badge type="Unity" />
                </div>
                <p>
                    Experience life as a vampire in Moonlight Peaks! Master the art of potion-making and spell-casting,
                    tend to your supernatural farm, and make your mark on the magical town. Befriend the local werewolves,
                    witches and mermaids, and maybe even find eternal love along the way.
                </p>
                <p>
                    During my internship at Little Chicken Game Company, I have created many features and written some
                    of the core systems of the game. Think of the spell casting, decorating, critters, the interactions
                    system, the hellcat and much more.
                </p>
            </ProjectHeader>
            <ProjectSidebar>
                <Video src={dir + '/video.mp4'} className="mb-2"/>

                <ProjectSidebarItem title='Company' value='Little Chicken'/>
                <ProjectSidebarItem title='Started' value='January 31st 2023'/>
                <ProjectSidebarItem title='Time' value='10 Months'/>
                <ProjectSidebarItem title='Language' value='C#'/>
                <ProjectSidebarItem title='Engine' value='Unity'/>

                <div className="flex gap-1 flex-wrap my-3">
                    <IconButton color="white" icon={faBuilding} to="https://www.littlechicken.nl/"/>
                    <IconButton color="white" icon={faGlobe} to="https://www.moonlightpeaks.com/"/>
                    <IconButton color="white" icon={faSteam} to="https://store.steampowered.com/app/2209900/Moonlight_Peaks"/>
                </div>
            </ProjectSidebar>
        </Project>
    );
}