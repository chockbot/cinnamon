export function focusInput(id) {
    const input = document.getElementById(id);
    if (input) {
        input.focus();
    }
}

export function getClipboardText() {
    return new Promise((resolve) => {
        document.addEventListener('paste', (event) => {
            const clipboardData = event.clipboardData || window.clipboardData;
            if (clipboardData) {
                // Remove spaces and non-numeric characters
                const pastedData = clipboardData.getData('text').replace(/\s+/g, '').replace(/[^0-9]/g, '');
                resolve(pastedData);
            } else {
                resolve("");
            }
        }, { once: true });
    });
}