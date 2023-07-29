const activityDetails = {};

activityDetails._init_swiper = () => {
  const swiper = new Swiper(".swiper-details-mobile-images", {
    pagination: {
      el: ".swiper-details-mobile-images .swiper-pagination",
      type: "fraction",
    },
  });
};

activityDetails.init = () => {
  activityDetails._init_swiper();
  console.log("activity details initialized");
};

export default activityDetails;
