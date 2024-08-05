export function copyToClipboard(text) {
    navigator.clipboard.writeText("")
        .then(function () {
            return navigator.clipboard.writeText(text);
        })
        .then(function () {
            var alertContainer = document.getElementById("alertContainer");
            alertContainer.classList.add("showAlert");

            // Hide the alert message after 1 second
            setTimeout(function () {
                alertContainer.classList.remove("showAlert");
            }, 1000);
        })
        .catch(function (error) {
            console.error("Copy to clipboard failed: " + error);
        });
}
