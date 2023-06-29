window.Observer = {

    observer: null,

    Initialize: function (component, observerTargetId) {
        const options = {
            root: null,
            rootMargin: '0px',
            threshold: 1.0
        };
        let scrollHeight = 0;

        const callback = (entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {

                    if (scrollHeight == 0) {
                        scrollHeight = $(".conversation-section").prop("scrollHeight");
                        $(".conversation-section").scrollTop(($(".conversation-section").prop("scrollHeight") * -1) + 1000);
                        component.invokeMethodAsync('OnIntersection');
                    }
                    else {
                        if (scrollHeight != $(".conversation-section").prop("scrollHeight")) {
                            scrollHeight = $(".conversation-section").prop("scrollHeight");

                            $(".conversation-section").scrollTop(($(".conversation-section").prop("scrollHeight") * -1) + 1000);
                            component.invokeMethodAsync('OnIntersection');
                        }
                        else {
                        }
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