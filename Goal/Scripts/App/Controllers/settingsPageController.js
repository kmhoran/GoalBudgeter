﻿(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('settingsPageController', TransactionPageController);

    TransactionPageController.$inject = ['$scope', '$rootScope', '$logHttpService'];

    function TransactionPageController($scope, $rootScope, $logHttpService) {
        
        // Injection
        var vm = this;
        vm.$scope = $scope;
        vm.$logHttpService = $logHttpService;


        // Methods
        vm.addCredit = _addCredit;
        vm.addDebit = _addDebit;

        // Properties
        vm.creditToAdd = null;
        vm.debitToAdd = null;

        // /////////////////////////////////////////////////////////////////////////////////////////

        function _addCredit() {
            if (vm.creditToAdd != null && vm.creditToAdd.length > 0) {
                // Credit == typeId 1
                _addCategory(vm.creditToAdd, 1);
                vm.creditToAdd = null;
            }
        }


        // .........................................................................................

        function _addDebit() {
            if (vm.debitToAdd != null && vm.debitToAdd.length > 0) {
                // Debit == typeId 0
                _addCategory(vm.debitToAdd, 0);
                vm.debitToAdd = null;   
            }
        }


        // .........................................................................................

        function _addCategory(name, typeId) {

            var payload = {
                name: name,
                typeId: typeId
            };

            vm.$logHttpService.insertTransactionCategory(payload)
            .then(function handleCategoryAdd() {
                $rootScope.$emit("onToastrSuccess", "Category Added Successfully!");
                $rootScope.$emit("RefreshCategories");
            }).catch(_showError);
            
        }


        // .........................................................................................

        function _showError(error) {
            console.log("Something went wrong: ", error);
        }


    }
})();