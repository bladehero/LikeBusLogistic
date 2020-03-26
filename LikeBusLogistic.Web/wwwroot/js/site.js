function setUpAjax() {
    let timeout;
    $.ajaxSetup({
        beforeSend: function () {
            App.blockUI();
        },
        complete: function (xhr) {
            if (timeout) {
                clearTimeout(timeout);
            }
            timeout = setTimeout(App.unblockUI, 500);
            if (xhr.status === 401) {
                Swal.fire({
                    title: 'Выход',
                    html: 'Ваша сессия истекла!<br />Пожалуйста, перезайдите в систему.',
                    icon: 'info'
                }).then(() => {
                    $('#logout').click();
                });
            }
        },
    });
}
setUpAjax();
var App = {
    isiOS: function () {
        return !!navigator.platform && /iPad|iPhone|iPod/.test(navigator.platform);
    },
    isMobile: function () {
        return /Android|webOS|Macintosh|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    },
    lockUI: $('#ui-lock'),
    blockUI: function () {
        App.lockUI.fadeIn();
    },
    unblockUI: function () {
        App.lockUI.fadeOut();
    },
    loader: '<div class="lds-ripple uk-align-center"><div></div><div></div></div>',
    customDataTable: function (selector) {
        setTimeout(function () {
            let _this = $(selector);
            let defaultIndex = $(_this).find('th.datatable-default-column').index();
            defaultIndex = defaultIndex === -1 ? 0 : defaultIndex;
            let table = _this.dataTable({
                aoColumnDefs: [{
                    bSortable: false,
                    aTargets: ['datatable-no-sort']
                }],
                order: [[defaultIndex, "asc"]],
                lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "Все"]],
                'language': {
                    "processing": "Подождите...",
                    "search": "",
                    "lengthMenu": "_MENU_",
                    "info": "Записи с _START_ до _END_ из _TOTAL_ записей",
                    "infoEmpty": "Записи с 0 до 0 из 0 записей",
                    "infoFiltered": "(отфильтровано из _MAX_ записей)",
                    "infoPostFix": "",
                    "loadingRecords": "Загрузка записей...",
                    "zeroRecords": "Записи отсутствуют.",
                    "emptyTable": "В таблице отсутствуют данные",
                    "paginate": {
                        "first": '<span uk-icon="icon: chevron-double-left; ratio: 0.9;"></span>',
                        'previous': '<span uk-icon="icon: chevron-left; ratio: 0.9;"></span>',
                        'next': '<span uk-icon="icon: chevron-right; ratio: 0.9;"></span>',
                        "last": '<span uk-icon="icon: chevron-double-right; ratio: 0.9;"></span>'
                    },
                    "aria": {
                        "sortAscending": ": активировать для сортировки столбца по возрастанию",
                        "sortDescending": ": активировать для сортировки столбца по убыванию"
                    },
                    "select": {
                        "rows": {
                            "_": "Выбрано записей: %d",
                            "0": "Кликните по записи для выбора",
                            "1": "Выбрана одна запись"
                        }
                    }
                }
            });
            table.addClass('custom-table-resize');
            table.addClass('uk-width-1-1 uk-table-small uk-table-justify uk-table-middle uk-table-divider uk-card uk-card-default uk-margin-small-top uk-margin-small-bottom uk-box-shadow-small');
            let th = table.find('th');
            th.addClass('uk-text-center');
            let wrapper = table.parents('div.dataTables_wrapper');
            wrapper.addClass('uk-grid-collapse uk-child-width-expand uk-margin-small-top').attr('uk-grid', '');
            let divTableLength = wrapper.find('div.dataTables_length');
            divTableLength.addClass('uk-width-1-2');
            let tableLength = divTableLength.find('select');
            tableLength.addClass('uk-select uk-form-width-medium uk-form-small uk-float-left');
            var divSearch = wrapper.find('div.dataTables_filter');
            divSearch.addClass('uk-width-1-2');
            let search =  divSearch.find('input[type="search"]');
            search.addClass('uk-input uk-form-width-medium uk-form-small uk-float-right');
            let info = wrapper.find('div.dataTables_info');
            info.addClass('uk-width-1-2 uk-float-left uk-visible@m uk-text-lighter uk-text-muted');
            let divPaginate = wrapper.find('div.dataTables_paginate.paging_simple_numbers');
            divPaginate.addClass('uk-width-1-2@m uk-width-auto@s');
            let paginateButtons = divPaginate.find('a.paginate_button');
            let emptyText = table.find('.dataTables_empty');
            emptyText.addClass(' uk-text-lighter uk-text-emphasis');
        }, 45);
    },
    hasProperty: function (obj, key) {
        return key.split(".").every(function (x) {
            if (typeof obj != "object" || obj === null || !x in obj)
                return false;
            obj = obj[x];
            return true;
        });
    },
    getPropertyOrDefault: function (obj, key, def) {
        return key.split(".").reduce(function (o, x) {
            return (typeof o == "undefined" || o === null) ? o : o[x] || def;
        }, obj);
    },
    colorShade: function (color, percent) {

        var R = parseInt(color.substring(1, 3), 16);
        var G = parseInt(color.substring(3, 5), 16);
        var B = parseInt(color.substring(5, 7), 16);

        R = parseInt(R * (100 + percent) / 100);
        G = parseInt(G * (100 + percent) / 100);
        B = parseInt(B * (100 + percent) / 100);

        R = (R < 255) ? R : 255;
        G = (G < 255) ? G : 255;
        B = (B < 255) ? B : 255;

        var RR = ((R.toString(16).length == 1) ? "0" + R.toString(16) : R.toString(16));
        var GG = ((G.toString(16).length == 1) ? "0" + G.toString(16) : G.toString(16));
        var BB = ((B.toString(16).length == 1) ? "0" + B.toString(16) : B.toString(16));

        return "#" + RR + GG + BB;
    },
    map: null,
    tab: {
        name: null,
        isName: function (name) {
            return App.tab.name === name;
        }
    },
    geo: {
        icons: {
            folder: '../images/leaflet-icons/',
            getIconsUrl: function (type) {
                let folder = App.geo.icons.folder;
                let image = { iconUrl: folder, iconRetinaUrl: folder };
                switch (type) {
                    case 'background':
                        image.iconUrl += 'background-marker-icon.png';
                        image.iconRetinaUrl += 'background-marker-icon-2x.png';
                        break;
                    case 'finish':
                        image.iconUrl += 'finish-marker-icon.png';
                        image.iconRetinaUrl += 'finish-marker-icon-2x.png';
                        break;
                    case 'new':
                        image.iconUrl += 'new-marker-icon.png';
                        image.iconRetinaUrl += 'new-marker-icon-2x.png';
                        break;
                    case 'start':
                        image.iconUrl += 'start-marker-icon.png';
                        image.iconRetinaUrl += 'start-marker-icon-2x.png';
                        break;
                    case '':
                    default:
                        image.iconUrl += 'marker-icon.png';
                        image.iconRetinaUrl += 'marker-icon-2x.png';
                        break;
                }
                return image;
            },
            getShadowUrl: function () {
                return App.geo.icons.folder + 'marker-shadow.png';
            },
            getIcon: function (type) {
                let shadowUrl = App.geo.icons.getShadowUrl();
                let urls = App.geo.icons.getIconsUrl(type);
                let leafIcon = L.icon({
                    iconUrl: urls.iconUrl,
                    iconRetinaUrl: urls.iconRetinaUrl,
                    shadowUrl: shadowUrl,
                    iconSize: [25, 41],
                    iconAnchor: [12, 41],
                    popupAnchor: [1, -34],
                    tooltipAnchor: [16, -28],
                    shadowSize: [41, 41]
                });
                return leafIcon;
            }
        },
        setIconById: function (id, type) {
            let i = App.geo.icons.getIcon(type);
            App.geo.getLocationById(id).marker.setIcon(i);
        },
        setView: function (locationId, zoom) {
            if (!locationId) {
                return;
            }
            let location = App.geo.getLocationById(locationId);
            if (!location) {
                return;
            }

            zoom = zoom || App.geo.getZoomToView();
            App.map.setView([location.data.latitude, location.data.longitude], zoom)
        },
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
            let cluster = L.markerClusterGroup();
            if (obj) {
                if (!!obj.length) {
                    for (var i = 0; i < obj.length; i++) {
                        App.geo.locations.push(obj[i]);
                        cluster.addLayer(obj[i].marker);
                        //obj[i].marker.addTo(App.map);
                    }
                } else if (!obj.hasOwnProperty('length')) {
                    App.geo.locations.push(obj);
                    obj.marker.addTo(App.map);
                }
            }
            App.map.addLayer(cluster);
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
                                let icon = App.geo.icons.getIcon();
                                var marker = L.marker([result.data[i].latitude, result.data[i].longitude], { icon: icon });
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
        setAllDefaultIcon: function () {
            App.geo.locations.forEach(function (l) {
                let d = App.geo.icons.getIcon();
                l.marker.setIcon(d);
            });
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
            color: '#1e87f0',
            resetRouteLocations: function (url, id, color, reverse = false) {
                App.geo.route.clear();
                let routeLocations = App.geo.route.getRouteLocations(url, id, reverse);
                App.geo.route.setRouteLocations(routeLocations, id, color);
            },
            getLocations: function () {
                let locations = [];
                for (let routeLocation of App.geo.route.routeLocations) {
                    let location = {};
                    if (routeLocation.routeLocation) {
                        routeLocation = routeLocation.routeLocation;
                    }
                    location.id = routeLocation.id || routeLocation.currentLocationId;
                    location.fullName = routeLocation.fullName || routeLocation.currentFullName;
                    location.name = routeLocation.name || routeLocation.currentName;
                    location.latitude = routeLocation.latitude || routeLocation.currentLatitude;
                    location.longitude = routeLocation.longitude || routeLocation.currentLongitude;
                    location.distance = routeLocation.distance;
                    location.cityId = routeLocation.cityId || routeLocation.currentCityId;
                    location.cityName = routeLocation.cityName || routeLocation.currentCityName;
                    location.districtId = routeLocation.districtId || routeLocation.currentDistrictId;
                    location.districtName = routeLocation.districtName || routeLocation.currentDistrictName;
                    location.countryId = routeLocation.countryId || routeLocation.currentCountryId;
                    location.countryName = routeLocation.countryName || routeLocation.currentCountryName;
                    location.isParking = routeLocation.isParking || routeLocation.currentIsParking;
                    location.isDeleted = routeLocation.isDeleted;
                    locations.push(location);
                }
                return locations;
            },
            setPathOptions: function (options) {
                let pulseColor = options.pulseColor || App.geo.route.color;
                let shade = App.colorShade(pulseColor, 50);
                let _default = { use: L.polyline, delay: 800, dashArray: [10, 10], weight: 6, color: shade, pulseColor: pulseColor };
                if (!options) {
                    options = _default;
                } else {
                    for (let key in _default) {
                        if (!options[key]) {
                            options[key] = _default[key];
                        }
                    }
                }
                if (options && App.geo.route.path.options) {
                    for (let key in options) {
                        if (App.geo.route.path.options[key])
                            App.geo.route.path.options[key] = options[key];
                    }
                    App.map.removeControl(App.geo.route.path);
                    App.map.addControl(App.geo.route.path);
                }
            },
            setRouteLocations: function (routeLocations, id, color) {
                App.geo.setAllDefaultIcon();
                color = color || '#1e87f0';
                let latlngs = [];
                for (var i = 0; i < routeLocations.length; i++) {
                    let id = routeLocations[i].id || routeLocations[i].routeLocation.currentLocationId;
                    let icon = null;
                    if (!i) {
                        icon = App.geo.icons.getIcon('start');
                    }
                    else if (i < routeLocations.length - 1) {
                        icon = App.geo.icons.getIcon('background');

                    } else {
                        icon = App.geo.icons.getIcon('finish');
                    }
                    App.geo.getLocationById(id).marker.setIcon(icon);

                    let leg = routeLocations[i].tomTomLeg;
                    if (routeLocations[i].routeLocation) {
                        leg = routeLocations[i].routeLocation.tomTomLeg;
                    }
                    if (leg) {
                        for (let point of leg.points) {
                            latlngs.push([point.latitude, point.longitude]);
                        }
                    } else {
                        latlngs.push([routeLocations[i].latitude || routeLocations[i].routeLocation.currentLatitude,
                        routeLocations[i].longitude || routeLocations[i].routeLocation.currentLongitude]);
                    }
                }
                let shade = App.colorShade(color, 50);
                let options = { use: L.polyline, delay: 800, dashArray: [10, 10], weight: 6, color: shade, pulseColor: color };
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
                        l.data.distance = routeLocation.routeLocation.distance;
                        locations.push(l.data);
                    }
                }
                return locations;
            },
            getRouteLocations: function (url, id, reverse) {
                var routeLocations = [];
                App.postDataOnServer(url, { id: id, reverse: reverse },
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
            setStartView() {
                let id = App.geo.route.routeLocations[0].routeLocation.currentLocationId;
                if (id) {
                    App.geo.setView(id, 8);
                }
            },
            setViewToRouteLocation(routeLocationId) {
                let routeLocation = App.geo.route.getRouteLocationById(routeLocationId);
                App.map.setView([routeLocation.currentLatitude, routeLocation.currentLongitude], 8);
            },
            getRouteLocationById(routeLocationId) {
                for (let routeLocation of App.geo.route.routeLocations) {
                    if (routeLocation.routeLocation.routeLocationId == routeLocationId) {
                        return routeLocation.routeLocation;
                    }
                }
                return null;
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
        getMaxHeight: function () {
            return $(window).height() - 70;
        },
        getMediumHeight: function () {
            return $(window).height() / 3;
        },
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
                    height: App.footer.getMaxHeight()
                }, App.footer.animateTimer);
                App.footer.slideButton.find('span').attr('uk-icon', 'chevron-down');
                App.footer.mode = -1;
                App.footer.content.fadeIn(App.footer.animateTimer / 1.25);
            } else {
                App.footer.element.stop().animate({
                    height: App.footer.getMediumHeight()
                }, App.footer.animateTimer / 1.5);
                App.footer.slideButton.find('span').attr('uk-icon', 'chevron-up');
                App.footer.mode = 1;
                App.footer.content.fadeIn(App.footer.animateTimer / 1.25);
            }
        }),
        toggle: function () {
            App.footer.slideButton.click();
        },
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
            App.footer.content.html(App.loader);
            if (App.footer.mode !== -1) {
                App.footer.show();
            }
            $.get(url, data, function (html) {
                App.footer.content.html(html);
                UIkit.util.on('.uk-switcher', 'shown', function (event, component) {
                    let url = $(event.target).data('content-url');
                    if (url) {
                        App.loadContent(event.target, url);
                    }
                });
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
            crumbs = crumbs || [];
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
        },
        toggle: function (interval) {
            var toggle = () => UIkit.offcanvas('#offcanvas-slide').toggle();
            if (!interval) {
                toggle();
            } else {
                setTimeout(toggle, interval);
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
        loading: function () {
            Swal.fire({
                icon: 'info',
                title: 'Обработка',
                text: 'Пожалуйста, подождите...',
                showConfirmButton: false,
                timer: 0
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
        _content.empty();
        let loader = $(App.loader);
        _content.html(loader);
        $.get(url, data, function (html) {
            if (handler) {
                handler(html);
            }
            else if (selector) {
                let wrapper = $('<div style="display: none;"></div>');
                wrapper.ready(function () {
                    setTimeout(
                        function () {
                            $(window).resize();
                            loader.remove();
                            wrapper.fadeIn("slow");
                        }, 100);
                });
                wrapper.html(html);
                _content.append(wrapper);
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
$(document).ready(function () {
    $.datetimepicker.setLocale('ru');
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

    $(document).keyup(function (obj) {
        if (!$(document.activeElement).parents().hasClass('uk-offcanvas-container')) {
            if (obj.keyCode === 113) {
                if (App.footer.mode === 1) {
                    App.footer.hide();
                } else if (App.footer.mode === -1) {
                    App.footer.show();
                }
            }
            else if (obj.keyCode === 115) {
                if (App.footer.mode === 1) {
                    App.footer.maximize();
                } else if (App.footer.mode === 0) {
                    App.footer.show();
                }
            }
            else if (obj.keyCode === 27) {
                let isClosedPopup = App.geo.closeAllPopups();
            }
        }
    });

    window.onkeydown = function (obj) {
        if (obj.ctrlKey) {
            if (obj.keyCode === 10) {
                App.menu.toggle();
            }
        }
    }

    var lastWindowWidth;
    $(window).on('resize', function () {
        windowWidth = $(window).width();
        if (lastWindowWidth && lastWindowWidth <= 960 && $(window).width() > 960) {
            App.footer.show();
        }
        lastWindowWidth = windowWidth;
        let customTables = $('.custom-table-resize');
        for (let table of customTables) {
            setTimeout(function () {
                let wrapper = $(table).parents('div.dataTables_wrapper').first();
                let width = wrapper.width();
                $(table).width(width);
            }, 150);
        }
    });

    $(window).on("orientationchange", function (event) {
        setTimeout(function () {
            App.footer.show();
            App.menu.hide();
        }, 50);
    });

    $('#dragging-slider').on('touchmove', function (e) {
        let sliderHeight = $('#dragging-slider').height();
        let maxHeight = $(window).height();
        if (maxHeight - e.touches[0].clientY < maxHeight - sliderHeight)
            App.footer.element.height(maxHeight - e.touches[0].clientY);
    });



    let previousHeight;
    let toCloseSlider;
    $('#dragging-slider').on('touchmove', function (e) {
        e.preventDefault();
        e.stopPropagation();
        let maxHeight = $(window).height();
        let currentHeight = maxHeight - e.touches[0].clientY;
        toCloseSlider = previousHeight > (currentHeight + 10);
        if (toCloseSlider) {
            previousHeight = 0;
        } else {
            previousHeight = currentHeight;
        }
    }); 

    let closeSlider = function (e) {
        if (toCloseSlider) {
            App.footer.hide();
        }
    };
    $('#dragging-slider').off('touchend', closeSlider).on('touchend', closeSlider);

    $('#dragging-slider').on('doubleTap', function () {
        let height = Math.round(App.footer.element.height());
        if (height >= App.footer.getMaxHeight()) {
            App.footer.show();
        } else if (height >= Math.round(App.footer.getMediumHeight() - $('#dragging-slider').height())) {
            App.footer.maximize();
        } else {
            App.footer.show();
        }
    });

    let fullscreenMode;
    let body = document.getElementById('main-container');
    function openFullscreen() {
        if (body.requestFullscreen) {
            body.requestFullscreen();
        } else if (body.mozRequestFullScreen) { /* Firefox */
            body.mozRequestFullScreen();
        } else if (body.webkitRequestFullscreen) { /* Chrome, Safari and Opera */
            body.webkitRequestFullscreen();
        } else if (body.msRequestFullscreen) { /* IE/Edge */
            body.msRequestFullscreen();
        }
        fullscreenMode = true;
    }

    function closeFullscreen() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.mozCancelFullScreen) { /* Firefox */
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) { /* Chrome, Safari and Opera */
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) { /* IE/Edge */
            document.msExitFullscreen();
        }
        fullscreenMode = false;
    }

    $('#fullscreen-mode').click(function () {
        let _this = $(this);
        if (fullscreenMode) {
            closeFullscreen();
            _this.html('<span class="uk-margin-small-right" uk-icon="icon: expand"></span> Полный экран');
        } else {
            openFullscreen();
            _this.html('<span class="uk-margin-small-right" uk-icon="icon: shrink"></span> Обычный экран');
        }
    });
    $('div.leaflet-bottom.leaflet-right').remove();

    if ($(window).width() > 959) {
        if (App.isMobile()) {
            $('#dragging-slider').removeClass('uk-hidden@m').show();
            $('#slide-up').hide();
        } else {
            $('#dragging-slider').hide();
            $('#slide-up').show();
        }
    }
    if (App.isiOS()) {
        $('#fullscreen-mode-item').hide();
    }
});