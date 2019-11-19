var app = {
    footer: {
        mode: 0,
        breadcrumb: $('#footer-breacrumb'),
        element: $('footer'),
        content: $('#footer-content').hide(),
        animateTimer: 800,
        slideButton: $('#slide-up').click(function () {
            if (app.footer.mode < 0) {
                app.footer.element.stop().animate({
                    bottom: '0px',
                    height: 28
                }, app.footer.animateTimer);
                app.footer.slideButton.find('span').attr('uk-icon', 'chevron-up');
                app.footer.mode = 0;
                app.footer.content.fadeOut(app.footer.animateTimer / 1.5);
            } else if (app.footer.mode > 0) {
                app.footer.element.stop().animate({
                    height: $(window).height() - 70
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
            app.footer.content.html('<div uk-spinner="ratio: 1.5" style="position:absolute;left:50%;top:50%;transform:translate(-50%,-50%);"></div>');
            if (app.footer.mode !== -1) {
                app.footer.show();
            }
            $.get(url, data, function (html) {
                app.footer.content.html(html);
            }).fail(function () {
                app.footer.content.html('Произошла непредвиденная ошибка!');
            }).then(function () {
                if (finished) {
                    finished();
                }

                app.setLastContentState({ footerOptions: { url: url, data: data } });
            });
        },
        setBreadcrumbs: function (crumbs) {
            if (crumbs) {
                var htmlBreadCrumbs = '';
                for (var i = 0; i < crumbs.length; i++) {
                    if (crumbs[i] && crumbs[i].name) {
                        htmlBreadCrumbs += '<li><a href=""';
                        if (crumbs[i].isDisabled) {
                            htmlBreadCrumbs += '  class="uk-disabled" ';
                        }
                        if (crumbs[i].url) {
                            htmlBreadCrumbs += ' data-href="' + crumbs[i].url + '"';
                        }
                        htmlBreadCrumbs += '>' + crumbs[i].name + '</a></li>';
                    }
                }

                app.footer.breadcrumb.html(htmlBreadCrumbs).find('li a').off('click').click(function () {
                    debugger;
                    app.footer.getContent($(this).data('href'));
                });
            }
        }
    },
    menu: {
        element: $('#offcanvas-slide'),
        show: function (interval) {
            var show = () => UIkit.offcanvas('#offcanvas-slide').show();
            if (!interval) {
                show();
            } else {
                setTimeout(show, interval);
            }
        },
        hide: function (interval) {
            var hide = () => UIkit.offcanvas('#offcanvas-slide').hide();
            if (!interval) {
                hide();
            } else {
                setTimeout(hide, interval);
            }
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
    },
    getLastContentState: function () {
        var state = JSON.parse(localStorage.getItem('contentState'));
        return state;
    },
    setLastContentState: function (obj) {
        var state = app.getLastContentState();
        if (!state) state = {};
        if (obj) {
            if (obj.footerOptions) state.footerOptions = obj.footerOptions;
            if (obj.isOpenMenu) state.isOpenMenu = obj.isOpenMenu;
        }
        localStorage.setItem('contentState', JSON.stringify(state));
    },
    initializeContentState: function () {
        var contentState = app.getLastContentState();
        if (!contentState)
            app.setLastContentState({
                footerOptions: {
                    url: null,
                    data: null
                }, isOpenMenu: true
            });
    },
    useContentState: function () {
        var _showFunc = () => {
            app.footer.content.html('<div class="uk-text-center">Добро пожаловать в приложение LikeBusLogistic!<br />Для того чтобы начать выберите один из пунктов меню.</div>');
            app.menu.show(2000);
            app.footer.show();
        };
        var state = app.getLastContentState();
        if (state) {
            if (state.footerOptions && state.footerOptions.url)
                app.footer.getContent(state.footerOptions.url, state.footerOptions.data);
            else _showFunc();
            if (state.isOpenMenu) app.menu.show(); else app.menu.hide();
        } else _showFunc();
    }
};