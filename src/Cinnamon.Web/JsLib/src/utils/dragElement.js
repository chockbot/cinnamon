export default function dragElement(parent, child) {
  const parentEl = document.querySelector(parent);
  const childEl = document.querySelector(child);

  let isDragging = false;
  let startX, startY, initialX, initialY;
  let scale = 1;

  childEl.addEventListener("mousedown", (e) => {
    isDragging = true;
    startX = e.clientX;
    startY = e.clientY;
    initialX = childEl.offsetLeft;
    initialY = childEl.offsetTop;
    childEl.style.cursor = "grabbing";
  });

  const onMouseMove = (e) => {
    if (isDragging) {
      requestAnimationFrame(() => {
        const dx = e.clientX - startX;
        const dy = e.clientY - startY;
        let newX = initialX + dx;
        let newY = initialY + dy;

        // Allow the childEl to move freely
        childEl.style.left = `${newX}px`;
        childEl.style.top = `${newY}px`;
      });
    }
  };

  const onMouseUp = () => {
    isDragging = false;
    childEl.style.cursor = "grab";
  };

  const onWheel = (e) => {
    e.preventDefault();
    const rect = childEl.getBoundingClientRect();
    const offsetX = (e.clientX - rect.left) / rect.width;
    const offsetY = (e.clientY - rect.top) / rect.height;

    const prevScale = scale;
    scale += e.deltaY * -0.0008; // Reduce the scaling factor change for slower zoom
    scale = Math.min(Math.max(0.5, scale), 3); // Limit scale between 0.5 and 3

    const scaleChange = scale / prevScale;

    const newWidth = rect.width * scaleChange;
    const newHeight = rect.height * scaleChange;

    const dx = (newWidth - rect.width) * offsetX;
    const dy = (newHeight - rect.height) * offsetY;

    childEl.style.transform = `scale(${scale})`;
    childEl.style.left = `${childEl.offsetLeft - dx}px`;
    childEl.style.top = `${childEl.offsetTop - dy}px`;
  };

  document.addEventListener("mousemove", onMouseMove);
  document.addEventListener("mouseup", onMouseUp);
  childEl.addEventListener("wheel", onWheel);
}
