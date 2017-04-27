(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('inputDebitController', InputDebitController);

    InputDebitController.$inject = ['$scope'];

    function InputDebitController($scope) {
        // Inject
        var vm = this;
        vm.$scope = $scope;

        // Properties
        vm.title = "Money Spent";

    };

})();