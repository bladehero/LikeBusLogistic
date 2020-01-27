var App = {
    map: null,
    tab: {
        name: null,
        isName: function (name) {
            return App.tab.name === name;
        }
    },
    geo: {
        locations: [],
        defaultZoom: 10,
        closeAllPopups: function (is_forced) {
            let is_closed = false;
            App.geo.locations.forEach(function (location) {
                if (location.marker.isPopupOpen()) {
                    location.marker.closePopup();
                    is_closed = true;
                }
            });
            return is_closed;
        },
        getZoomToView: function () {
            let zoom = App.map.getZoom();
            return zoom > App.geo.defaultZoom ? zoom : App.geo.defaultZoom;
        },
        onClickLocation: function (handler) {
            if (handler) {
                for (var i = 0; i < App.geo.locations.length; i++) {
                    let location = App.geo.locations[i];
                    location.marker.on('click',
                        function (obj) {
                            handler(obj, location);
                        }
                    );
                }
            }
        },
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
        },
        clearLocations: function () {
            let location;
            while (App.geo.locations.length > 0) {
                location = App.geo.locations.pop();
                App.map.removeControl(location.marker);
            }
        },
        resetLocations: function (url) {
            App.geo.clearLocations();
            let locations = App.geo.getAllLocations(url);
            App.geo.setLocations(locations);
        },
        getLocationById: function (id) {
            for (let location of App.geo.locations) {
                if (location.data.id === id) {
                    return location;
                }
            }
        },
        getAllLocations: function (url) {
            var locations = [];
            App.postDataOnServer(url, null,
                function (result) {
                    if (result.success) {
                        if (result.data.length) {
                            for (var i = 0; i < result.data.length; i++) {
                                var marker = L.marker([result.data[i].latitude, result.data[i].longtitude]);
                                locations.push({
                                    marker: marker,
                                    data: result.data[i]
                                });
                            }
                        }
                    }
                },
                (result) => { console.log(result) },
                'GET',
                false
            );
            return locations;
        },
        route: {
            id: null,
            path: null,
            getIndexByRouteLocationId: function (id) {
                for (let i = 0; i < App.geo.route.routeLocations.length; i++) {
                    if (App.geo.route.routeLocations[i].routeLocation.currentLocationId === id) {
                        return i;
                    }
                }
                return -1;
            },
            routeLocations: [],
            resetRouteLocations: function (url, id) {
                App.geo.route.clear();
                let routeLocations = App.geo.route.getRouteLocations(url, id);
                App.geo.route.setRouteLocations(routeLocations, id);
            },
            setRouteLocations: function (routeLocations, id) {
                let latlngs = [];
                for (var i = 0; i < routeLocations.length; i++) {
                    latlngs.push([routeLocations[i].latitude || routeLocations[i].routeLocation.currentLatitude,
                        routeLocations[i].longtitude || routeLocations[i].routeLocation.currentLongtitude]);
                }
                let options = { use: L.polyline, delay: 800, dashArray: [10, 10], weight: 6, color: "#C3C3FF", pulseColor: "#0059FF" };
                let path = new L.Polyline.AntPath(latlngs, options);
                path.addTo(App.map);
                App.geo.route.routeLocations = routeLocations;
                let oldPath = App.geo.route.path;
                App.geo.route.path = path;
                if (oldPath) {
                    oldPath.removeFrom(App.map);
                }
                App.geo.route.id = id;
            },
            routeLocationsAsLocations: function () {
                let locations = [];
                for (let routeLocation of App.geo.route.routeLocations) {
                    let l = App.geo.getLocationById(routeLocation.routeLocation.currentLocationId);
                    if (l && l.data) {
                        locations.push(l.data);
                    }
                }
                return locations;
            },
            getRouteLocations: function (url, id) {
                var routeLocations = [];
                App.postDataOnServer(url, { id: id },
                    function (result) {
                        if (result.success) {
                            if (result.data.length) {
                                for (var i = 0; i < result.data.length; i++) {
                                    for (var j = 0; j < App.geo.locations.length; j++) {
                                        if (App.geo.locations[j].data.id === result.data[i].currentLocationId) {
                                            routeLocations.push({
                                                routeLocation: result.data[i]
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    },
                    (result) => { console.log(result) },
                    'GET',
                    false
                );
                return routeLocations;
            },
            clear: function () {
                if (App.geo.route.path) {
                    App.map.removeControl(App.geo.route.path);
                }
                App.geo.closeAllPopups();
                App.geo.route.id = null;
                App.geo.route.path = null;
                App.geo.route.routeLocations = [];
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
                    height: '0px'
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
        maximize: function (interval) {
            if (!interval) {
                App.footer.changeMode(1);
            } else {
                setTimeout(() => App.footer.changeMode(1), interval);
            }
        },
        show: function (interval) {
            if (!interval) {
                App.footer.changeMode(0);
            } else {
                setTimeout(() => App.footer.changeMode(0), interval);
            }
        },
        hide: function (interval) {
            if (!interval) {
                App.footer.changeMode(-1);
            } else {
                setTimeout(() => App.footer.changeMode(-1), interval);
            }
        },
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
        defaultTimer: 3000,
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
            this.showMessage('error', title, text, App.message.defaultTimer, false);
        },
        showSuccess: function (title, text) {
            this.showMessage('success', title, text, App.message.defaultTimer, false);
        },
        showInfo: function (title, text) {
            this.showMessage('info', title, text, App.message.defaultTimer, false);
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
                for (let i = 0; i < excludedAttributes.length; i++)
                    if ($(el).attr(excludedAttributes[i]) || $(el).is(':' + excludedAttributes[i])) return;
            }
            var val;
            if ($(el).attr('type') === 'checkbox' || $(el).attr('type') === 'radio') {
                val = $(el).is(':checked');
            } else {
                val = $(el).val();
            }
            obj[$(el).attr('name')] = val;
        });
        return obj;
    },
    postDataOnServer: function (url, data, successHandler = null, errorHandler = null, method = 'POST', async = true) {
        $.ajax({
            url: url,
            method: method,
            data: data,
            async: async,
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
    },
    loadContent: function (selector, url, data, handler, finished) {
        let _content = $(selector);
        _content.html('<div uk-spinner="ratio: 1.5" style="position:absolute;left:50%;top:50%;transform:translate(-50%,-50%);"></div>');
        $.get(url, data, function (html) {
            if (handler) {
                handler(html);
            }
            if (selector) {
                _content.html(html);
            }
        }).fail(function () {
            _content.html('Произошла непредвиденная ошибка!');
        }).then(function () {
            if (finished) {
                finished();
            }
        });
    }
};
$.ajaxSetup({
    complete: function (xhr) {
        if (xhr.status === 401) {
            Swal.fire({
                title: 'Выход',
                html: 'Ваша сессия истекла!<br />Пожалуйста, перезайдите в систему.',
                icon: 'info'
            }).then(() => {
                $('#logout').click();
            });
        }
    }
});
$(document).ready(function () {
    if (localStorage.getItem('isLoggedOff') === 'true') { App.useContentState(); localStorage.setItem('isLoggedOff', 'false'); }
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
    $(document).on('click', '#popup-change-location', function () {
        App.footer.maximize(1500);
    });

    $(document).on('click', 'ul.uk-tab>li', function () {
        var url = $(this).data('url');
        var data = $(this).data('data');
        App.setLastContentState({ footerOptions: { url: url, data: data } });
    });

    $(document).keydown(function (obj) {
        if (!$(document.activeElement).parents().hasClass('uk-offcanvas-container')) {
            if (obj.keyCode === 27) {
                let isClosedPopup = App.geo.closeAllPopups();
                if (!isClosedPopup) {
                    if (App.footer.mode === 1) {
                        App.footer.hide();
                    } else if (App.footer.mode === -1) {
                        App.footer.show();
                    }
                }
            }
            else if (obj.keyCode === 113) {
                if (App.footer.mode === 1) {
                    App.footer.maximize();
                } else if (App.footer.mode === 0) {
                    App.footer.show();
                }
            }
        }
    });
});