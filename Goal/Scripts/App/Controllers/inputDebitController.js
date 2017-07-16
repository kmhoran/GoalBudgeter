(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('inputDebitController', InputDebitController);

    InputDebitController.$inject = ['$scope', '$rootScope', '$route', '$logHttpService'];

    function InputDebitController ($scope, $rootScope, $route, $logHttpService) {
        // Inject
        var vm = this;
        vm.$scope = $scope;
        vm.$rootScope = $rootScope;
        vm.$route = $route;
        vm.$logHttpService = $logHttpService;

        // Methods
        vm.submitForm = _submitForm;
        vm.refreshForm = _refreshRoute;

        // Properties
        vm.type = "debit";
        vm.title = "Money Spent";
        vm.amount = null;
        vm.categories = $scope.$parent.test;
        vm.category = null;
        vm.date = new Date();
        vm.description = null;




        //_insertCredit();
        // /////////////////////////////////////////////////////////////////////////////////////////

        function _submitForm(isFormValid) {
            if (isFormValid) {
                _insertDebitit();
            }
        }


        // .........................................................................................

        function _insertDebitit() {

            var data = {
                amount: vm.amount,
                category: vm.category,
                date: vm.date.toLocaleDateString(),
                description: vm.description,
                // typeId 0 == Debit
                typeId: 0
            };

            // TODO FIXME Delete
            console.log("payload: ", data);

            vm.$logHttpService.insertTransaction(data)
            .then(function handleCreditInsert(transactionData) {
                console.log("HttpCreditData: ", transactionData.data);
            }).catch(_showError);

            $rootScope.$emit("onToastrSuccess", "Debit Added Successfully!");

            _refreshRoute();

            $rootScope.$emit("RefreshMonth");
        }



        // .........................................................................................

        function _refreshRoute() {
            vm.$route.reload();
        }

        // .........................................................................................

        function _clearFields() {
            vm.amount = null;
            vm.category = null;
            vm.date = new Date();
            vm.description = null;
        }

        // .........................................................................................

        function _showError(error) {
            console.log("Something went wrong: ", error);
        }

    }

})();