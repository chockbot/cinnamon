export function focusNextOtpInput(currentIndex) {
    var nextElement = document.querySelector(`#otp-${currentIndex}`);
    if (nextElement) {
        nextElement.focus();
    }
}