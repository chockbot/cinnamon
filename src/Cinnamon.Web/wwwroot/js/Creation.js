export function confirmation() {
    $(document).ready(function () {

        $('#regForm').on('mousedown', stopNavigate);

        $('#regForm').on('mouseleave', function () {
            $(window).on('beforeunload', function () {
                return 'Are you sure you want to leave?';
            });
        });
    });

    function stopNavigate() {
        $(window).off('beforeunload');
    }
}

export function OffBeforeUnload() {
   $(window).off('beforeunload');
}