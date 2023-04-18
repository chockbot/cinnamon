const customerProfile = {};

customerProfile.init = () => {
  const activitySwiper = new Swiper(".activitySwiper", {
    slidesPerView: "auto",
    spaceBetween: 30,
  });
  const reviewSwiper = new Swiper(".experience-review-swiper", {
    slidesPerView: "auto",
    spaceBetween: 30,
  });
  const recommendedSwiper = new Swiper(".recommendedSwiper", {
    slidesPerView: "auto",
    spaceBetween: 30,
  });
  const profileTabSwiper = new Swiper(".personal-tab-swiper", {
    slidesPerView: "auto",
    spaceBetween: 0,
  });
};

export default customerProfile;
