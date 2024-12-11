export function handleOtpInput(value, index, otpArray, otpLength) {
    if (value && value.length > 0 && !isNaN(value[0])) { // Check if input exists and starts with a digit
        otpArray[index] = value[0]; // Store the first character in OTP array

        // Move focus to the next input if it exists
        if (index + 1 < otpLength) {
            const nextInputId = `otp-${index + 1}`;
            const nextInput = document.getElementById(nextInputId);
            if (nextInput) {
                setTimeout(() => nextInput.focus(), 100);
            }
        }
    }
}

export function handleOtpBackspace(index, otpArray) {
    // Clear the current input
    otpArray[index] = "";

    // Move focus to the previous input if it exists
    if (index > 0) {
        const previousInputId = `otp-${index - 1}`;
        const previousInput = document.getElementById(previousInputId);
        if (previousInput) {
            setTimeout(() => previousInput.focus(), 100);
        }
    }
}

export async function getClipboardText() { // Mark the function as async
    if (navigator.clipboard) {
        try {
            const text = await navigator.clipboard.readText(); // Await works correctly now
            return text.replace(/\s+/g, '').replace(/[^0-9]/g, '');
        } catch (err) {
            console.error('Failed to read clipboard:', err);
        }
    }

    // Fallback for older browsers
    return new Promise((resolve) => {
        document.addEventListener(
            'paste',
            (event) => {
                const clipboardData = event.clipboardData || window.clipboardData;
                if (clipboardData) {
                    const pastedData = clipboardData.getData('text').replace(/\s+/g, '').replace(/[^0-9]/g, '');
                    resolve(pastedData);
                } else {
                    resolve('');
                }
            },
            { once: true }
        );
    });
}
