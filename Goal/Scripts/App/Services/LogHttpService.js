(function () {
    "use strict";

    angular.module(APPNAME)
    .factory("$logHttpService", LogHttpService);

    LogHttpService.$inject = ['$http'];

    function LogHttpService($http) {
        
        var baseUrl = "/api/log"

        // Public Methods
        return {
            insertTransaction: _insertTransaction
            ,getTransactionCategories: _getTransactionCategories
            ,insertTransactionCategory: _insertTransactionCategory
            ,updateCategories: _updateCategories
            ,deleteCategory: _deleteCategory
            ,getCurrentMonth: _getCurrentMonth
            ,initializeUserLog: _initializeUserLog
        };

        // /////////////////////////////////////////////////////////////////////////////////////////

        // --| Transactions |---------------------------------------------------------------------->

        function _insertTransaction(data) {
            var settings = {
                method: "POST",
                url: baseUrl.concat("/transaction"),
                data: data
            };

            return $http(settings);

        }

        // .........................................................................................

        function _getTransactionCategories() {
            var settings = {
                method: "GET",
                url: baseUrl.concat("/categories")
            };

            return $http(settings);
        }


        // .........................................................................................

        function _insertTransactionCategory(data) {
            var settings = {
                method: "POST",
                url: baseUrl.concat("/categories"),
                data: data
            };

            return $http(settings);
        }


        // --| Categories |------------------------------------------------------------------------>

        function _updateCategories (data) {
            // Data should come as an array of category objects

            var settings = {
                method: "PUT",
                url: baseUrl.concat("/categories"),
                data: data
            };

            return $http(settings);
        }


        // .........................................................................................

        function _deleteCategory(categoryId) {
            var settings = {
                method: "DELETE",
                url: baseUrl.concat("/category"),
                data: categoryId
            };

            return $http(settings);
        }

        // --| Months |---------------------------------------------------------------------------->

        function _getCurrentMonth() {
            var settings = {
                method: "GET"
                , url: baseUrl.concat("/currentmonth")
            };

            return $http(settings);
        }


        // --| UserLog |--------------------------------------------------------------------------->

        function _initializeUserLog(data) {
            var settings = {
                method: "POST",
                url: baseUrl.concat("/initializeUserLog"),
                data: data
            };

            return $http(settings);
        }
    }

})();