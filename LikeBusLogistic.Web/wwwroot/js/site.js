var app = {
    footer: {
        mode: 0,
        element: $('footer'),
        content: $('#footer-content').hide(),
        animateTimer: 800,
        slideButton: $('#slide-up').click(function () {
            if (app.footer.mode < 0) {
                app.footer.element.stop().animate({
                    bottom: '0px',
                    height: 50
                }, app.footer.animateTimer);
                app.footer.slideButton.find('span').attr('uk-icon', 'chevron-up');
                app.footer.mode = 0;
                app.footer.content.fadeOut(app.footer.animateTimer / 1.5);
            } else if (app.footer.mode > 0) {
                app.footer.element.stop().animate({
                    height: $(window).height() - 60
                }, app.footer.animateTimer);
                app.footer.slideButton.find('span').attr('uk-icon', 'chevron-down');
                app.footer.mode = -1;
            } else {
                app.footer.element.stop().animate({
                    height: $(window).height() / 3
                }, app.footer.animateTimer / 1.5);
                app.footer.slideButton.find('span').attr('uk-icon', 'chevron-up');
                app.footer.mode = 1;
                app.footer.content.fadeIn(app.footer.animateTimer / 1.25);
            }
        }),
        changeMode: function (m) {
            app.footer.mode = m;
            app.footer.slideButton.click();
        },
        maximize: function () { app.footer.changeMode(1); },
        show: function () { app.footer.changeMode(0); },
        hide: function () { app.footer.changeMode(-1); },
        getContent: function (url, data, finished) {
            app.footer.content.html('<div uk-spinner="ratio: 1.5"></div>');
            app.footer.show();
            $.get(url, data, function (html) {
                app.footer.content.html(html);
            }).fail(function () {
                    app.footer.content.html('Произошла непредвиденная ошибка!');
            }).then(function () {
                    debugger;
                if (finished) {
                    finished();
                }
            });
        }
    },
    message: {
        showMessage: function (type, title, text, timer, showConfirmButton) {
            Swal.fire({
                icon: type,
                title: title,
                text: text,
                timer: timer,
                showConfirmButton: showConfirmButton
            });
        },
        showError: function (title, text) {
            this.showMessage('error', title, text, 2000, false);
        },
        showSuccess: function (title, text) {
            this.showMessage('success', title, text, 2000, false);
        },
        showInfo: function (title, text) {
            this.showMessage('info', title, text, 2000, false);
        },
        showErrorWithOk: function (title, text) {
            this.showMessage('error', title, text, 0, true);
        },
        showSuccessWithOk: function (title, text) {
            this.showMessage('success', title, text, 0, true);
        },
        showInfoWithOk: function (title, text) {
            this.showMessage('info', title, text, 0, true);
        }
    },
    getCookie: function (name) {
        var match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
        if (match) return match[2];
    }
};

$(document).ready(function () {
    $('.menu-item').click(function () {
        app.footer.getContent($(this).data('href'), null, function () {
            UIkit.offcanvas('#offcanvas-slide').hide();
        });
    });
});