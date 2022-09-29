$(document).ready(function () {
    let items = document.getElementById("carouselSchedule").querySelectorAll('.carousel .carousel-item');
    items.forEach((el, i) => {
        if (i == 0) {
            $(el).addClass("active");
        }

        const minPerSlide = 2
        let next = el.nextElementSibling
        for (var i = 1; i < minPerSlide; i++) {
            if (!next) {
                // wrap carousel by using first child
                next = items[0]
            }
            let cloneChild = next.cloneNode(true)
            el.appendChild(cloneChild.children[0])
            next = next.nextElementSibling
        }
    })
});
