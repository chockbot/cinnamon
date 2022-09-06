import mixpanel from 'mixpanel-browser';

export function track(event) {
    // This is the Mixpanel Cinnamon DEV Project
    mixpanel.init('7d44367dd73a14d76f134da20250dcb5', { debug: true });
    mixpanel.track(event); 
}