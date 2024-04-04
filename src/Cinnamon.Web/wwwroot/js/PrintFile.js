export function printJS(base64Content, fileType) {
    if (fileType.startsWith('image/')) {
        var img = new Image();
        img.src = 'data:' + fileType + ';base64,' + base64Content;
        img.onload = function () {
            var iframe = document.createElement('iframe');
            iframe.style.display = 'none';
            document.body.appendChild(iframe);
            var iframeDoc = iframe.contentWindow.document;
            iframeDoc.open();
            iframeDoc.write('<html><head><style>@page { size: A4; margin: 0; }</style><title>Print Image</title></head><body>');
            iframeDoc.write('<img src="' + img.src + '" style="max-width: 100%; height: auto;" />');
            iframeDoc.write('</body></html>');
            iframeDoc.close();

            // Print the iframe content
            iframe.contentWindow.print();

            // Remove the iframe after printing
            document.body.removeChild(iframe);
        };
    } else {
        var binaryContent = atob(base64Content);
        var blob = new Blob([new Uint8Array(binaryContent.length).map((_, i) => binaryContent.charCodeAt(i))], { type: fileType });
        var url = URL.createObjectURL(blob);
        var printWindow = window.open(url, '_blank'); // Open in a new tab
        printWindow.onload = function () {
            // Add event listener to close print window if print dialog is canceled
            window.addEventListener('beforeprint', function () {
                printWindow.close();
            });
            // Reload the page after print dialog is closed or canceled
            window.addEventListener('afterprint', function () {
                location.reload(true);
            });

            printWindow.print();
            URL.revokeObjectURL(url);
        };
    }
}
