export function handleOtpInput() {
    document.querySelectorAll("[id^='otp-']").forEach((input, index, inputs) => {
        input.addEventListener("input", function (e) {
            const value = e.target.value;
            if (value && value.length > 0 && !isNaN(value[0])) {
                e.target.value = value[0]; // Store only the first character
                if (index + 1 < inputs.length) {
                    const nextInput = inputs[index + 1];
                    if (nextInput) {
                        setTimeout(() => nextInput.focus(), 100);
                    }
                }
            }
        });

        input.addEventListener("keydown", function (e) {
            if (e.key === "Backspace" && !e.target.value) {
                // Preventing the default action before shifting focus
                e.preventDefault();

                if (index > 0) {
                    const previousInput = inputs[index - 1];
                    if (previousInput) {
                        // Focus on the previous input field immediately
                        setTimeout(() => previousInput.focus(), 0); // Set timeout to 0 for immediate focus shift
                    }
                }
            }
        });
    });
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
