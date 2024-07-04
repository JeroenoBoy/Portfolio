import {useEffect, useState} from "react";

export default function useFetch(link: string) {
    const [data, setData] = useState<string>(null);

    function reFetch() {
        fetch(link)
            .then(res => res.text())
            .then(data => setData(data));
    }

    useEffect(() => {
        reFetch();
    }, []);

    return [data, reFetch]
}