import axios from "axios";
import { Buffer } from 'buffer';

const creation = {};
let dotnetObj = undefined;
const controls = ["#photo-upload", "#cover-photo", "#first-support-photo", "#second-support-photo"];
const imageData = ["#coverPhotoData", "#firstPhotoData", "#secondPhotoData"];
const locationModal = "#setLocationModal";

creation.showLocationModal = () => {
    $(locationModal).modal('show');
};

creation.hideLocationModal = () => {
    $(locationModal).modal('hide');
};

creation.init = async (obj, activityId) => {
    dotnetObj = obj;
    return await creation.uploadImages(activityId);
};

creation.uploadImages = async (activityId) => {
    const formData = new FormData();
    const formDataPlaceholderImage = new FormData();
    let counter = 0;
    var file = null;

    $(".img-banner").each(function (e) {
        const name = $(this).attr("data-name");
        if (name) {
            file = dataUrlToFile($(imageData[counter]).val(), name);

            if (file) {
                formData.append(`Image${counter + 1}`, file);
                counter++;
            }
        }
    });

    formData.append("ActivityId", activityId);

    try {
        const result = await axios.postForm(
            "api/activity/UploadActivityImage",
            formData
        );

        if (!result.data.success) {
            dotnetObj.invokeMethodAsync("ShowError", result.message);
        }

        return result.data.success;
    } catch (error) {
        if (error.response && error.response.status === 400) {
            try {
                const response = await fetch('/images/placeholder-image.png');
                if (!response.ok) {
                    dotnetObj.invokeMethodAsync("ShowError", response);
                }

                const blob = await response.blob();

                const file = new File([blob], 'placeholder-image.png', { type: blob.type });
                const formEntries = Array.from(formData.entries());
                const formLength = formEntries.length;

                if (formLength > 0) {
                    const filteredFormData = formEntries.filter((item) => item[0] !== 'ActivityId');

                    for (const item of filteredFormData) {
                        formDataPlaceholderImage.append(item[0], file);
                    }
                    formDataPlaceholderImage.append("ActivityId", activityId);

                    try {
                        const uploadResult = await axios.postForm(
                            "api/activity/UploadActivityImage",
                            formDataPlaceholderImage
                        );

                        if (!uploadResult.data.success) {
                            dotnetObj.invokeMethodAsync("ShowError", uploadResult.message);
                        }

                        return uploadResult.data.success;
                    } catch (uploadImageError) {
                        dotnetObj.invokeMethodAsync("ShowError", uploadImageError.message);
                    }
                }

            } catch (fetchImageError) {
                dotnetObj.invokeMethodAsync("ShowError", fetchImageError.response);
            }
        } else {
            dotnetObj.invokeMethodAsync("ShowError", error.response);
        }
    }
};

function dataUrlToFile(dataUrl, filename) {
    const arr = dataUrl.split(',');
    if (arr.length < 2) { return undefined; }
    const mimeArr = arr[0].match(/:(.*?);/);
    if (!mimeArr || mimeArr.length < 2) { return undefined; }
    const mime = mimeArr[1];
    const buff = Buffer.from(arr[1], 'base64');
    return new File([buff], filename, { type: mime });
}

export default creation;