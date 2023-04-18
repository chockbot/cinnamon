export default function checkFileSize(inputElem, overAllFileSize, maxFileSize) {
  debugger;
  try {
    if (inputElem && inputElem.files) {
      const file = inputElem.files[0];
      const sizeInMB = file.size / 1024 / 1024;

      if (sizeInMB + overAllFileSize > maxFileSize) {
        inputElem.value = "";

        return {
          success: false,
          message: "Can upload only 10mb for all images",
        };
      }

      return { success: true, message: "success" };
    }

    return {
      success: false,
      message: "An error occured please try again",
    };
  } catch {
    return {
      success: false,
      message: "Your browser not supported this feature",
    };
  }
}
