export function initSwiper(selector, scrollbarSelector) {
  const swiper = new Swiper(selector, {
    slidesPerView: "auto",
    spaceBetween: 30,
    grabCursor: true,
    keyboard: {
      enabled: true,
    },
    scrollbar: {
      el: scrollbarSelector,
      hide: false,
    },
  });
}

export function initSwiperMobile(selector, scrollbarSelector) {
  const swiper = new Swiper(selector, {
    slidesPerView: "auto",
    spaceBetween: 30,
    grabCursor: true,
    keyboard: {
      enabled: true,
    },
    scrollbar: {
      el: scrollbarSelector,
      hide: false,
    },
  });
}

export function initSwiperHelpPage(selector) {
  const swiper = new Swiper(selector, {
    slidesPerView: "auto",
    spaceBetween: 30,
    grabCursor: true,
    keyboard: {
      enabled: true,
    },
    scrollbar: false,
    observer: true,
    observeParents: true,
  });
}

export function initCircularProgress(selector, percentage) {
  $(selector).each(function () {
    const left = $(this).find(".progress-left .progress-bar");
    const right = $(this).find(".progress-right .progress-bar");

    if (percentage <= 50) {
      right.css(
        "transform",
        "rotate(" + percentageToDegrees(percentage) + "deg)"
      );
      if (percentage === 0) {
        left.css(
          "transform",
          "rotate(" + percentageToDegrees(percentage) + "deg)"
        );
      }
    } else {
      right.css("transform", "rotate(180deg)");
      left.css(
        "transform",
        "rotate(" + percentageToDegrees(percentage - 50) + "deg)"
      );
      if (percentage > 100) {
        right.css("transform", "rotate(180deg)");
        left.css("transform", "rotate(" + percentageToDegrees(50) + "deg)");
      }
    }
  });

  function percentageToDegrees(percentage) {
    return (percentage / 100) * 360;
  }
}

export default {
  initSwiper,
  initSwiperHelpPage,
  initSwiperMobile,
  initCircularProgress,
};
