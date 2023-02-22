import axios from "axios";

const creation = {};
let dotnetObj = undefined;

creation.init = async (obj, activityId) => {
  dotnetObj = obj;
  return await creation.uploadImages(activityId);
};

creation.uploadImages = async (activityId) => {
  const formData = new FormData();
  let counter = 1;
  $(".img-banner").each(function (e) {
    const name = $(this).attr("data-name");
    if (name) {
      const file = getFile(name);
      formData.append(`Image${counter}`, file);
      counter++;
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

function getFile(name) {
    var controls = ["#photo-upload", "#cover-photo", "#first-support-photo", "#second-support-photo"];
    var file = null;
    for (var j = 0; j < controls.length; j++) {
        const element = document.querySelector(controls[j]);
        for (let i = 0; i < element.files.length; i++) {
            if (element.files[i].name === name) {
                file = element.files[i];
                break;
            }
        }
        if (file) {
            break;
        }
    }

    return file;
}

export default creation;
