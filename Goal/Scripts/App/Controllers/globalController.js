(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('globalController', GlobalController);

    GlobalController.$inject = ['$scope'];

    function GlobalController($scope) {

        // Injection
        var vm = this;
        vm.$scope = $scope;



    }
})();