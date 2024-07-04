import {
    createContext,
    MutableRefObject,
    PropsWithChildren,
    ReactElement,
    useContext,
    useEffect,
    useRef,
    useState
} from "react";
import {EventEmitter} from "events";
import {func} from "prop-types";



export interface Section {
    name: string,
    ref: MutableRefObject<HTMLElement>
}



export interface ScrollAgent {
    on(event: 'scrollChange', fn: (prev: Section, next: Section) => void): this;
}



export class ScrollAgent extends EventEmitter {
    private static offset = 0;

    private sections = new Array<Section>();
    private currentSection: Section | undefined;


    public constructor() {
        super();
    }


    public addSection(name: string, element: MutableRefObject<HTMLElement>) {
        this.sections.push({name, ref: element});
    }


    public removeSection(name: string) {
        this.sections = this.sections.filter(section => section.name !== name);
    }


    private next() {
        let current: Section = null;
        let currentLoc = -Infinity;

        const scroll = window.scrollY + ScrollAgent.offset;

        for(const section of this.sections) {
            const loc = (section.ref.current?.offsetTop ?? 0) - window.innerHeight / 3;

            if (loc > scroll) continue;
            if (loc < currentLoc) continue;
            current = section;
            currentLoc = loc;
        }

        return current;
    }


    private _onScroll = () => {
        const current = this.currentSection;
        const next = this.next();

        if (current == next) return;

        this.currentSection = next;
        this.emit('scrollChange', current, next);
    }


    public bind() {
        window.addEventListener('scroll', this._onScroll);
        this._onScroll();
    }


    public dispose() {
        window.removeEventListener('scroll', this._onScroll);
    }
}



const scrollAgentContext = createContext(new ScrollAgent());



export function ScrollAgentProvider({ children }: PropsWithChildren<{}>) {
    const [ value ] = useState(new ScrollAgent());

    useEffect(() => {
        value.bind();
        return () => value.dispose();
    }, [value]);

    return (
        <scrollAgentContext.Provider value={value}>
            {children}
        </scrollAgentContext.Provider>
    )
}



export function useScrollAgent() {
    return useContext(scrollAgentContext);
}



export function useScrollAgentSection(name: string, element: MutableRefObject<HTMLElement>) {
    const agent = useContext(scrollAgentContext);

    useEffect(() => {
        agent.addSection(name, element);
        return () => agent.removeSection(name);
    }, [name, agent]);

    return agent;
}



export function useScrollAgentState(name: string) {
    const agent = useContext(scrollAgentContext);
    const [ isCurrent, setIsCurrent ] = useState(false);

    useEffect(() => {
        function handle(prev: Section, next: Section) {
            setIsCurrent(next.name === name);
        }

        agent.on('scrollChange', handle);
        return () => {agent.off('scrollChange', handle)};
    }, [agent]);

    return isCurrent;
}



export function ScrollAgentMark({ name, children }: { name: string, children: (ref: MutableRefObject<HTMLElement>) => ReactElement}) {
    const agent = useContext(scrollAgentContext);
    const ref = useRef<HTMLElement>(null);

    useEffect(() => {
        agent.addSection(name, ref);
        return () => agent.removeSection(name);
    }, [name, ref, agent]);

    return children(ref);
}
