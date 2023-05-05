import axios from "axios";

const upload = {};

upload.execute = async () => {
  const form = document.getElementById("form-upload-ids");
  const formData = new FormData(form);

  const { data } = await axios.postForm(
    "api/account/UploadGovernmentIds",
    formData
  );

  if (data.success) {
    return { success: true, message: "success" };
  } else {
    return { success: false, message: data.message };
  }
};

export default upload;
