$(document).ready(function () {
    if (!$('input[name="id"]').val()) {
        App.map.on('mousemove', setLatLng);
        App.map.on('click', clickLocation);
    }
    else {
        App.map.setView([Number($('input[name="latitude"]').val()), Number($('input[name="longtitude"]').val())], App.geo.getZoomToView());
    }

    function setLatLng(obj) {
        $('input[name="longtitude"]').val(obj.latlng.lng);
        $('input[name="latitude"]').val(obj.latlng.lat);
    }
    function clickLocation(obj) {
        App.map.off('mousemove');
        App.map.off('click');
        if ($('#toggle-animation').attr('hidden')) {
            UIkit.toggle('#toggler').toggle();
        }
        var marker = L.marker([obj.latlng.lat, obj.latlng.lng])
            .addTo(App.map)
            .bindPopup('<p class="uk-text-meta uk-margin-remove">Заполните все поля для добавления локации! Или можете <button class="uk-button uk-button-link uk-text-lowercase" id="new-location-delete">удалить</button>.</p>')
            .openPopup();
        App.footer.maximize(800);
        $('#cancel-location-btn,#save-location-btn').click(function () {
            App.map.removeControl(marker);
        });
        function removeMarker() {
            App.map.removeControl(marker);
            $('.menu-item').off('click', removeMarker);
        }
        $('.menu-item').click(removeMarker);
        $(document).on('click', '#new-location-delete', function () {
            removeMarker();
            clearForm();
            App.map.on('mousemove', setLatLng);
            App.map.on('click', clickLocation);
            App.footer.show();
        });
        App.map.setView([obj.latlng.lat, obj.latlng.lng], App.geo.getZoomToView());

        if (window.screen.width >= 992) {
            setTimeout(() => { $('#location-grid input[name="name"]').focus(); }, 1000);
        }

        function deleteLocation(obj) {
            if (obj.keyCode === 27) {
                clearForm();
                App.map.removeControl(marker);
                App.map.on('mousemove', setLatLng);
                App.map.on('click', clickLocation);
                App.footer.show();
                $(document).off('keydown', null, deleteLocation);
            }
        }
        $(document).keydown(deleteLocation);
    }
    function clearForm() {
        var grid = $('#location-grid');
        grid.find('input').val(null);
        grid.find('select').each(function (i, el) {
            $(el).val($(el).find('option:first').val());
        });
    }

    window.onresize = function () {
        if ($('#toggle-animation').attr('hidden')) {
            UIkit.toggle('#toggler').toggle();
        }
    };
    UIkit.util.on(document, 'show', '#toggle-animation', function () {
        var toggler = $('#toggler');
        toggler.removeClass('uk-button-primary')
            .addClass('uk-button-default')
            .text('Свернуть');
    });
    UIkit.util.on(document, 'hide', '#toggle-animation', function () {
        var toggler = $('#toggler');
        toggler.removeClass('uk-button-default')
            .addClass('uk-button-primary')
            .text('Показать');
    });

    $('#cancel-location-btn').click(function () {
        clearForm();
    });
    $('#cancel-location-btn,#save-location-btn').click(function () {
        App.map.on('mousemove', setLatLng);
        App.map.on('click', clickLocation);
        App.footer.show();
    });
});