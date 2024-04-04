export default {
  initSwiper: (selector) => {
    const swiper = new Swiper(selector, {
      loop: true,
      autoplay: {
        delay: 3000,
        disableOnInteraction: false,
      },
    });
    console.log("swiper initialized");
  },
};
