import {PropsWithChildren, useEffect, useState} from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCode, faXmark, faSpinner } from "@fortawesome/free-solid-svg-icons";
import styles from '../../styles/components/projects/Codeblock.module.scss'
import Center from "../alignment/Center";
import Button from "../Button";
import CodeMirror from '@uiw/react-codemirror';
import {csharp} from "@codemirror/legacy-modes/mode/clike";
import {StreamLanguage} from "@codemirror/stream-parser";


export default function Codeblock({ title, file, language, children }: PropsWithChildren<{ title: string, file: string, language: 'cs'}>) {
    const [ open, setOpen ] = useState(false);
    const [ code, setCode ] = useState<string>(null);
    const [ lang, setLang ] = useState<StreamLanguage<any> | null>(null);


    useEffect(() => {
        fetch(file)
            .then(res => res.text())
            .then(text => setCode(text));
    }, [file, code])


    useEffect(() => {
        setLang({
            'cs': StreamLanguage.define(csharp)
        }[language]);
    }, [language]);


    return (
        <div className="flex-1">
            <Button color="secondary" className={styles.button} onClick={() => setOpen(true)}>
                <FontAwesomeIcon icon={faCode}/>
                <span className='text'>{title}</span>
            </Button>

            {open &&
                <Center className='fixed top-0 right-0 p-4 w-screen h-screen bg-black bg-opacity-50 z-50'>
                    <div className="p-1 rounded overflow-hidden h-full w-full max-w-5xl" style={{background: '#282c34'}}>
                        {code && lang
                            ?
                            <CodeMirror
                                value={code}
                                height="100%"
                                className={styles.block}
                                theme={'dark'}
                                readOnly
                                extensions={[lang]}
                            />
                            :
                            <FontAwesomeIcon icon={faSpinner} className='fa-spin' />
                        }
                    </div>
                    <Center
                        className="fixed top-3 right-3 rounded-full p-6 w-5 h-5 bg-red-500 hover:bg-red-600 active:bg-red-700 cursor-pointer transition-color duration-150 text-white"
                        onClick={() => setOpen(false)}
                    >
                        <FontAwesomeIcon size='2x' icon={faXmark} />
                    </Center>
                </Center>
            }
        </div>
    )
}