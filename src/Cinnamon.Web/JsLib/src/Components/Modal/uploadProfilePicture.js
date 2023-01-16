import axios from "axios";

const uploadProfilePicture = {};
let cropper;
let cropperModalId = "#cropperModal";
let $jsPhotoUploadInput = undefined;
let imageData;

uploadProfilePicture._initForm = (dotnetObj) => {
  $("#submit-profile-picture").submit(async function (e) {
    e.preventDefault();

    const profileImage = $("#ProfilePicture").val();

    if (!profileImage) {
      $("#id-profile-image").show();
    } else {
      $("#id-profile-image").hide();
    }

    const formData = new FormData(this);

    // show loading spinner
    dotnetObj.invokeMethodAsync("ShowLoading");

    const { data } = await axios.postForm(
      "api/account/UploadProfilePicture",
      formData
    );

    // hide loading
    dotnetObj.invokeMethodAsync("HideLoading");

    if (data.success) {
      dotnetObj.invokeMethodAsync("HideModal");
    }
  });
};
uploadProfilePicture._uploadImage = () => {
  $jsPhotoUploadInput.on("change", function (e) {
    var files = this.files;
    if (files.length > 0) {
      var photo = files[0];

      if (photo.size > 10000000) {
        alert("Please upload image less than 10MB.");
      } else {
        var reader = new FileReader();
        reader.onload = function (event) {
          var image = $(".js-avatar-preview")[0];
          image.src = event.target.result;

          cropper = new Cropper(image, {
            viewMode: 1,
            aspectRatio: 1,
            minContainerWidth: 350,
            minContainerHeight: 400,
            minCropBoxWidth: 271,
            minCropBoxHeight: 271,
            movable: true,
            ready: function () {
              console.log("ready");
              console.log(cropper.ready);
            },
          });

          $(cropperModalId).modal("show");
        };
        reader.readAsDataURL(photo);
      }
    }
  });
};
uploadProfilePicture._cropImage = () => {
  $(".js-save-cropped-avatar").on("click", function (event) {
    event.preventDefault();

    console.log(cropper.ready);

    var $button = $(this);
    $button.text("Saving...");
    $button.prop("disabled", true);

    //Crop
    const canvas = cropper.getCroppedCanvas();
    //Round
    const roundedcanvas = getRoundedCanvas(canvas);

    //Check image Size
    const size = roundedcanvas.size;
    //Show
    const base64encodedImage = roundedcanvas.toDataURL();

    imageData = base64encodedImage;

    $("#avatar-crop").attr("src", base64encodedImage);
    $(cropperModalId).modal("hide");

    $button.prop("disabled", false);
    $button.text("Save");

    cropper.destroy();
    cropper = null;
  });
};
uploadProfilePicture._roundImage = () => {
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
    context.arc(
      width / 2,
      height / 2,
      Math.min(width, height) / 2,
      0,
      2 * Math.PI,
      true
    );
    context.fill();
    return canvas;
  }
};
uploadProfilePicture._closeModal = () => {
  $(".btn-close").on("click", function (event) {
    $(cropperModalId).modal("hide");
    cropper.destroy();
    cropper = null;
  });
};
uploadProfilePicture.init = (dotnetObj) => {
  $jsPhotoUploadInput = $(".js-photo-upload");

  uploadProfilePicture._initForm(dotnetObj);
  uploadProfilePicture._uploadImage();
  uploadProfilePicture._cropImage();
  uploadProfilePicture._roundImage();
  uploadProfilePicture._closeModal();
};

export default uploadProfilePicture;
