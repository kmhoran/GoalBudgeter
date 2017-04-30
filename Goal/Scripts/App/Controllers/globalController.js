(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('globalController', GlobalController);

    GlobalController.$inject = ['$scope', '$rootScope', '$logHttpService'];

    function GlobalController($scope, $rootScope, $logHttpService) {

        // Injection
        var vm = this;
        vm.$scope = $scope;
        vm.$rootScope = $rootScope;
        vm.$logHttpService = $logHttpService;

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

        vm.credits = null;
        vm.debits = null;

        // Startup Functions
        _getTransactionCategories()

        // /////////////////////////////////////////////////////////////////////////////////////////
        // $rootscope broadcasting

        vm.$rootScope.$on("onToastrSuccess", function (event, messageIn) {
            _onToastrSuccess(messageIn);
        });

        vm.$rootScope.$on("RefreshCategories", function () {
            _getTransactionCategories();
        })
        // /////////////////////////////////////////////////////////////////////////////////////////

        function _onToastrSuccess(message) {
            toastr.success(message);
        }


        // .........................................................................................

        function _getTransactionCategories() {
            $logHttpService.getTransactionCategories()
            .then(function populateCategories(categories) {
                vm.credits = categories.data.Item.Credits;
                vm.debits = categories.data.Item.Debits

            }).catch(_showError);
        }


        // .........................................................................................

        function _showError(error) {
            console.log("Something went wrong: ", error);
        }



    }
})();