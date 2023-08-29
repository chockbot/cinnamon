const imageCropper = {};

imageCropper._initImage = (
  imgInitSelector,
  imgSrcSelector,
  fileSrcSelector,
  fileIndex
) => {
  const imgInit = document.querySelector(imgInitSelector);
  return new Promise((resolve, reject) => {
    try {
      if (fileSrcSelector && fileIndex) {
        const el = document.querySelector(fileSrcSelector);
        const url = URL.createObjectURL(el.files[fileIndex]);
        imgInit.addEventListener(
          "load",
          () => {
            URL.revokeObjectURL(url);
            return resolve();
          },
          {
            once: true,
          }
        );
        imgInit.src = url;
      }
      if (imgSrcSelector) {
        const el = document.querySelector(imgSrcSelector);
        imgInit.addEventListener("load", () => resolve());
        imgInit.src = el.src;
      }
    } catch (error) {
      reject(error);
    }
  });
};

imageCropper.init = (previewSelector, imgSrcSelector, fileSrcSelector) => {
  try {
  } catch (error) {}
};

export default imageCropper;
