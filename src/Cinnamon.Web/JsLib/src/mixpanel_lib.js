import mixpanel from 'mixpanel-browser';

export function track(event) {
    const mixPanelProjectKey = process.env.mixPanelProjectKey || '7d44367dd73a14d76f134da20250dcb5';
    // This is the Mixpanel Cinnamon DEV Project
    mixpanel.init(mixPanelProjectKey, { debug: true });
    mixpanel.track(event);
}