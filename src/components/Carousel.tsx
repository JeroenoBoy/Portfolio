import {Component, DetailedHTMLProps, HTMLAttributes, useEffect, useReducer, useState} from "react";
import clsx from "clsx";
import {faArrowLeft, faArrowRight} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import styles from '../styles/components/carousel.module.css'
import NImage from 'next/image';


interface CarouselProps extends DetailedHTMLProps<HTMLAttributes<HTMLDivElement>, HTMLDivElement>{
    images: string[];
    stayFor: number;
    duration: number;
}


interface CarouselState {
    currentImage: number;
    targetDirections: -1 | 1;

    interval: NodeJS.Timer;
    dimensions: [x: number, y: number]
}


export default class Carousel extends Component<CarouselProps, CarouselState>{
    private container: HTMLDivElement;


    public state: CarouselState = {
        currentImage: 0,
        targetDirections: 1,
        interval: null,
        dimensions: [0,0]
    }


    public loop = () => {
        this.changeImage(this.state.currentImage+1);
    }


    public changeImage = (index: number) => {
        this.setState({
            targetDirections: 1,
            currentImage: index % this.props.images.length,
            dimensions: [this.container.clientWidth, this.container.clientHeight]
        })
    }


    public setImage = (index: number) => {
        this.changeImage(index);
        clearInterval(this.state.interval);
        this.setState({ interval: setInterval(this.loop, this.props.stayFor) });
    }


    public componentDidMount() {
        this.setState({
            interval: setInterval(this.loop, this.props.stayFor),
            dimensions: [this.container.clientWidth, this.container.clientHeight]
        })
    }


    public componentWillUnmount() {
        clearInterval(this.state.interval);
    }


    public render() {
        const { currentImage: current, targetDirections: direction, dimensions } = this.state;
        const { images, className, stayFor, duration, children, ...props } = this.props;

        return (
            <div className={clsx(styles.hero__carousel, className)} {...props} ref={el => this.container = el}>
                <div className={styles['carousel-image_container']}>
                    <div className={styles['carousel-image_parent']} style={{ left: '-' + current*dimensions[0] + 'px' }}>
                        {images.map((i) => <Image key={i} alt={`Carousel - ${i}`} img={i} dimensions={dimensions} />)}
                    </div>
                </div>

                <div className={`${styles['carousel-button']} left-0`} onClick={() => this.setImage(current-1)}>
                    <FontAwesomeIcon icon={faArrowLeft} />
                </div>

                <div className={`${styles['carousel-button']} right-0`} onClick={() => this.setImage(current+1)}>
                    <FontAwesomeIcon icon={faArrowRight} />
                </div>


                <div className={styles['carousel-indexes']}>
                    {images.map((img, i) => ( <div
                            key={i}
                            className={clsx(styles['carousel-index'], {
                                [styles['carousel-index_active']]: i === current
                            })}
                            //  @ts-ignore
                            onClick={() => i !== current && i !== next && this.setImage(i)}
                        />
                    ))}
                </div>

                {children}
            </div>
        )
    }
}


export function Image({ img, alt, dimensions }: { img: string, alt: string, dimensions: [x: number, y: number] }) {
    return (
        <div className={clsx(styles['carousel-image'])} style={{width: dimensions[0], height: dimensions[1]}}>
            <NImage layout="fill" src={img} alt={alt} className="object-cover"/>
        </div>
    )
}