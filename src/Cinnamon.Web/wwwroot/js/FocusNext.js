export function focusNextOtpInput(currentIndex) {
    var nextElement = document.querySelector(`#otp-${currentIndex}`);
    if (nextElement) {
        try {
            nextElement.focus({ preventScroll: true }); // preventScroll ensures the input stays visible
        } catch (e) {
            console.error("Focus failed:", e);
        }
    } else {
        console.warn("Element not found for index:", currentIndex);
    }
}