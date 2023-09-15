export function saveAsFile(filename, bytesBase64) {
    try {
        var link = document.createElement('a');
        link.download = filename;
        link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + bytesBase64; // Add ',' after 'base64'
        document.body.appendChild(link); // Needed for Firefox
        link.click();
        document.body.removeChild(link);
    } catch (error) {
        console.error("Error while downloading the file:", error);
    }
}
