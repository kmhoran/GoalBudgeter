(function () {
    "use strict";

    angular.module(APPNAME)
    .controller("externalController", ExternalController);

    ExternalController.$inject = ["$scope"];

    function ExternalController($scope) {

        // Injection
        var vm = this;
        vm.$scope = $scope;


        // Methods
        vm.emailSent = _emailSent;
        // /////////////////////////////////////////////////////////////////////////////////////////

        function _emailSent(){
            _onToastrSuccess("Check your email. A confirmation has been sent!");
        }

        function _onToastrSuccess(message) {
            toastr.success(message);
        }
    }
})();