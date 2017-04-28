(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('inputCreditController', InputCreditController);

    InputCreditController.$inject = ['$scope', '$logHttpService'];

    function InputCreditController($scope, $logHttpService) {
        // Inject
        var vm = this;
        vm.$scope = $scope;
        vm.$logHttpService = $logHttpService;

        // Methods
        vm.submitForm = _submitForm;

        // Properties
        vm.title = "Money Earned";
        vm.amount = null;
        vm.category = null;
        vm.date = new Date();
        vm.description = null;
        vm.null = null;

        //_insertCredit();
        // /////////////////////////////////////////////////////////////////////////////////////////

        function _submitForm(isFormValid) {
            if (isFormValid) {
                _insertCredit();
            }
        }


        // .........................................................................................

        function _insertCredit() {

            var data = {
                amount: vm.amount,
                category: vm.category,
                date: vm.date.toLocaleDateString(),
                description: vm.description,
                // typeId 1 == Credit
                typeId: 1
            };

            // TODO FIXME Delete
            console.log("payload: ", data);

            vm.$logHttpService.insertTransaction(data)
            .then(function handleCreditInsert(transactionData) {
                console.log("HttpCreditData: ", transactionData.data);
            }).catch(_showError);

        }


        // .........................................................................................

        function _showError(error) {
            console.log("Something went wrong: ", error);
        }

    }

})();