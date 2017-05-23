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
        vm.test = "TEST"

        // Methods 
        vm.updateCreditForecastSettings = _updateCreditForecastSettings;
        vm.updateDebitForecastSettings = _updateDebitForecastSettings;
        vm.getTransactionCategories = _getTransactionCategories;

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
                vm.debits = categories.data.Item.Debits;

                $rootScope.$broadcast("RefreshCreditCategories")

            }).catch(_showError);
        }


        // .........................................................................................

        function _updateCreditForecastSettings() {
            var payload = {CategoryList: vm.credits};
            $logHttpService.updateCategories(payload)
            .then(function updateCreditSuccess() {
                var messageIn = "Income Forecasting Updated Successfully!";
                _onToastrSuccess(messageIn)
                _getTransactionCategories();
            }).catch(_showError)
        }


        // .........................................................................................

        function _updateDebitForecastSettings() {
            var payload = { CategoryList: vm.debits };
            $logHttpService.updateCategories(payload)
            .then(function updatedebitSuccess() {
                var messageIn = "Expese Forecasting Updated Successfully!";
                _onToastrSuccess(messageIn)
                _getTransactionCategories();
            }).catch(_showError)
        }



        // .........................................................................................

        function _showError(error) {
            console.log("Something went wrong: ", error);
        }



    }
})();