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

    const { data } = await axios.postForm(
        "api/activity/UploadActivityImage",
        formData
    );

    if (!data.success) {
        dotnetObj.invokeMethodAsync("ShowError", data.message);
    }

    return data.success;
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