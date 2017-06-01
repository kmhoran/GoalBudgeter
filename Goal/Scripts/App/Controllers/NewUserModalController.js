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
            startingAmount: null,
            goalAmount: null
        };

        // Methods
        vm.submitForm = _submitForm;
        //vm.cancel = _cancel;

        // /////////////////////////////////////////////////////////////////////////////////////////

        function _submitForm(isValid) {
            if (isValid) {
                vm.$uibModalInstance.close(vm.preferences);
            }
        };


        // .........................................................................................

        //function _cancel() {
        //    vm.$uibModalInstance.dismiss('cancel');
        //};
    }
})();