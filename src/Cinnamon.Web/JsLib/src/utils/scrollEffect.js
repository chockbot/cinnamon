export function scrollToElement(id) {
    const element = document.getElementById(id);
    if (element) {
        console.log(`Element with ID "${id}" found. Scrolling into view.`); // Debug log
        element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    } else {
        console.warn(`Element with ID "${id}" not found.`);
    }
}

export default {
    scrollToElement
};
