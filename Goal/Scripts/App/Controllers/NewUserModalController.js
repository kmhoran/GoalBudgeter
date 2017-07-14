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
        //vm.partOne = true;

        vm.preferences = {
            startingAmount: null,
            goalAmount: null
        };

        // Methods
        vm.submitForm = _submitForm;
        //vm.toggleForm = _toggleForm;
        //vm.cancel = _cancel;

        // /////////////////////////////////////////////////////////////////////////////////////////

        function _toggleForm() {
            if (vm.partOne) {
                vm.partOne = false;
            } else {
                vm.partOne = true;
            }
        }

        // .........................................................................................

        function _submitForm() {
         
                //console.log("preferences: ",vm.preferences);
               vm.$uibModalInstance.close(vm.preferences);
          
        };


        // .........................................................................................

        //function _cancel() {
        //    vm.$uibModalInstance.dismiss('cancel');
        //};
    }
})();