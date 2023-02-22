const imageHelper = {
    previewSingleImage(imgSelector, url, filename) {
        const el = document.querySelector(imgSelector);
        if (!el) return;

        el.classList.remove("invalid");
        $(el).attr("data-name", filename);
        el.addEventListener("load", () => URL.revokeObjectURL(url), { once: true });
        el.src = url;
    },
    loadEmptySrc(imgSelector) {
        const src =
            "https://i1.wp.com/www.slntechnologies.com/wp-content/uploads/2017/08/ef3-placeholder-image.jpg";

        $(imgSelector).attr("data-index", "-1");
        $(imgSelector).attr("data-name", "");
        $(imgSelector).attr("src", src);
        $(imgSelector).addClass("invalid");
    }
}

export default imageHelper