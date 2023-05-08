window.copyToClipboard = (text) => {
    var element = document.getElementById(text);
    var clipboard = new ClipboardJS(element);
    clipboard.on('success', function (e) {
        console.log('Copied to clipboard: ' + e.text);
        e.clearSelection();
    });
    clipboard.on('error', function (e) {
        console.error('Error copying to clipboard: ' + e.text);
    });
}