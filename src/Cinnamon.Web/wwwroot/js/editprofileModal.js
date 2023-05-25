let cropper;
let cropperModalId = "#cropperModal";
let $jsPhotoUploadInput = $("input[data-input-id='js-photo-upload']");
let imageData;
const allowedFileTypes = ["jpeg", "jpg", "png"];

$jsPhotoUploadInput.on("change", function (e) {
  var files = this.files;
  if (files.length > 0) {
    var photo = files[0];
    let extension = photo.name
      .substr(photo.name.lastIndexOf(".") + 1)
      .toLowerCase();
    if (photo.size > 10000000) {
      return;
    } else if (!allowedFileTypes.includes(extension)) {
      return;
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

$(".js-save-cropped-avatar").on("click", function (event) {
  event.preventDefault();

  console.log(cropper.ready);

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

  imageData = base64encodedImage;

  $("#avatar-crop").attr("src", base64encodedImage);
  $(cropperModalId).modal("hide");

  $button.prop("disabled", false);
  $button.text("Save");

  cropper.destroy();
  cropper = null;
});

$(".btn-delete").on("click", function (event) {
  event.preventDefault();

  imageData = null;

  $("#avatar-crop").attr("src", $("#originalImage").val());
});

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
export function ChangeContentJS(dotnetHelper) {
  dotnetHelper.invokeMethodAsync("ChangeParaContentValue", imageData);
}

$(".btn-close").on("click", function (event) {
  $(cropperModalId).modal("hide");
  cropper.destroy();
  cropper = null;
});
