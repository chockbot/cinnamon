import { track } from './mixpanel_lib';

export function TestJS() {
    return 'Hello World';
}

export function Track(event) {
    return track(event);
}