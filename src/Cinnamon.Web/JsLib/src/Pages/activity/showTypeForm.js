import { createWidget } from '@typeform/embed'
import '@typeform/embed/build/css/widget.css'

export function showTypeForm() {
    // Replace 'cgzRQKWI' with your actual Typeform ID
    var typeformId = 'cgzRQKWI';
    
    // Open Typeform popup
    window.typeformEmbed.makePopup(typeformId, {
        mode: 'popup',
        autoOpen: true,
        autoClose: 0,
    });
}

export default {
    showTypeForm
};
