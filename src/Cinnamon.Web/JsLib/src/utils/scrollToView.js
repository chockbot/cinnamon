export default function scrollToView(selector) {
  const ele = document.querySelector(selector);
  if (ele) {
    ele.scrollIntoView({
      behavior: "smooth",
      block: "center",
    });
  }
}
