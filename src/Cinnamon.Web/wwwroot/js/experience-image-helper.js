let cropperCoverPhoto, cropperFirstPhoto, cropperSecondPhoto;

let multiplePhotoUploadInput = "#photo-upload";
const controls = [
  "#cover-photo",
  "#first-support-photo",
  "#second-support-photo",
];

let cropperCoverPhotoModal = "#cropperCoverPhotoModal";
let $jsPhotoUploadInput = "#cover-photo";
let imageCoverPhoto = "#imgCoverPhotoPreview";
let btnSaveCropCoverPhoto = "#btnSaveCropCoverPhoto";
let imgSrc1 = "#imgSrc1";
let coverPhotoData = "#coverPhotoData";
let btnCloseCropperCoverPhotoModal = "#btnCloseCropperCoverPhotoModal";
let coverPhotoFileName = "";

let cropperFirstSupportPhotoModal = "#cropperFirstSupportPhotoModal";
let firstSupportPhotoInput = "#first-support-photo";
let imageFirstSupportPhoto = "#imgFirstSupportPhotoPreview";
let btnSaveCropFirstPhoto = "#btnSaveCropFirstPhoto";
let imgSrc2 = "#imgSrc2";
let firstPhotoData = "#firstPhotoData";
let btnCloseCropperFirstSupportPhotoModal =
  "#btnCloseCropperFirstSupportPhotoModal";
let firstPhotoFileName = "";

let cropperSecondSupportPhotoModal = "#cropperSecondSupportPhotoModal";
let secondSupportPhotoInput = "#second-support-photo";
let imageSecondSupportPhoto = "#imgSecondSupportPhotoPreview";
let btnSaveCropSecondPhoto = "#btnSaveCropSecondPhoto";
let imgSrc3 = "#imgSrc3";
let secondPhotoData = "#secondPhotoData";
let btnCloseCropperSecondSupportPhotoModal =
  "#btnCloseCropperSecondSupportPhotoModal";
let secondPhotoFileName = "";

let experienceCreationGuideModal = "#experienceCreationGuideModal";
let infoTitle = "#info-title";
let infoDescription = "#info-description";
let imgExperienceCreationGuidePreview = "#imgExperienceCreationGuidePreview";
let btnCloseExperienceCreationGuideModal = "#btnCloseExperienceCreationGuideModal";

let coverPhotos = [
  imageCoverPhoto,
  imageFirstSupportPhoto,
  imageSecondSupportPhoto,
];
let photoModals = [
  cropperCoverPhotoModal,
  cropperFirstSupportPhotoModal,
  cropperSecondSupportPhotoModal,
];

$(document).on("change", multiplePhotoUploadInput, function () {
  const inputElement = document.querySelector(multiplePhotoUploadInput);
  let totalFileSize = 0;

  if (inputElement.files.length > 3) {
    return;
  } else {
    for (let i = 0; i < 3; i++) {
      if (inputElement.files[i]) {
        totalFileSize += inputElement.files[i].size / 1024 / 1024;
      }
    }
    if (totalFileSize > 10) {
      return;
    } else {
      for (let i = 0; i < 3; i++) {
        if (inputElement.files[i]) {
          handlePhoto(this, coverPhotos[i], photoModals[i], [
            inputElement.files[i],
          ]);
        }
      }
    }
  }
});

$(document).on("change", $jsPhotoUploadInput, function () {
  handlePhoto(this, imageCoverPhoto, cropperCoverPhotoModal, null);
});

$(document).on("change", firstSupportPhotoInput, function () {
  handlePhoto(
    this,
    imageFirstSupportPhoto,
    cropperFirstSupportPhotoModal,
    null
  );
});

$(document).on("change", secondSupportPhotoInput, function () {
  handlePhoto(
    this,
    imageSecondSupportPhoto,
    cropperSecondSupportPhotoModal,
    null
  );
});

$(document).on("click", btnSaveCropCoverPhoto, function () {
  saveCroppedCoverPhoto(
    event,
    imgSrc1,
    cropperCoverPhotoModal,
    imageCoverPhoto,
    coverPhotoData,
    cropperCoverPhoto,
    0,
    coverPhotoFileName
  );
});

$(document).on("click", btnSaveCropFirstPhoto, function () {
  saveCroppedCoverPhoto(
    event,
    imgSrc2,
    cropperFirstSupportPhotoModal,
    imageFirstSupportPhoto,
    firstPhotoData,
    cropperFirstPhoto,
    1,
    firstPhotoFileName
  );
});

$(document).on("click", btnSaveCropSecondPhoto, function () {
  saveCroppedCoverPhoto(
    event,
    imgSrc3,
    cropperSecondSupportPhotoModal,
    imageSecondSupportPhoto,
    secondPhotoData,
    cropperSecondPhoto,
    2,
    secondPhotoFileName
  );
});

$(document).on("click", btnCloseCropperCoverPhotoModal, function () {
  closeModal(cropperCoverPhotoModal, cropperCoverPhoto);
});

$(document).on("click", btnCloseCropperFirstSupportPhotoModal, function () {
  closeModal(cropperFirstSupportPhotoModal, cropperFirstPhoto);
});

$(document).on("click", btnCloseCropperSecondSupportPhotoModal, function () {
  closeModal(cropperSecondSupportPhotoModal, cropperSecondPhoto);
});

$(document).on("click", `${infoTitle}, ${infoDescription}`, function () {
    let fileName = $(this).attr("data-image-url");
    displayExperienceCreationGuideModal(fileName);
});

$(document).on("click", btnCloseExperienceCreationGuideModal, function () {
    closeExperienceCreationGuideModal();
});




function handlePhoto(that, imgPreview, photoModal, multipleFileObject) {
  var files = multipleFileObject ? multipleFileObject : that.files;
  if (files.length > 0) {
    var photo = files[0];
    var fileName = photo.name;
    if (photo.size > 10000000) {
      return;
    } else {
      var reader = new FileReader();
      reader.onload = function (event) {
        $(photoModal).find(imgPreview).attr("src", event.target.result);
        //imgPreview.src = event.target.result;

        if (photoModal == cropperCoverPhotoModal) {
          cropperCoverPhoto = initializeCropper($(imgPreview)[0], photoModal);
          coverPhotoFileName = fileName;
        } else if (photoModal == cropperFirstSupportPhotoModal) {
          cropperFirstPhoto = initializeCropper($(imgPreview)[0], photoModal);
          firstPhotoFileName = fileName;
        } else if (photoModal == cropperSecondSupportPhotoModal) {
          cropperSecondPhoto = initializeCropper($(imgPreview)[0], photoModal);
          secondPhotoFileName = fileName;
        }
      };
      reader.readAsDataURL(photo);
    }
  }
}

function initializeCropper(imgPreview, photoModal) {
  return new Cropper(imgPreview, {
    viewMode: 1,
    aspectRatio: 4 / 5,
    minContainerWidth: 350,
    minContainerHeight: 400,
    minCropBoxWidth: 271,
    minCropBoxHeight: 271,
    movable: true,
    ready: function () {
      $(photoModal).modal({ backdrop: "static", keyboard: false });
      $(photoModal).modal("show");
    },
  });
}

function saveCroppedCoverPhoto(
  event,
  imgSrc,
  photoModal,
  imgPreview,
  imageData,
  cropper,
  index,
  fileName
) {
  event.preventDefault();

  var $button = $(this);
  $button.text("Saving...");
  $button.prop("disabled", true);

  //Crop
  const canvas = cropper.getCroppedCanvas({
    imageSmoothingQuality: "low",
    imageSmoothingEnabled: true,
  });
  //Round
  const roundedcanvas = getRoundedCanvas(canvas);

  //Check image Size
  const size = roundedcanvas.size;
  //Show
  const base64encodedImage = roundedcanvas.toDataURL("image/jpeg", 0.4);

  $(imageData).val(base64encodedImage);

  loadImage(imgSrc, base64encodedImage, index, fileName);
  $(photoModal).modal("hide");

  $button.prop("disabled", false);
  $button.text("Save");

  cropper.destroy();
  cropper = null;

  $(imgPreview).src = null;
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
  //$(imgSrc).attr("src", base64encodedImage);
}

function getRoundedCanvas(sourceCanvas) {
  var canvas = document.createElement("canvas");
  var context = canvas.getContext("2d");
  var width = sourceCanvas.width;
  var height = sourceCanvas.height;
  canvas.width = width;
  canvas.height = height;
  context.imageSmoothingEnabled = true;
  context.drawImage(sourceCanvas, 0, 0, width, height);
  context.globalCompositeOperation = "destination-in";
  context.beginPath();
  context.rect(0, 0, width, height);
  context.fill();
  return canvas;
}

function closeModal(cropperModalId, cropper) {
  $(cropperModalId).modal("hide");
  cropper.destroy();
  cropper = null;
}

function displayExperienceCreationGuideModal(fileName) {
    $(experienceCreationGuideModal).modal("show");
    $(imgExperienceCreationGuidePreview).attr("src", fileName);
}

function closeExperienceCreationGuideModal() {
    $(experienceCreationGuideModal).modal("hide");
    $(imgExperienceCreationGuidePreview).attr("src", "");
}