(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('globalController', GlobalController);

    GlobalController.$inject = ['$scope', '$rootScope'];

    function GlobalController($scope, $rootScope) {

        // Injection
        var vm = this;
        vm.$scope = $scope;
        vm.$rootScope = $rootScope;

        // Properties
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-center",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };


        // /////////////////////////////////////////////////////////////////////////////////////////

        vm.$rootScope.$on("onToastrSuccess", function (event, messageIn) {
            _onToastrSuccess(messageIn);
        });


        function _onToastrSuccess(message) {
            toastr.success(message);
        }



    }
})();