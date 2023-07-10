window.Observer = {

    observer: null,

    Initialize: function (component, observerTargetId, containerTargetClass) {
        const options = {
            root: null,
            rootMargin: '0px',
            threshold: 1.0
        };
        let scrollHeight = 0;

        const callback = (entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {

                    if (scrollHeight != $(containerTargetClass).prop("scrollHeight")) {
                        scrollHeight = $(containerTargetClass).prop("scrollHeight");

                        if (window.location.toString().toLowerCase().indexOf(("profile").toLowerCase()) != -1 ||
                            window.location.toString().toLowerCase().indexOf(("explore").toLowerCase()) != -1) {
                            $(containerTargetClass).scrollTop(($(containerTargetClass).prop("scrollHeight")) + 1000);
                        }
                        else {
                            $(containerTargetClass).scrollTop(($(containerTargetClass).prop("scrollHeight") * -1) + 1000);
                        }
                        component.invokeMethodAsync('OnIntersection');
                    }
                }
            });
        };

        const observer = new IntersectionObserver(callback, options);

        let box = document.getElementById(observerTargetId);
        
        if (box != null) {
            observer.observe(box);
        }
    }

};