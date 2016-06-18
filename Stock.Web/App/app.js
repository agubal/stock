var app = angular.module('stock', ['ngRoute'])
.constant("baseUrl", "http://localhost:25653/api/");

app.config(function ($routeProvider) {

    $routeProvider.when("/", {
        controller: "stockCtrl",
        templateUrl: "/app/views/stock.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });

});
