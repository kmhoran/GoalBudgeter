(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('splashPageController', SplashPageController);

    SplashPageController.$inject = ['$scope'];

    function SplashPageController($scope) {
        // Inject
        var vm = this;
        vm.$scope = $scope;

    };

})();