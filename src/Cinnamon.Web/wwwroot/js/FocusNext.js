export function focusNextOtpInput(currentIndex) {
    debugger;
    var nextElement = document.querySelector(`#otp-${currentIndex}`);
    if (nextElement) {
        nextElement.focus();
    }
}