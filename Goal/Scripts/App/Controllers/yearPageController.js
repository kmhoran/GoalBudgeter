(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('yearPageController', YearPageController);

    YearPageController.$inject = ['$scope'];

    function YearPageController($scope) {

        // Injection
        var vm = this;
        vm.$scope = $scope;


    }
})();