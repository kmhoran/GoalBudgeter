(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('newUserModalController', NewUserModalController);

    NewUserModalController.$inject = ['$scope', '$uibModalInstance'];

    function NewUserModalController(
        $scope
        , $uibModalInstance) {

        // Injection
        var vm = this;

        vm.$scope = $scope;
        vm.$uibModalInstance = $uibModalInstance;

        // Properties
        vm.preferences = {
            startingAmount: 0.00,
            goalAmount: 0
        };

        // Methods
        vm.confirm = _confirm;
        //vm.cancel = _cancel;

        // /////////////////////////////////////////////////////////////////////////////////////////

        function _confirm() {
            vm.$uibModalInstance.close(vm.preferences);
        };


        // .........................................................................................

        //function _cancel() {
        //    vm.$uibModalInstance.dismiss('cancel');
        //};
    }
})();