const maxMinWords = {};

maxMinWords.max = (selector, max) => {
  $(selector).keydown(function (e) {
    const wordsLength = $(this)
      .val()
      .trim()
      .split(" ")
      .filter((t) => !!t).length;

    if (wordsLength === max) {
      e.preventDefault();
      e.stopPropagation();
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

export default maxMinWords;
