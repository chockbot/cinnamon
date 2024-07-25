export async function moveEffect(event, inputSelector) {
    debugger;
        const button = document.querySelector(inputSelector);
        const effect = button.querySelector('.effect');
        const rect = button.getBoundingClientRect();
        const x = event.clientX - rect.left;
        const y = event.clientY - rect.top;

        effect.style.left = `${x}px`;
        effect.style.top = `${y}px`;
        effect.style.transform = 'translate(-50%, -50%) scale(1)';
        effect.style.opacity = '1';
}

export async function hideEffect(inputSelector) {
    debugger;
        const button = document.querySelector(inputSelector);
        const effect = button.querySelector('.effect');
        effect.style.transform = 'translate(-50%, -50%) scale(0)';
        effect.style.opacity = '0';
}

export default {
    moveEffect,
    hideEffect
};