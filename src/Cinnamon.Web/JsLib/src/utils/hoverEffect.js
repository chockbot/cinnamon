const hoverEffect = {
    moveEffect: async function (event) {
        const button = event.currentTarget;
        const effect = button.querySelector('.effect');
        const rect = button.getBoundingClientRect();
        const x = event.clientX - rect.left;
        const y = event.clientY - rect.top;

        effect.style.left = `${x}px`;
        effect.style.top = `${y}px`;
        effect.style.transform = 'translate(-50%, -50%) scale(1)';
        effect.style.opacity = '1';
    },

    hideEffect: async function (event) {
        const button = event.currentTarget;
        const effect = button.querySelector('.effect');
        effect.style.transform = 'translate(-50%, -50%) scale(0)';
        effect.style.opacity = '0';
    }
};

window.hoverEffect = hoverEffect;