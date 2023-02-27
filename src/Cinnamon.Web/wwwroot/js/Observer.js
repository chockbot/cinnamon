window.Observer = {

    observer: null,

    Initialize: function (component, observerTargetId) {
        const options = {
            root: null,
            rootMargin: '0px',
            threshold: 1.0
        };

        const callback = (entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    console.log('Box is visible!');
                    component.invokeMethodAsync('OnIntersection');
                }
            });
        };

        const observer = new IntersectionObserver(callback, options);

        let box = document.getElementById(observerTargetId);
        
        if (box == null) console.log('Target was not found');;
        observer.observe(box);
    }

};