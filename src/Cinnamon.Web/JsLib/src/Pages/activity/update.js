import axios from "axios";

const update = {};
let dotnetObj = undefined;

update.init = async (obj, activityId) => {
  dotnetObj = obj;
  return await update.uploadImages(activityId);
};

update.uploadImages = async (activityId) => {
  const formData = new FormData();
  const inputEl = document.querySelector("#photo-upload");
  let counter = 1;
  $(".img-banner").each(function (e) {
    const name = $(this).attr("data-name");
    if (name) {
      const file = getFile(inputEl, name);
      if (file) {
        formData.append(`Image${counter}`, file);
        counter++;
      }
    }
  });

  formData.append("ActivityId", activityId);

  const { data } = await axios.postForm(
    "api/activity/UploadActivityImage",
    formData
  );

  if (!data.success) {
    dotnetObj.invokeMethodAsync("ShowError", data.message);
  }

  return data.success;
};

function getFile(element, name) {
  for (let i = 0; i < element.files.length; i++) {
    if (element.files[i].name === name) {
      return element.files[i];
    }
  }
  return undefined;
}

export default update;
