(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('globalController', GlobalController);

    GlobalController.$inject = ['$scope', '$rootScope', '$logHttpService', '$reportService'];

    function GlobalController($scope, $rootScope, $logHttpService, $reportService) {

        // Injection
        var vm = this;
        vm.$scope = $scope;
        vm.$rootScope = $rootScope;
        vm.$logHttpService = $logHttpService;
        vm.$reportService = $reportService;

        // Properties

        vm.credits = null;
        
        vm.debits = null;
        vm.test = "TEST";
        vm.month = {};
        vm.monthGoalAmount = 0;

        

        // --> reports
        vm.reports = {
            categorySpending: [
                    { name: "Gifts", pred: 999, real: 500 },
                    { name: "other", pred: 888, real: 400 },
                    { name: "Health", pred: 666, real: 300 },
            ]
        };

        vm.delinquentCategories = [];
        //--

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
        _getLatestReports();
        // /////////////////////////////////////////////////////////////////////////////////////////
        // $rootscope broadcasting

        vm.$rootScope.$on("onToastrSuccess", function (event, messageIn) {
            _onToastrSuccess(messageIn);
        });

        vm.$rootScope.$on("RefreshCategories", function () {
            _getTransactionCategories();
            _getCurrentMonth();
            _getLatestReports();
        })

        vm.$rootScope.$on("RefreshMonth", function () {
            _getCurrentMonth();
            _getLatestReports();
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

        function _getLatestReports() {
            _reportGetMonthlySpendingByCategory();
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


        // --| Reports |--------------------------------------------------------------------------->

        function _reportGetMonthlySpendingByCategory() {
            $reportService.getMonthlySpendingByCategory()
            .then(function (response) {
                console.log("reports: ", response);
                var report = response.data.Items;

                if(report != null && report.length > 0)
                {
                    var result = [];
                    for (var i = 0, len = report.length; i < len; i++) {

                        // Get 
                        var obj = {
                            name: report[i].CategoryName,
                            pred: report[i].Predicted,
                            real: report[i].Real
                        }

                        if (report[i].Predicted < report[i].Real) {
                            vm.delinquentCategories.push(report[i].CategoryName);
                        }

                        result.push(obj);
                    }
                    vm.reports.categorySpending = result;
                }
            })
        }


    }
})();