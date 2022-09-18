export function onClickSignup() {
    $("#signup-waiting").attr("disabled", true);
    $("#email-signup").attr("disabled", true);
    $("input[name=signup-value]").attr("disabled", true);
    $("#spinner-loading").css("visibility", "visible");
}

export function onDoneSignup() {
    $("#signup-waiting").attr("disabled", false);
    $("#email-signup").attr("disabled", false);
    $("input[name=signup-value]").attr("disabled", false);
    $("#spinner-loading").css("visibility", "hidden");
}