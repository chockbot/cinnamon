export function scrollToView(selector) {
  const ele = document.querySelector(selector);
  if (ele) {
    ele.scrollIntoView({
      behavior: "smooth",
      block: "center",
    });
  }
}

export function scrollToTop() {
  setTimeout(() => {
    window.scrollTo(0, 0);
  }, 100);
}
