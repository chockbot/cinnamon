import { createWidget } from '@typeform/embed'

export function showTypeForm() {
    // Replace 'cgzRQKWI' with your actual Typeform ID
    var typeformId = 'cgzRQKWI';
    
    // Open Typeform popup
    window.typeformEmbed.makePopup(typeformId, {
        mode: 'popup',
        autoOpen: true,
        autoClose: 0,
        refresh: true,
    });
}

export default {
    showTypeForm
};
