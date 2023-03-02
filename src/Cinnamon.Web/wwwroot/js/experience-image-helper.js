let cropperCoverPhoto, cropperFirstPhoto, cropperSecondPhoto;

let multiplePhotoUploadInput = "#photo-upload";
const controls = ["#cover-photo", "#first-support-photo", "#second-support-photo"];

let cropperCoverPhotoModal = "#cropperCoverPhotoModal";
let $jsPhotoUploadInput = "#cover-photo";
let imageCoverPhoto = "#imgCoverPhotoPreview";
let btnSaveCropCoverPhoto = "#btnSaveCropCoverPhoto";
let imgSrc1 = "#imgSrc1";
let coverPhotoData = "#coverPhotoData";

let cropperFirstSupportPhotoModal = "#cropperFirstSupportPhotoModal";
let firstSupportPhotoInput = "#first-support-photo";
let imageFirstSupportPhoto = "#imgFirstSupportPhotoPreview";
let btnSaveCropFirstPhoto = "#btnSaveCropFirstPhoto";
let imgSrc2 = "#imgSrc2";
let firstPhotoData = "#firstPhotoData";

let cropperSecondSupportPhotoModal = "#cropperSecondSupportPhotoModal";
let secondSupportPhotoInput = "#second-support-photo";
let imageSecondSupportPhoto = "#imgSecondSupportPhotoPreview";
let btnSaveCropSecondPhoto = "#btnSaveCropSecondPhoto";
let imgSrc3 = "#imgSrc3";
let secondPhotoData = "#secondPhotoData";

let coverPhotos = [imageCoverPhoto, imageFirstSupportPhoto, imageSecondSupportPhoto];
let photoModals = [cropperCoverPhotoModal, cropperFirstSupportPhotoModal, cropperSecondSupportPhotoModal];


$(document).on('change', multiplePhotoUploadInput, function () {
    const inputElement = document.querySelector(multiplePhotoUploadInput);

    for (let i = 0; i < 3; i++) {
        if (inputElement.files[i]) {
            handlePhoto(this, coverPhotos[i], photoModals[i], [inputElement.files[i]]);
        }
    }
});


$(document).on('change', $jsPhotoUploadInput, function () {
    handlePhoto(this, imageCoverPhoto, cropperCoverPhotoModal, null);

});

$(document).on('change', firstSupportPhotoInput, function () {
    handlePhoto(this, imageFirstSupportPhoto, cropperFirstSupportPhotoModal, null);

});

$(document).on('change', secondSupportPhotoInput, function () {
    handlePhoto(this, imageSecondSupportPhoto, cropperSecondSupportPhotoModal, null);
});

$(document).on('click', btnSaveCropCoverPhoto, function () {
    saveCroppedCoverPhoto(event, imgSrc1, cropperCoverPhotoModal, imageCoverPhoto, coverPhotoData, cropperCoverPhoto);

});

$(document).on('click', btnSaveCropFirstPhoto, function () {
    saveCroppedCoverPhoto(event, imgSrc2, cropperFirstSupportPhotoModal, imageFirstSupportPhoto, firstPhotoData, cropperFirstPhoto);
});

$(document).on('click', btnSaveCropSecondPhoto, function () {
    saveCroppedCoverPhoto(event, imgSrc3, cropperSecondSupportPhotoModal, imageSecondSupportPhoto, secondPhotoData, cropperSecondPhoto);

});

function handlePhoto(that, imgPreview, photoModal, multipleFileObject) {
    var files = multipleFileObject ? multipleFileObject : that.files;
    if (files.length > 0) {
        var photo = files[0];

        if (photo.size > 10000000) {

            alert("Please upload image less than 10MB.");
        }
        else {
            var reader = new FileReader();
            reader.onload = function (event) {

                $(photoModal).find(imgPreview).attr('src', event.target.result);
                //imgPreview.src = event.target.result;

                if (photoModal == cropperCoverPhotoModal)
                    cropperCoverPhoto = initializeCropper($(imgPreview)[0]);
                else if (photoModal == cropperFirstSupportPhotoModal)
                    cropperFirstPhoto = initializeCropper($(imgPreview)[0]);
                else if (photoModal == cropperSecondSupportPhotoModal)
                    cropperSecondPhoto = initializeCropper($(imgPreview)[0]);

                $(photoModal).modal({ backdrop: "static", keyboard: false });
                $(photoModal).modal('show');
            };
            reader.readAsDataURL(photo);
        }
    }
}

function initializeCropper(imgPreview) {
    return new Cropper(imgPreview, {
        viewMode: 1,
        aspectRatio: 515 / 670,
        minContainerWidth: 515,
        minContainerHeight: 670,
        minCropBoxWidth: 515,
        minCropBoxHeight: 670,
        movable: true,
        ready: function () {
        },
    });
}

function saveCroppedCoverPhoto(event, imgSrc, photoModal, imgPreview, imageData, cropper) {
    event.preventDefault();

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

    $(imageData).val(base64encodedImage);;

    $(imgSrc).attr("src", base64encodedImage);
    $(photoModal).modal("hide");

    $button.prop("disabled", false);
    $button.text("Save");

    cropper.destroy();
    cropper = null;

    $(imgPreview).src = null;
}

function getRoundedCanvas(sourceCanvas) {
    var canvas = document.createElement('canvas');
    var context = canvas.getContext('2d');
    var width = sourceCanvas.width;
    var height = sourceCanvas.height;
    canvas.width = width;
    canvas.height = height;
    context.imageSmoothingEnabled = true;
    context.drawImage(sourceCanvas, 0, 0, width, height);
    context.globalCompositeOperation = 'destination-in';
    context.beginPath();
    context.fill();
    return canvas;
}
