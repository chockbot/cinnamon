const maxMinWords = {};

maxMinWords.max = (selector, max) => {
  $(selector).keyup(function (e) {
      const inputValue = e.target.value;
      const wordCount = inputValue.trim().split(/\s+/).length;

      if (wordCount >= 15 && !isSpace(inputValue)) {
          e.preventDefault();
          e.target.value = truncateText(inputValue);
      }
  });
};

maxMinWords.min = (selector, min) => {
  $(selector).keydown(function (e) {
    const wordsLength = $(this)
      .val()
      .trim()
      .split(" ")
      .filter((t) => !!t).length;

    if (wordsLength < min) {
      e.preventDefault();
      e.stopPropagation();
    }
  });
};

function isSpace(text) {
    return text.trim().split(/\s+/).length === 14 && /\s$/.test(text);
}

function truncateText(text) {
    return text.trim().split(/\s+/).slice(0, 15).join(' ');
}
export default maxMinWords;
