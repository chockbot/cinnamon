import axios from "axios";

const creation = {};
let dotnetObj = undefined;

creation.init = async (obj, activityId) => {
  dotnetObj = obj;
  return await creation.uploadImages(activityId);
};

creation.uploadImages = async (activityId) => {
  const formData = new FormData();

  const img1 = document.getElementById("image-input-1");
  if (img1) {
    formData.append("Image1", img1.files[0]);
  }
  const img2 = document.getElementById("image-input-2");
  if (img2) {
    formData.append("Image2", img2.files[0]);
  }
  const img3 = document.getElementById("image-input-3");
  if (img3) {
    formData.append("Image3", img3.files[0]);
  }

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

export default creation;
