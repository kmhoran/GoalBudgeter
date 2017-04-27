(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('monthPageController', MonthPageController);

    MonthPageController.$inject = ['$scope'];

    function MonthPageController($scope) {

        // Injection
        var vm = this;
        vm.$scope = $scope;


    }
})();