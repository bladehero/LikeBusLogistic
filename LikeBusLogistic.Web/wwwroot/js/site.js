var App = {
    map: L.map('map'),
    geo: {
        locations: [],
        setLocations: function (obj) {
            if (obj) {
                if (obj.length) {
                    for (var i = 0; i < obj.length; i++) {
                        App.geo.locations.push(obj[i]);
                        obj[i].marker.addTo(App.map);
                    }
                } else {
                    App.geo.locations.push(obj);
                    obj.marker.addTo(App.map);
                }
            }
        }
    },
    footer: {
        mode: 0,
        breadcrumb: $('#footer-breacrumb'),
        element: $('footer'),
        content: $('#footer-content').hide(),
        animateTimer: 800,
        slideButton: $('#slide-up').click(function () {
            if (App.footer.mode < 0) {
                App.footer.element.stop().animate({
                    bottom: '0px',
                    height: 25
                }, App.footer.animateTimer);
                App.footer.slideButton.find('span').attr('uk-icon', 'chevron-up');
                App.footer.mode = 0;
                App.footer.content.fadeOut(App.footer.animateTimer / 1.5);
            } else if (App.footer.mode > 0) {
                App.footer.element.stop().animate({
                    height: $(window).height() - 70
                }, App.footer.animateTimer);
                App.footer.slideButton.find('span').attr('uk-icon', 'chevron-down');
                App.footer.mode = -1;
                App.footer.content.fadeIn(App.footer.animateTimer / 1.25);
            } else {
                App.footer.element.stop().animate({
                    height: $(window).height() / 3
                }, App.footer.animateTimer / 1.5);
                App.footer.slideButton.find('span').attr('uk-icon', 'chevron-up');
                App.footer.mode = 1;
                App.footer.content.fadeIn(App.footer.animateTimer / 1.25);
            }
        }),
        changeMode: function (m) {
            App.footer.mode = m;
            App.footer.slideButton.click();
        },
        maximize: function () { App.footer.changeMode(1); },
        show: function () { App.footer.changeMode(0); },
        hide: function () { App.footer.changeMode(-1); },
        getContent: function (url, data, finished) {
            App.map.off('mousemove');
            App.map.off('click');
            App.footer.content.html('<div uk-spinner="ratio: 1.5" style="position:absolute;left:50%;top:50%;transform:translate(-50%,-50%);"></div>');
            if (App.footer.mode !== -1) {
                App.footer.show();
            }
            $.get(url, data, function (html) {
                App.footer.content.html(html);
            }).fail(function () {
                App.footer.content.html('Произошла непредвиденная ошибка!');
            }).then(function () {
                if (finished) {
                    finished();
                }

                App.setLastContentState({ footerOptions: { url: url, data: data } });
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

                App.footer.breadcrumb.html(htmlBreadCrumbs).find('li span').off('click').click(function () {
                    var href = $(this).data('href');
                    if (href) {
                        App.footer.getContent(href);
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
        var state = App.getLastContentState();
        if (!state) state = {};
        if (obj) {
            if (obj.footerOptions) state.footerOptions = obj.footerOptions;
            state.isOpenMenu = !(!obj.isOpenMenu && true);
        }
        localStorage.setItem('contentState', JSON.stringify(state));
    },
    initializeContentState: function () {
        var contentState = App.getLastContentState();
        if (!contentState)
            App.setLastContentState({
                footerOptions: {
                    url: null,
                    data: null
                },
                isOpenMenu: true
            });
    },
    useContentState: function () {
        var _showFunc = () => {
            App.footer.content.html('<div class="uk-text-center">Добро пожаловать в приложение LikeBusLogistic!<br />Для того чтобы начать выберите один из пунктов меню.</div>');
            App.menu.show(2000);
            App.footer.show();
        };
        var state = App.getLastContentState();
        if (state) {
            if (state.footerOptions && state.footerOptions.url)
                App.footer.getContent(state.footerOptions.url, state.footerOptions.data);
            else _showFunc();
            if (state.isOpenMenu) App.menu.show(); else App.menu.hide();
        } else _showFunc();
    },
    serializeInputsToObject: function (selector, excludedAttributes = null, excludedTypes = null) {
        var obj = {};
        $(selector).each(function (i, el) {
            if (excludedAttributes && excludedAttributes.length) {
                for (var i = 0; i < excludedAttributes.length; i++)
                    if ($(el).attr(excludedAttributes[i]) || $(el).is(':' + excludedAttributes[i])) return;
            }
            var val;
            if ($(el).attr('type') === 'checkbox' || $(el).attr('type') === 'radiobutton') {
                val = $(el).is(':checked');
            } else {
                val = $(el).val();
            }
            obj[$(el).attr('name')] = val;
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
                        successHandler(result);
                    } else {
                        App.message.showSuccessWithOk('Успешно', result.message || 'Успешно выполнено!');
                    }
                } else {
                    if (errorHandler) {
                        errorHandler(result);
                    } else {
                        App.message.showErrorWithOk('Ошибка', result.message || 'Произошла непредвиденная ошибка!');
                    }
                }
            }
        });
    }
};

$(document).ready(function () { if (localStorage.getItem('isLoggedOff') === 'true') { App.useContentState(); localStorage.setItem('isLoggedOff', 'false'); } });
document.addEventListener("DOMContentLoaded", function (event) {
    App.initializeContentState();
    $('#offcanvas-slide').on('shown', function () {
        App.setLastContentState({ isOpenMenu: true });
    });
    $('#offcanvas-slide').on('hidden', function () {
        App.setLastContentState({ isOpenMenu: false });
    });
    $('.menu-item').click(function () {
        App.footer.getContent($(this).data('href'), null, function () {
            UIkit.offcanvas('#offcanvas-slide').hide();
        });
    });
});