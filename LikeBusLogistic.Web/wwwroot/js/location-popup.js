$(document).ready(function () {
    function addLocationToRoute(_popup, locationId, routeLocationId, mode) {
        Swal.fire({
            title: 'Добавление',
            text: 'Нажмите на локацию, чтобы добавить в маршрут',
            icon: 'info',
            showConfirmButton: false,
            timer: 1500
        });
        const firstLocation = App.geo.route.routeLocations[0].currentLocationId;
        const lastLocation = App.geo.route.routeLocations[App.geo.route.routeLocations.length - 1].currentLocationId;
        const isRouteLocation = (element) => element.currentLocationId == locationId;
        for (let i = 0; i < App.geo.locations.length; i++) {
            let _this = App.geo.locations[i];
            _this.marker.off('click').on('click', function () {
                let hasError = false;
                let errorMessage;
                if (routeLocationId && App.geo.route.routeLocations.some(isRouteLocation)) {
                    hasError = true;
                    errorMessage = 'Невозможно добавить в маршрут уже добавленную ранее локацию!';
                }
                if (!routeLocationId) {
                    if (!App.geo.route.routeLocations.some(isRouteLocation)) {
                        errorMessage = 'Локация должна быть прикреплена к локации, которая уже есть в маршруте!';
                        hasError = true;
                    } else if (firstLocation == _this.data.id && mode == App.mergeRouteLocationMode.append) {
                        errorMessage = 'Локация не может быть добавлена до точки отправления!';
                        hasError = true;
                    } else if (lastLocation == _this.data.id && mode == App.mergeRouteLocationMode.prepend) {
                        errorMessage = 'Локация не может быть добавлена после точки прибытия!';
                        hasError = true;
                    }
                }

                if (hasError) {
                    App.message.showErrorWithOk(
                        'Ошибка',
                        errorMessage
                    );
                    return;
                }

                App.footer.getContent('/Route/_MergeRouteLocation/',
                {
                    routeId: App.geo.route.id,
                    routeLocationId: !!routeLocationId ? locationId : _this.data.id,
                    locationToAddId: !routeLocationId ? locationId : _this.data.id,
                    mode: mode
                });
            });
        }
        App.geo.closeAllPopups();
    }
    $(document).on('click', '#specialists-link', function () {
        var id = $(this).parents('#location-popup').data('location-id');
        App.loadContent(
            '#specialists-modal div.uk-modal-body',
            '/Geolocation/_RepairSpecialistsForLocation/',
            { locationId: id }
        );
    });
    $(document).on('click', '#create-route-button,#delete-route-location-button,#append-route-location-button,#prepend-route-location-button', function () {
        App.geo.closeAllPopups();
    });
    $(document).on('click', '#create-route-button', function () {
        App.footer.getContent('/Route/_CreateRoute/',
            { startLocationId: $('#location-popup').data('location-id') });
    });
    $(document).on('click', '#append-route-location-button', function () {
        let _this = $(this);
        let _popup = _this.parents('#location-popup');
        let locationId = _popup.data('location-id');
        let routeLocationId = _popup.data('route-location-id');
        addLocationToRoute(_popup, locationId, routeLocationId, App.mergeRouteLocationMode.append);
    });
    $(document).on('click', '#prepend-route-location-button', function () {
        let _this = $(this);
        let _popup = _this.parents('#location-popup');
        let locationId = _popup.data('location-id');
        let routeLocationId = _popup.data('route-location-id');
        addLocationToRoute(_popup, locationId, routeLocationId, App.mergeRouteLocationMode.prepend);
    });
    $(document).on('click', '#delete-route-location-button', function () {
        Swal.fire({
            title: 'Удаление',
            text: 'Вы уверены, что хотите удалить эту локацию из текущего маршрута?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Да',
            cancelButtonText: 'Отмена'
        }).then((result) => {
            if (result.value) {
                let _this = $(this);
                let _popup = _this.parents('#location-popup');
                let locationId = _popup.data('location-id');
                var routeId = App.geo.route.id;
                App.postDataOnServer('/Route/DeleteRouteLocation/', {
                    routeId: routeId,
                    locationId: locationId
                }, (result) => {
                    App.geo.route.setRouteLocations(result.data, routeId);
                    Swal.fire({
                        title: 'Удалено',
                        text: 'Локация успешно удалена из маршрута',
                        icon: 'success',
                        showConfirmButton: false,
                        timer: 1500
                    });
                }, null, 'POST');
            }
        });

    });
});