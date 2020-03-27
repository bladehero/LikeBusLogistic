function addLocationToRoute(_popup, locationId, routeLocationId, mode, spliceHandler) {
    Swal.fire({
        title: 'Добавление',
        text: 'Нажмите на локацию, чтобы добавить в маршрут',
        icon: 'info',
        showConfirmButton: false,
        timer: 1500
    });
    const isRouteLocation = (element) => element.routeLocation.currentLocationId == locationId;
    for (let i = 0; i < App.geo.locations.length; i++) {
        let _this = App.geo.locations[i];
        _this.marker.off('click').on('click', function () {
            if (routeLocationId) {
                if (!App.geo.route.routeLocations.some(isRouteLocation)) {
                    App.message.showErrorWithOk(
                        'Ошибка',
                        'Невозможно добавить в маршрут уже добавленную ранее локацию!'
                    );
                    return;
                }

                let existedLocation = _this;
                let firstId = mode ? existedLocation.data.id : locationId;
                let lastId = mode ? locationId : existedLocation.id;
                let leg = null;
                App.postDataOnServer('/Route/GetDistanceInfo/',
                    { locationId1: firstId, locationId2: lastId },
                    function (result) {
                        if (result.success) {
                            leg = result.data;
                        }
                    },
                    null,
                    'GET',
                    false
                );
                var latlng = L.latLng(_this.marker._latlng.lat, _this.marker._latlng.lng);
                var latlngs = App.geo.route.path.getLatLngs();
                spliceHandler(latlngs, latlng);
                App.geo.route.path.setLatLngs(latlngs);
                App.footer.getContent('/Route/_MergeRouteLocation/',
                    {
                        routeId: App.geo.route.id,
                        routeLocationId: locationId,
                        locationToAddId: _this.data.id,
                        mode: mode
                    });
            }
            else {
                if (App.geo.route.routeLocations.some(isRouteLocation)) {
                    App.message.showErrorWithOk(
                        'Ошибка',
                        'Локация должна быть прикреплена к локации, которая уже есть в маршруте!'
                    );
                    return;
                }

                let locationToAdd = App.geo.getLocationById(locationId);
                var latlng = L.latLng(locationToAdd.marker._latlng.lat, locationToAdd.marker._latlng.lng);
                var latlngs = App.geo.route.path.getLatLngs();
                spliceHandler(latlngs, latlng, _this.data.id);
                App.geo.route.path.setLatLngs(latlngs);
                App.footer.getContent('/Route/"_MergeRouteLocation/',
                    {
                        routeId: App.geo.route.id,
                        routeLocationId: _this.data.id,
                        locationToAddId: locationId,
                        mode: mode
                    });
            }
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

    const spliceHandler = (latlngs, latlng, routeLocationToAddId) => { latlngs.splice(App.geo.route.getIndexByRouteLocationId(routeLocationId || routeLocationToAddId), 0, latlng) }
    addLocationToRoute(_popup, locationId, routeLocationId, App.mergeRouteLocationMode.append, spliceHandler);
    });
$(document).on('click', '#prepend-route-location-button', function () {
    let _this = $(this);
    let _popup = _this.parents('#location-popup');
    let locationId = _popup.data('location-id');
    let routeLocationId = _popup.data('route-location-id');

    const spliceHandler = (latlngs, latlng, routeLocationToAddId) => { latlngs.splice(App.geo.route.getIndexByRouteLocationId(routeLocationId || routeLocationToAddId) + 1, 0, latlng) }
    addLocationToRoute(_popup, locationId, routeLocationId, App.mergeRouteLocationMode.prepend, spliceHandler);
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
            }, () => {
                App.geo.route.resetRouteLocations('/Route/GetRouteLocations/', routeId);
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