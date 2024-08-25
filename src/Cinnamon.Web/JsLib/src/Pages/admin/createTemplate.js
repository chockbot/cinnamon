import axios from "axios";

export default async function createTemplate({ items, imgId, jsonId }) {
  try {
    const formData = new FormData();
    Object.entries(items).forEach(([key, value]) => {
      formData.append(key, value);
    });

    const imgEl = document.getElementById(imgId);
    const jsonEl = document.getElementById(jsonId);

    formData.append("ImageFile", imgEl.files[0]);
    formData.append("JsonFile", jsonEl.files[0]);

    const result = await axios.postForm("api/activity/SeatPlan", formData);
    return { success: result.data.success, message: result.data.message };
  } catch (error) {
    return {
      success: false,
      message: "An error occurred. Please try again later.",
    };
  }
}
