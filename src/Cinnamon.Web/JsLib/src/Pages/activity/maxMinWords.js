const maxMinWords = {};

maxMinWords.init = (labelSelector, editorSelector, measure, wordsLimit) => {
    setLabelText(labelSelector, editorSelector, measure, wordsLimit);
}

maxMinWords.max = (labelSelector, selector, measure, max) => {
  $(selector).keyup(function (e) {
      const inputValue = e.target.value;
      const wordCount = inputValue.trim().split(/\s+/).length;

      if (wordCount >= max && !isSpace(inputValue)) {
          e.preventDefault();
          e.target.value = truncateText(inputValue);
      }

      setLabelText(labelSelector, selector, measure, max);
  });
};

maxMinWords.min = (labelSelector, selector, measure, min) => {
    $(selector).keyup(function (e) {
        setLabelText(labelSelector, selector, measure, min);
    });
};

function isSpace(text) {
    return text.trim().split(/\s+/).length === 14 && /\s$/.test(text);
}

function truncateText(text) {
    return text.trim().split(/\s+/).slice(0, 15).join(' ');
}

function getWordsLength(editorSelector) {
    return $(editorSelector).val() ? $(editorSelector).val().trim().split(/\s+/).length : 0;
}

function setLabelText(labelSelector, editorSelector, measure, count) {
    $(labelSelector).text(`${measure} of ${count} words (${getWordsLength(editorSelector)}/${count})`);

}
export default maxMinWords;
