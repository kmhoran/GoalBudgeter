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

        // /////////////////////////////////////////////////////////////////////////////////////////

        function _submitForm() {
         
                //console.log("preferences: ",vm.preferences);
               vm.$uibModalInstance.close(vm.preferences);
          
        };
    }
})();