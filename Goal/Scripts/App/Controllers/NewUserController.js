(function () {
    "use strict";

    angular.module(APPNAME)
    .controller("newUserController", NewUserController);

    NewUserController.$inject = ['$scope', '$rootScope', '$timeout', '$uibModal', '$logHttpService'];

    function NewUserController($scope, $rootScope, $timeout, $uibModal, $logHttpService) {
        // Injection
        var vm = this;
        vm.$scope = $scope;
        vm.$rootScope = $rootScope;
        vm.$uibModal = $uibModal;
        vm.$logHttpService = $logHttpService;


        // Startup Functions
        $timeout(_openNewUserModal(), 3000);
        

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

            modalInstance.result.then(function (userPreferences) {

                console.log("your preferences: ", userPreferences);

                //vm.$logHttpService.initializeUserLog(userPreferences)
                //.then(function displaydata(data) {
                //    console.log("http data: ", data)
                //}).catch(_showError);
            });
        }


        // .........................................................................................

        function _showError(error) {
            console.log("Something went wrong: ", error);
        }

    }
})();