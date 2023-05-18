import axios from "axios";

const uploadGovernmentIds = {};

uploadGovernmentIds._initForm = (dotnetObj) => {
  $("#submit-government-ids").submit(async function (e) {
    e.preventDefault();

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

    const formData = new FormData(this);

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

export default uploadGovernmentIds;
