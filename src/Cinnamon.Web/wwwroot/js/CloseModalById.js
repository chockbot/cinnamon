export function closeModalById(modalId) {
    var modal = document.getElementById(modalId);
    if (modal) {
        modal.style.display = "none";
        modal.className = "";
    }
}

