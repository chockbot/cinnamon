export function focusInput(id) {
    const input = document.getElementById(id);
    if (input) {
        setTimeout(() => {
            input.focus();
        }, 100);
    }
}

export function getClipboardText() {
    if (navigator.clipboard) {
        try {
            const text = await navigator.clipboard.readText();
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