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
                    height: 25
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
                        htmlBreadCrumbs += '<li><span';
                        if (crumbs[i].url && i !== crumbs.length - 1) {
                            htmlBreadCrumbs += ' data-href="' + crumbs[i].url + '"';
                        }
                        htmlBreadCrumbs += '>' + crumbs[i].name + '</span></li>';
                    }
                }

                app.footer.breadcrumb.html(htmlBreadCrumbs).find('li span').off('click').click(function () {
                    var href = $(this).data('href');
                    if (href) {
                        app.footer.getContent(href);
                    }
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
            state.isOpenMenu = !(!obj.isOpenMenu && true);
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
    },
    serializeInputsToObject: function (selector, excludedAttributes = null, excludedTypes = null) {
        var obj = {};
        $(selector).each(function (i, el) {
            if (excludedAttributes && excludedAttributes.length) {
                for (var i = 0; i < excludedAttributes.length; i++)
                    if ($(el).attr(excludedAttributes[i]) || $(el).is(':' + excludedAttributes[i])) return;
            }

            obj[$(el).attr('name')] = $(el).val();
        });
        return obj;
    },
    postDataOnServer: function (url, data, successHandler = null, errorHandler = null, method = 'POST') {
        $.ajax({
            url: url,
            method: method,
            data: data,
            success: function (result) {
                if (result.success) {
                    if (successHandler) {
                        successHandler(result.message);
                    } else {
                        app.message.showSuccessWithOk('Успешно', result.message || 'Успешно выполнено!');
                    }
                } else {
                    if (errorHandler) {
                        errorHandler(result.message);
                    } else {
                        app.message.showErrorWithOk('Ошибка', result.message || 'Произошла непредвиденная ошибка!');
                    }
                }
            }
        });
    }
};

$(document).ready(function () { if (localStorage.getItem('isLoggedOff') === 'true') { app.useContentState(); localStorage.setItem('isLoggedOff', 'false'); } });
document.addEventListener("DOMContentLoaded", function (event) {
    app.initializeContentState();
    $('#offcanvas-slide').on('shown', function () {
        app.setLastContentState({ isOpenMenu: true });
    });
    $('#offcanvas-slide').on('hidden', function () {
        app.setLastContentState({ isOpenMenu: false });
    });
    $('.menu-item').click(function () {
        app.footer.getContent($(this).data('href'), null, function () {
            UIkit.offcanvas('#offcanvas-slide').hide();
        });
    });
});