export function moveNext(selector) {
  const parent = $(selector).parent();
  const nextEl = $(parent).next();
  $(nextEl).after(parent);
}

export function movePrevious(selector) {
  const parent = $(selector).parent();
  const prevEl = $(parent).prev();
  // prevent to select dragable preview element
  if (!$(prevEl).hasClass("draggable-preview-start")) {
    $(prevEl).before(parent);
  }
}

export default {
  moveNext,
  movePrevious,
};
