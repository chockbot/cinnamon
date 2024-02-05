export function disableModalClose() {
    try
    {
        $('#modal-disabled-close').modal({ backdrop: 'static', keyboard: false });
    }
    catch (error)
    {
        console.error("Error while closing the modal:", error);
    }
}