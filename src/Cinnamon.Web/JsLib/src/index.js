import { track } from "./mixpanel_lib";
import pages from "./Pages";

export function TestJS() {
  return "Hello World";
}

export function Track(event) {
  return track(event);
}

export const Pages = pages;
