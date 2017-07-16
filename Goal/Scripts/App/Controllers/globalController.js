﻿(function () {
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

        vm.credits = null;
        
        vm.debits = null;
        vm.test = "TEST";
        vm.month = {};
        vm.monthGoalAmount = 0;

        // Methods 
        vm.updateCreditForecastSettings = _updateCreditForecastSettings;
        vm.updateDebitForecastSettings = _updateDebitForecastSettings;
        vm.getTransactionCategories = _getTransactionCategories;
        vm.setMonthGoal = _setMonthGoal;
        vm.updatePreferences = _updatePreferences;
        vm.refreshPreferences = _refreshPreferences;

        // Startup Functions
        _getTransactionCategories();
        _getCurrentMonth();

        // /////////////////////////////////////////////////////////////////////////////////////////
        // $rootscope broadcasting

        vm.$rootScope.$on("onToastrSuccess", function (event, messageIn) {
            _onToastrSuccess(messageIn);
        });

        vm.$rootScope.$on("RefreshCategories", function () {
            _getTransactionCategories();
        })

        vm.$rootScope.$on("RefreshMonth", function () {
            _getCurrentMonth();
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

        function _getCurrentMonth() {
            $logHttpService.getCurrentMonth()
            .then(function populateCurrentMonth(thisMonth) {

                vm.month = thisMonth.data.Item;

                vm.monthGoalAmount = vm.month.GoalType == "Yearly" ? vm.month.CalculatedMonthGoal : vm.month.FixedMonthGoal;
            })
        }


        // .........................................................................................

        function _updateCreditForecastSettings() {

            var categoryList = vm.credits.map(function mapCreditPayload(obj) {
                var toReturn = {
                    CategoryId: obj.CategoryId
                    , Name: obj.Name
                    , UserId: ""
                    , TypeId: obj.TypeId
                    , ForecastType: obj.ForecastType
                    , FixedPrediction: obj.Predictions.Fixed
                };

                return toReturn;
            });

            var payload = { CategoryList: categoryList };

            $logHttpService.updateCategories(payload)
            .then(function updateCreditSuccess() {
                var messageIn = "Income Forecasting Updated Successfully!";
                _onToastrSuccess(messageIn)
                _getTransactionCategories();
            }).catch(_showError)
        }


        // .........................................................................................

        function _updateDebitForecastSettings() {

            var categoryList = vm.debits.map(function mapDebitPayload (obj) {
                var toReturn = {
                    CategoryId: obj.CategoryId
                    ,Name: obj.Name
                    ,UserId: ""
                    , TypeId: obj.TypeId
                    , ForecastType: obj.ForecastType
                    ,FixedPrediction: obj.Predictions.Fixed
                };

                return toReturn;
            });

            var payload = { CategoryList: categoryList };


            $logHttpService.updateCategories(payload)
            .then(function updatedebitSuccess() {
                var messageIn = "Expese Forecasting Updated Successfully!";
                _onToastrSuccess(messageIn)
                _getTransactionCategories();
            }).catch(_showError)
        }



        // .........................................................................................

        function _updatePreferences() {
            var payload = {
                preferenceId: vm.month.PreferenceId
                , MonthlyGoalType: vm.month.GoalType
                , MonthlyFixedGoal: vm.month.FixedMonthGoal
            };

            $logHttpService.updatePreferences(payload)
            .then(function handlePreferenceUpdateSuccess(message) {
                console.log("preference response: ", message);
                if (message.data.IsSuccessful) {
                    _onToastrSuccess("Preferences Updated");
                }
            })
        }



        // .........................................................................................

        function _refreshPreferences() {
            _getCurrentMonth();
        }

        // .........................................................................................
        function _setMonthGoal(type){
            vm.month.GoalType = type;
        }

        // .........................................................................................

        function _showError(error) {
            console.log("Something went wrong: ", error);
        }



    }
})();