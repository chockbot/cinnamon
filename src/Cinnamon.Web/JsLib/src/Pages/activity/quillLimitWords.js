const quillLimitWords = {};

quillLimitWords.init = (selector, label, wordsLimit) => {
  const container = document.querySelector(selector);
  const editorInstance = Quill.find(container);

  function wordsLength() {
    const text = editorInstance.getText();
    const words = text.split(" ").filter((w) => w.trim() !== "");

    return words.length;
  }

  $(label).text(`Max of ${wordsLimit} words (${wordsLength()}/80)`);

  editorInstance.on("text-change", function (delta, oldDelta, source) {
    const lengthOfWords = wordsLength();
    $(label).text(
      `Max of ${wordsLimit} words (${lengthOfWords}/${wordsLimit})`
    );

    if (lengthOfWords > wordsLimit) {
      editorInstance.setContents(oldDelta);
    }
  });
};

export default quillLimitWords;
