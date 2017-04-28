/// <reference path="../Templates/splash-index.html" />
/// <reference path="../Templates/settings.html" />
(function () {
    "use strict";

    angular.module(APPNAME)
        .config(["$routeProvider", "$locationProvider",
            function ($routeProvider, $locationProvider) {

                $routeProvider.when('/', {
                    // Splash page
                    templateUrl: '/Scripts/App/Templates/splash.html',
                    controller: 'splashPageController',
                    controllerAs: 'SplC'
                }).when('/settings', {
                    // Settings
                    templateUrl: '/Scripts/App/Templates/settings.html',
                    controller: 'settingsPageController',
                    controllerAs: 'SetC'
                }).when('/inputcredit', {
                    // Credit input
                    templateUrl: '/Scripts/App/Templates/input.html',
                    controller: 'inputCreditController',
                    controllerAs: 'InC'
                }).when('/inputdebit', {
                    // Debit input
                    templateUrl: '/Scripts/App/Templates/input.html',
                    controller: 'inputDebitController',
                    controllerAs: 'InC'
                }).when('/log', {
                    // Log
                    templateUrl: '/Scripts/App/Templates/log.html',
                    controller: 'logPageController',
                    controllerAs: 'LogC'
                }).when('/month', {
                    // Month
                    templateUrl: '/Scripts/App/Templates/month.html',
                    controller: 'monthPageController',
                    controllerAs: 'MonC'
                }).when('/year', {
                    // Year
                    templateUrl: '/Scripts/App/Templates/year.html',
                    controller: 'yearPageController',
                    controllerAs: 'YrC'
                }).when('/wishlist', {
                    // Wishlist
                    templateUrl: '/Scripts/App/Templates/wishlist.html',
                    controller: 'wishlistPageController',
                    controllerAs: 'WlC'
                });

                $locationProvider.html5Mode(false).hashPrefix('');

            }]);

})();