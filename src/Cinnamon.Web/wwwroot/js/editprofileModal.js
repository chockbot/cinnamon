let cropper;
let cropperModalId = '#cropperModal';
let $jsPhotoUploadInput = $('.js-photo-upload');
let imageData;


$jsPhotoUploadInput.on('change', function (e) {
    var files = this.files;
    if (files.length > 0) {
        var photo = files[0];

        var reader = new FileReader();
        reader.onload = function (event) {
            var image = $('.js-avatar-preview')[0];
            image.src = event.target.result;

            cropper = new Cropper(image, {
                viewMode: 1,
                aspectRatio: 1,
                minContainerWidth: 400,
                minContainerHeight: 400,
                minCropBoxWidth: 271,
                minCropBoxHeight: 271,
                movable: true,
                ready: function () {
                    console.log('ready');
                    console.log(cropper.ready);
                }
            });

            $(cropperModalId).modal('show');
        };
        reader.readAsDataURL(photo);
    }
});

$('.js-save-cropped-avatar').on('click', function (event) {
    event.preventDefault();

    console.log(cropper.ready);

    var $button = $(this);
    $button.text('Saving...');
    $button.prop('disabled', true);

    const canvas = cropper.getCroppedCanvas();
    const base64encodedImage = canvas.toDataURL();
    imageData = base64encodedImage;

    $('#avatar-crop').attr('src', base64encodedImage);
    $(cropperModalId).modal('hide');

    $button.prop('disabled', false);
    $button.text('Save');

    cropper.destroy();
    cropper = null;
});

export function ChangeContentJS() {
    DotNet.invokeMethodAsync('Cinnamon.Web', "ChangeParaContentValue", imageData);
}

$('.btn-close').on('click', function (event) {
    $(cropperModalId).modal('hide');
    cropper.destroy();
    cropper = null;
});

