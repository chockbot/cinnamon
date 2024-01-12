export function countWords({
  inputSelector,
  count,
  isPrevent,
  labelSelector,
  labelFormat,
  continueCount,
}) {
  const textInput = document.querySelector(inputSelector);
  const wordCount = document.querySelector(labelSelector);
  const maxWords = count;
  $(textInput).on("input", (e) => {
    const words = $(textInput).val().split(/\s+/);
    const wordsTrimmed = words.filter((w) => w !== "");
    const wordsTrimmedCount =
      wordsTrimmed.length > maxWords && !continueCount
        ? maxWords
        : wordsTrimmed.length;
    let labelValue;
    if (labelFormat) {
      labelValue = labelFormat.split("###").join(wordsTrimmedCount);
    }
    $(wordCount).text(labelValue);
    if (isPrevent && words.length > maxWords) {
      $(textInput).val(words.slice(0, maxWords).join(" "));
    }
  });
  $(inputSelector).trigger("input");
}
