export default function previewImage(inputSelector, imgIds) {
  const inputElem = document.querySelector(inputSelector);
  let totalFileSize = 0;
  const urls = [];

  for (const file of inputElem.files) {
    totalFileSize += file.size / 1024 / 1024;
  }
  //imgIds.forEach((s) => loadEmptySrc(s));

  if (totalFileSize > 10) {
    inputElem.value = "";
    return;
  }

  for (let i = 0; i < 3; i++) {
    if (inputElem.files[i] && imgIds[i]) {
      const url = URL.createObjectURL(inputElem.files[i]);
      const filename = inputElem.files[i].name;
      urls.push({ url, filename });
      //loadImage(imgIds[i], url, i, filename);
    }
  }

  return urls;
}

function loadImage(imgSelector, url, index, filename) {
  const el = document.querySelector(imgSelector);
  if (!el) return;

  el.classList.remove("invalid");
  $(el).attr("data-index", index);
  $(el).attr("data-name", filename);
  $(el).attr("data-changed", true);
  el.addEventListener("load", () => URL.revokeObjectURL(url), { once: true });
  el.src = url;
}

function loadEmptySrc(imgSelector) {
  const src =
    "https://i1.wp.com/www.slntechnologies.com/wp-content/uploads/2017/08/ef3-placeholder-image.jpg";

  $(imgSelector).attr("data-index", "-1");
  $(imgSelector).attr("data-name", "");
  $(imgSelector).attr("src", src);
  $(imgSelector).addClass("invalid");
}
