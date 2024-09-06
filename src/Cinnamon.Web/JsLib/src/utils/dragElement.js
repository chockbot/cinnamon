export default function dragElement(parent, child) {
  const parentEl = document.querySelector(parent);
  const childEl = document.querySelector(child);

  let isDragging = false;
  let startX, startY, initialX, initialY;

  childEl.addEventListener("mousedown", (e) => {
    isDragging = true;
    startX = e.clientX;
    startY = e.clientY;
    initialX = childEl.offsetLeft;
    initialY = childEl.offsetTop;
    childEl.style.cursor = "grabbing";
  });

  document.addEventListener("mousemove", (e) => {
    if (isDragging) {
      const dx = e.clientX - startX;
      const dy = e.clientY - startY;
      let newX = initialX + dx;
      let newY = initialY + dy;

      // Ensure the childEl stays within the bounds of the parent
      newX = Math.max(
        0,
        Math.min(newX, parentEl.clientWidth - childEl.clientWidth)
      );
      newY = Math.max(
        0,
        Math.min(newY, parentEl.clientHeight - childEl.clientHeight)
      );

      childEl.style.left = `${newX}px`;
      childEl.style.top = `${newY}px`;
    }
  });

  document.addEventListener("mouseup", () => {
    isDragging = false;
    childEl.style.cursor = "grab";
  });
}
