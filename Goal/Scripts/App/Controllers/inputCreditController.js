(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('inputCreditController', InputCreditController);

    InputCreditController.$inject = ['$scope'];

    function InputCreditController($scope) {
        // Inject
        var vm = this;
        vm.$scope = $scope;

        // Properties
        vm.title = "Money Earned";

    };

})();