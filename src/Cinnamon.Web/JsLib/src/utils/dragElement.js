export default function dragElement(parent, child) {
    const parentEl = document.querySelector(parent);
    const childEl = document.querySelector(child);

    let isDragging = false;
    let startX, startY, initialX, initialY;
    let scale = 1;
    let initialDistance = 0;
    const dragThreshold = 10; // Threshold in pixels to distinguish drag from tap
    let movedDistance = 0;
    let startTime = 0; // Track when touch started
    const tapDurationThreshold = 200; // Milliseconds to consider as tap instead of drag

    // Helper function to get the correct coordinates
    const getEventCoordinates = (e) => {
        if (e.touches) {
            return { x: e.touches[0].clientX, y: e.touches[0].clientY };
        } else {
            return { x: e.clientX, y: e.clientY };
        }
    };

    const getDistanceBetweenTouches = (e) => {
        if (e.touches.length === 2) {
            const touch1 = e.touches[0];
            const touch2 = e.touches[1];
            const dx = touch2.clientX - touch1.clientX;
            const dy = touch2.clientY - touch1.clientY;
            return Math.sqrt(dx * dx + dy * dy);
        }
        return 0;
    };

    const onStartDrag = (e) => {
        if (e.touches && e.touches.length === 2) {
            // Pinch-to-zoom start
            initialDistance = getDistanceBetweenTouches(e);
            return;
        }

        e.preventDefault();
        isDragging = true;
        startTime = Date.now(); // Record the time the touch started
        const coords = getEventCoordinates(e);
        startX = coords.x;
        startY = coords.y;
        initialX = childEl.offsetLeft;
        initialY = childEl.offsetTop;
        movedDistance = 0; // Reset moved distance
        childEl.style.cursor = "grabbing";
    };

    const onDragMove = (e) => {
        if (e.touches && e.touches.length === 2) {
            // Pinch-to-zoom move
            const newDistance = getDistanceBetweenTouches(e);
            if (initialDistance) {
                const scaleChange = newDistance / initialDistance;
                scale *= scaleChange;
                scale = Math.min(Math.max(0.5, scale), 3);
                childEl.style.transform = `scale(${scale})`;
                initialDistance = newDistance;
            }
            return;
        }

        if (isDragging) {
            const coords = getEventCoordinates(e);
            const dx = coords.x - startX;
            const dy = coords.y - startY;
            movedDistance = Math.sqrt(dx * dx + dy * dy); // Calculate moved distance

            // Only treat it as a drag if movement exceeds the threshold
            if (movedDistance > dragThreshold) {
                requestAnimationFrame(() => {
                    let newX = initialX + dx;
                    let newY = initialY + dy;

                    // Move the child element
                    childEl.style.left = `${newX}px`;
                    childEl.style.top = `${newY}px`;
                });
            }
        }
    };

    const onStopDrag = () => {
        isDragging = false;
        childEl.style.cursor = "grab";
    };

    const onWheelZoom = (e) => {
        e.preventDefault();
        const rect = childEl.getBoundingClientRect();
        const offsetX = (e.clientX - rect.left) / rect.width;
        const offsetY = (e.clientY - rect.top) / rect.height;

        const prevScale = scale;
        scale += e.deltaY * -0.0008;
        scale = Math.min(Math.max(0.5, scale), 3);

        const scaleChange = scale / prevScale;
        const newWidth = rect.width * scaleChange;
        const newHeight = rect.height * scaleChange;

        const dx = (newWidth - rect.width) * offsetX;
        const dy = (newHeight - rect.height) * offsetY;

        childEl.style.transform = `scale(${scale})`;
        childEl.style.left = `${childEl.offsetLeft - dx}px`;
        childEl.style.top = `${childEl.offsetTop - dy}px`;
    };

    // Mouse events for desktop
    childEl.addEventListener("mousedown", onStartDrag);
    document.addEventListener("mousemove", onDragMove);
    document.addEventListener("mouseup", onStopDrag);
    childEl.addEventListener("wheel", onWheelZoom);

    // Touch events for mobile
    childEl.addEventListener("touchstart", onStartDrag);
    document.addEventListener("touchmove", onDragMove);
    document.addEventListener("touchend", (e) => {
        const duration = Date.now() - startTime;
        // Consider it a tap if moved distance is below threshold and duration is short enough
        if (movedDistance <= dragThreshold && duration <= tapDurationThreshold) {
            e.target.click(); // Simulate a click for touch events
        }
        onStopDrag();
    });

    // Ensure clicks on buttons inside the child element work
    childEl.addEventListener(
        "click",
        (e) => {
            // Only prevent click if it was a real drag
            if (movedDistance > dragThreshold) {
                e.preventDefault();
            }
        },
        true
    ); // Use capturing to make sure it's applied before child elements' handlers
}
