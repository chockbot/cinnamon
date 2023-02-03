import Swiper from "swiper";

export function initSwiper(selector, scrollbarSelector) {
  console.log("init", selector);
  const swiper = new Swiper(selector, {
    slidesPerView: "auto",
    spaceBetween: 30,
    grabCursor: true,
    keyboard: {
      enabled: true,
    },
    scrollbar: {
      el: scrollbarSelector,
    },
  });
}

export default {
  initSwiper,
};
