import {MutableRefObject, useEffect, useState} from "react";
import {useValue} from "./usePhone";

const d = 'YW1WeWIyVnVRSFpoYm1SbFoyVmxjM1F1WlhVPQ';

export function useEmail(ref: MutableRefObject<HTMLElement>) {
    const email = 'virtualmafia@jeroenvdg.com'
    return useValue(d, email, ref)
}