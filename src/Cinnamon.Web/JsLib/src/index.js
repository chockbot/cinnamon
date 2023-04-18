import { track } from "./mixpanel_lib";
import pages from "./Pages";
import components from "./Components";
import account from "./Account";

export function Track(event) {
  return track(event);
}

export function previewImage(inputElem, imgElemId) {
  const url = URL.createObjectURL(inputElem.files[0]);
  const el = document.getElementById(imgElemId);
  if (!el) return;

  el.addEventListener("load", () => URL.revokeObjectURL(url), {
    once: true,
  });
  el.src = url;
}

export const Pages = pages;

export const Components = components;

export const Account = account;
