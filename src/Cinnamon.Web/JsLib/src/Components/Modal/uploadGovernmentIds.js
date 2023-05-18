import axios from "axios";
import Compressor from "compressorjs";

const uploadGovernmentIds = {};

uploadGovernmentIds._initForm = (dotnetObj) => {
  $("#submit-government-ids").submit(async function (e) {
    e.preventDefault();

    debugger;

    const front = $("#FrontId").val();
    const back = $("#BackId").val();

    if (!front) {
      $("#id-front-error").show();
    } else {
      $("#id-front-error").hide();
    }

    if (!back) {
      $("#id-back-error").show();
    } else {
      $("#id-back-error").hide();
    }

    if (!back || !front) return;

    const formData = new FormData();

    try {
      const image1 = await uploadGovernmentIds.compressImage(
        $("#FrontId")[0].files[0]
      );
      const image2 = await uploadGovernmentIds.compressImage(
        $("#BackId")[0].files[0]
      );

      formData.append("FrontId", image1, image1.name);
      formData.append("BackId", image2, image2.name);
    } catch (error) {
      return;
    }

    // show loading spinner
    dotnetObj.invokeMethodAsync("ShowLoading");

    const { data } = await axios.postForm(
      "api/account/UploadGovernmentIds",
      formData
    );

    // hide loading
    dotnetObj.invokeMethodAsync("HideLoading");

    if (data.success) {
      await dotnetObj.invokeMethodAsync("SubmitAccountVerified");
      location.href = "/verificationprocess";
    } else {
      dotnetObj.invokeMethodAsync("ShowError", data.message);
    }
  });
};

uploadGovernmentIds.init = (dotnetObj) => {
  uploadGovernmentIds._initForm(dotnetObj);
};

uploadGovernmentIds.compressImage = (blob) => {
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

export default uploadGovernmentIds;
