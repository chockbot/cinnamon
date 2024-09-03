export function updateProgress() {
    // Initialize tooltips with manual trigger and show them
    $('[data-toggle="tooltip"]').tooltip({ trigger: 'manual' }).tooltip('show');
   // Adjust the width of each progress bar based on the 'aria-valuenow' attribute
    $(".progress-bar").each(function () {
        each_bar_width = $(this).attr('aria-valuenow');
        $(this).width(each_bar_width + '%');
    });
}