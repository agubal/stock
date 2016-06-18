'use strict';
app.factory('apiClientService', [
    '$http', 'baseUrl', function ($http, baseUrl) {

        $http.defaults.useXDomain = true;
        var apiClientFactory = {};

        var get = function (url) {
            return $http.get(baseUrl + url);
        }

        var post = function (url, body) {
            return $http.post(baseUrl + url, body);
        }

        apiClientFactory.get = get;
        apiClientFactory.post = post;
        return apiClientFactory;
    }
]);