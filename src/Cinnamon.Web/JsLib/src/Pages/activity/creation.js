import axios from "axios";
import { Buffer } from "buffer";
import Compressor from "compressorjs";

const creation = {};
const creationInProgress = {};
let dotnetObj = undefined;
const controls = [
  "#photo-upload",
  "#cover-photo",
  "#first-support-photo",
  "#second-support-photo",
];
const imageData = ["#coverPhotoData", "#firstPhotoData", "#secondPhotoData"];
const locationModal = "#setLocationModal";

creation.showLocationModal = () => {
  $(locationModal).modal("show");
};

creation.hideLocationModal = () => {
  $(locationModal).modal("hide");
};

creation.uploadImages = async (activityId) => {
  const formData = new FormData();
  const formDataPlaceholderImage = new FormData();
  let counter = 0;
  var file = null;

  $(".img-banner").each(function (e) {
    const name = $(this).attr("data-name");
    if (name) {
      file = dataUrlToFile($(imageData[counter]).val(), name);

      if (file) {
        formData.append(`Image${counter + 1}`, file);
        counter++;
      }
    }
  });

  formData.append("ActivityId", activityId);

  try {
    const result = await axios.postForm(
      "api/activity/UploadActivityImage",
      formData
    );

    if (!result.data.success) {
      dotnetObj.invokeMethodAsync("ShowError", result.message);
    }

    return result.data.success;
  } catch (error) {
    if (error.response && error.response.status === 400) {
      try {
        const response = await fetch("/images/placeholder-image.png");
        if (!response.ok) {
          dotnetObj.invokeMethodAsync("ShowError", response);
        }

        const blob = await response.blob();

        const file = new File([blob], "placeholder-image.png", {
          type: blob.type,
        });
        const formEntries = Array.from(formData.entries());
        const formLength = formEntries.length;

        if (formLength > 0) {
          const filteredFormData = formEntries.filter(
            (item) => item[0] !== "ActivityId"
          );

          for (const item of filteredFormData) {
            formDataPlaceholderImage.append(item[0], file);
          }
          formDataPlaceholderImage.append("ActivityId", activityId);

          try {
            const uploadResult = await axios.postForm(
              "api/activity/UploadActivityImage",
              formDataPlaceholderImage
            );

            if (!uploadResult.data.success) {
              dotnetObj.invokeMethodAsync("ShowError", uploadResult.message);
            }

            return uploadResult.data.success;
          } catch (uploadImageError) {
            dotnetObj.invokeMethodAsync("ShowError", uploadImageError.message);
          }
        }
      } catch (fetchImageError) {
        dotnetObj.invokeMethodAsync("ShowError", fetchImageError.response);
      }
    } else {
      dotnetObj.invokeMethodAsync("ShowError", error.response);
    }
  }
};

creationInProgress.uploadImages = async (activityId) => {
  const formData = new FormData();
  let counter = 0;
  var hasUploadedFile = false;
  var imageFile = null;

  formData.append("ActivityId", activityId);

  $(".img-banner").each(function (e) {
    const name = $(this).attr("data-name");
    if (name) {
      imageFile = dataUrlToFile($(imageData[counter]).val(), name);

      if (imageFile) {
        formData.append(`Image${counter + 1}`, imageFile);
        counter++;
        hasUploadedFile = true;
      }
    }
  });

  if (hasUploadedFile) {
    try {
      const uploadResult = await axios.postForm(
        "api/activity/UploadActivityImage",
        formData
      );

      if (!uploadResult.data.success) {
        dotnetObj.invokeMethodAsync("ShowError", uploadResult.message);
      }

      return uploadResult.data.success;
    } catch (uploadImageError) {
      dotnetObj.invokeMethodAsync("ShowError", uploadImageError.message);
    }
  } else {
    const response = await fetch("/images/placeholder-image.png");
    if (!response.ok) {
      dotnetObj.invokeMethodAsync("ShowError", response);
    }

    const blob = await response.blob();

    const file = new File([blob], "placeholder-image.png", { type: blob.type });

    $(".img-banner").each(function (e) {
      formData.append(`Image${counter + 1}`, file);
      counter++;
    });

    try {
      const uploadResult = await axios.postForm(
        "api/activity/UploadActivityImage",
        formData
      );

      if (!uploadResult.data.success) {
        dotnetObj.invokeMethodAsync("ShowError", uploadResult.message);
      }

      return uploadResult.data.success;
    } catch (uploadImageError) {
      dotnetObj.invokeMethodAsync("ShowError", uploadImageError.message);
    }
  }
};

function dataUrlToFile(dataUrl, filename) {
  if (!dataUrl) return undefined;

  const arr = dataUrl.split(",");
  if (arr.length < 2) {
    return undefined;
  }
  const mimeArr = arr[0].match(/:(.*?);/);
  if (!mimeArr || mimeArr.length < 2) {
    return undefined;
  }
  const mime = mimeArr[1];
  const buff = Buffer.from(arr[1], "base64");
  return new File([buff], filename, { type: mime });
}

export async function initCreation(obj, activityId) {
  dotnetObj = obj;
  return await creation.uploadImages(activityId);
}

export async function initCreationInProgress(obj, activityId) {
  dotnetObj = obj;
  return await creationInProgress.uploadImages(activityId);
}

export async function previewImage(imgSelector, inputSelector) {
  const inputElem = document.querySelector(inputSelector);
  const imgElem = document.querySelector(imgSelector);
  const urlSrc = inputElem.value;
  imgElem.src = urlSrc;
}
export function previewBase64Image(imgSelector, base64String, fileType) {
    debugger;
    const imgElem = document.querySelector(imgSelector);
    // Construct the Data URL
    const dataUrl = `data:${fileType};base64,${base64String}`;
    imgElem.src = dataUrl;
}

export async function previewImageByFileInput({
  imgSelector,
  inputSelector,
  fileIndex,
  inputSelectorToWriteBlob,
}) {
  const inputElem = document.querySelector(inputSelector);
  const imgElem = document.querySelector(imgSelector);
  if (inputElem.files[fileIndex]) {
    const image = await compressImage(inputElem.files[fileIndex]);
    if (inputSelectorToWriteBlob) {
      const blob = await blobToBase64(image);
      const el = document.querySelector(inputSelectorToWriteBlob);
      el.value = blob;
    }
    imgElem.src = URL.createObjectURL(image);
  }
}

export async function uploadListImages(selectors, activityId, deletedIds) {
    debugger;
  const formData = new FormData();
  for (const selector of selectors) {
    const file = dataUrlToFile($(selector.selector).val(), "image");
    if (file) {
      formData.append("Images", file);
      formData.append("Orders", selector.order);
    }
  }
  formData.append("ActivityId", activityId);
  // for deleted ids
  for (const id of deletedIds) {
    formData.append("DeletedIds", id);
  }

  try {
    const result = await axios.postForm(
      "api/activity/UploadActivityImage",
      formData
    );
    return { success: result.data.success, message: result.data.message };
  } catch (error) {
    return { success: false, message: error.message };
  }
}

export async function addImageTemplate(imgId, inputId, containerSelector) {
  const template = `
    <div class="col-12 col-md-6 mb-4 d-none" id="${imgId}-${inputId}">
        <input type="hidden" id="${inputId}" name = "${inputId}" class="generated-input-data" />
        <img id="${imgId}" class="w-100" src="" alt="supporting-photo">
    </div>
    `;
  $(containerSelector).append(template);
}

export async function removeImageTemplate(selector) {
  $(selector).remove();
}

export async function showImageTemplate(selector) {
  $(selector).removeClass("d-none");
}

export async function removeImageItems(selector) {
  $(selector).empty();
}

const compressImage = (blob) => {
  return new Promise((resolve, reject) => {
    if (!blob) return reject();

    new Compressor(blob, {
      quality: 0.4,
      success: (result) => {
        return resolve(result);
      },
      error: (error) => {
        return reject(error);
      },
    });
  });
};

function blobToBase64(blob) {
  return new Promise((resolve, _) => {
    const reader = new FileReader();
    reader.onloadend = () => resolve(reader.result);
    reader.readAsDataURL(blob);
  });
}

export default {
  initCreation,
  initCreationInProgress,
  addImageTemplate,
  previewImage,
  removeImageItems,
  removeImageTemplate,
  showImageTemplate,
  uploadListImages,
  previewImageByFileInput,
  previewBase64Image
};
