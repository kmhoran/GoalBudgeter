(function () {
    "use strict";

    angular.module(APPNAME)
    .controller("newUserController", NewUserController);

    NewUserController.$inject = ['$scope', '$uibModal'];

    function NewUserController($scope, $uibModal) {
        // Injection
        var vm = this;
        vm.$scope = $scope;
        vm.$uibModal = $uibModal;


        // Startup Functions
        console.log("newUserControllerConnected");
        _openNewUserModal();

        // /////////////////////////////////////////////////////////////////////////////////////////

        function _openNewUserModal() {
            var modalInstance = vm.$uibModal.open({
                animation: true,
                templateUrl: '/Scripts/App/Templates/newUserModal.html',
                controller: 'newUserModalController as nUsrMC',
                size: 'md',
                backdrop  : 'static',
                keyboard  : false,
                resolve: {}
            });

            //  when the modal closes it returns a promise
            modalInstance.result.then(function (userPreferences) {
                    //  if the user closed the modal by clicking Save  
            }, function () {
                console.log('Modal dismissed at: ' + new Date());   //  if the user closed the modal by clicking cancel
            });
        }
    }
})();