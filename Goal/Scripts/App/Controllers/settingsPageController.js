(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('settingsPageController', TransactionPageController);

    TransactionPageController.$inject = ['$scope'];

    function TransactionPageController($scope) {
        
        // Injection
        var vm = this;
        vm.$scope = $scope;


    }
})();