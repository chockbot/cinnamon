export function ClickIcon() {
	$(".star-icon i").click(function () {
		$(this).toggleClass("fa-star fa-star-o");
	});
}

export function HideModal() {
	$("#sigupModal").modal('hide');
}