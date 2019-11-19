$(document).ready(function () { if (localStorage.getItem('isLoggedOff') === 'true') { app.useContentState(); localStorage.setItem('isLoggedOff', 'false'); }});
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