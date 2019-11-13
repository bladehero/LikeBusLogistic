window.getCookie = function (name) {
    var match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    if (match) return match[2];
};

var app = {
    authorizeFetch: function (url, method, data, mode, cache, credentials) {
        var authCookie = window.getCookie('Bearer');
        return fetch(url, {
            method: method | 'GET',
            mode: mode | 'cors',
            cache: cache | 'no-cache',
            credentials: credentials | 'same-origin',
            headers: {
                'Authorization': 'Bearer ' + authCookie
            },
            body: JSON.stringify(data)
        });
    },
    getPage: function (url) {
        debugger;
        var authCookie = window.getCookie('Bearer');
        fetch(url, {
            method: 'GET',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {
                'Authorization': 'Bearer ' + authCookie
            }
        }).then((response) => {
            if (response.ok) {
                return response.text();
            } else {
                console.log(response);
            }
        }).then(function(html) {
            document.open();
            document.write(html);
            document.close();
        }).catch(function (data) {
            console.log(data);
        });
    }
};