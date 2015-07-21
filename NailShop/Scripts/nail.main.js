$('#langChange').click(function () {
    if (lfl.fn.getInputValue('langValue') == 'en-US') {
        lfl.fn.changeLanguage('vi-VN');
    } else {
        lfl.fn.changeLanguage('en-US');
    };
});


var lfl = lfl || {};

lfl.fn = ({
    setCookie: function (name, value) {
        document.cookie = name + "=" + value + "; path=/";
        if (window.parent) {
            window.parent.document.cookie = name + "=" + value + "; path=/";
        };
    },

    getCookie: function (name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1);
            if (c.indexOf(nameEQ) != -1) return c.substring(nameEQ.length, c.length);
        };
        return "";
    },

    changeLanguage: function (val) {
        var seft = this;
        seft.setCookie('LANGUAGE', val);
        window.location.reload(true);
    },

    getIndexOwlCarousel: function (classElement) {
        var seft = this, $classElement = $(classElement);
        return $classElement.closest('.owl-item').index();
    },

    getInputValue: function (idElement) {
        return $('#' + idElement).val();
    },
});