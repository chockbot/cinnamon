import Swiper from "swiper";

export function initSwiper(selector) {
  console.log("init", selector);
  const swiper = new Swiper(selector, {
    slidesPerView: "auto",
    spaceBetween: 30,
  });
}

export default {
  initSwiper,
};
