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
        },
        observer: true,
        observeParents: true,
        breakpoints: {
            320: {
                slidesPerView: 2,
                spaceBetween: 15
            },
            // when window width is >= 480px
            480: {
                slidesPerView: 2,
                spaceBetween: 30
            },
            // when window width is >= 640px
            640: {
                slidesPerView: 4,
                spaceBetween: 40
            },
            768: {
                slidesPerView: 3,
                spaceBetween: 30,
            },
            1024: {
                slidesPerView: 4,
                spaceBetween: 50,
            },
        },
        loop: true,
    });
}

export default {
    initSwiper,
};
