(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('logPageController', LogPageController);

    LogPageController.$inject = ['$scope'];

    function LogPageController($scope) {

        // Injection
        var vm = this;
        vm.$scope = $scope;


    }
})();