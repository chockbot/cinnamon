const imageCropper = {};

class ImageCropper {
  constructor() {
    this.imgSrc = "";
    this.imgContainerSelector = "";
    this.imgSrcSelector = "";
    this.fileSrcSelector = "";
    this.fileIndex = undefined;
    this.cropper = undefined;
    this.outputInputSelector = "";
  }

  async _initImage() {
    const imgInit = document.querySelector(this.imgContainerSelector);
    return new Promise((resolve, reject) => {
      try {
        if (this.imgSrc) {
          imgInit.src = this.imgSrc;
          resolve();
        }
        if (this.fileSrcSelector) {
          const el = document.querySelector(this.fileSrcSelector);
          const fr = new FileReader();
          fr.onload = (event) => {
            imgInit.src = event.target.result;
            resolve();
          };
          fr.readAsDataURL(el.files[this.fileIndex]);
        }
        if (this.imgSrcSelector) {
          const el = document.querySelector(this.imgSrcSelector);
          imgInit.src = el.src;
          resolve();
        }
      } catch (error) {
        console.error(error);
        reject(error);
      }
    });
  }

  async _initCropper() {
    const el = document.querySelector(this.imgContainerSelector);
    return new Promise((resolve, reject) => {
      try {
        if (this.cropper) {
          this.cropper.destroy();
        }
        this.cropper = new Cropper(el, {
          viewMode: 1,
          aspectRatio: 4 / 5,
          minContainerWidth: 350,
          minContainerHeight: 400,
          minCropBoxWidth: 271,
          minCropBoxHeight: 271,
          movable: true,
        });
        resolve();
      } catch (error) {
        console.error(error);
      }
    });
  }

  async init(opts) {
    try {
      this.fileIndex = opts.fileIndex;
      this.fileSrcSelector = opts.fileSrcSelector;
      this.imgContainerSelector = opts.previewSelector;
      this.imgSrc = opts.imgSrouce;
      this.imgSrcSelector = opts.imgSrcSelector;
      this.outputInputSelector = opts.ouputSelector;

      await this._initImage();
      await this._initCropper();
      return { success: true, message: "succcessfully initialized." };
    } catch (error) {
      console.error(error);
      return { success: false, message: error.message };
    }
  }

  _createCanvas(source) {
    const canvas = document.createElement("canvas");
    const context = canvas.getContext("2d");
    const { width, height } = source;
    context.imageSmoothingEnabled = true;
    context.drawImage(source, 0, 0, width, height);
    context.globalCompositeOperation = "destination-in";
    context.beginPath();
    context.rect(0, 0, width, height);
    context.fill();
    return canvas;
  }

  async cropImage() {
    try {
      const canvas = this.cropper.getCroppedCanvas({
        imageSmoothingQuality: "low",
        imageSmoothingEnabled: true,
      });
      //const newCanvas = this._createCanvas(canvas);
      const base64Data = canvas.toDataURL("image/jpeg", 0.4);
      const head = "data:image/png;base64, ";
      const sizeMB =
        Math.round(((base64Data.length - head.length) * 3) / 4) / 1024 / 1024;
      const el = document.querySelector(this.outputInputSelector);
      el.value = base64Data;
      return {
        success: true,
        message: "Image successfully cropped.",
        fileSize: sizeMB,
      };
    } catch (error) {
      console.error(error);
      return { success: false, message: error.message };
    }
  }
}

imageCropper.createInstance = () => new ImageCropper();

export default imageCropper;
