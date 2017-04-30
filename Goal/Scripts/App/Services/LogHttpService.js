(function () {
    "use strict";

    angular.module(APPNAME)
    .factory("$logHttpService", LogHttpService);

    LogHttpService.$inject = ['$http'];

    function LogHttpService($http) {
        
        var baseUrl = "/api/log"

        return {
            insertTransaction: _insertTransaction,
            getTransactionCategories: _getTransactionCategories,
            insertTransactionCategory: _insertTransactionCategory
        };

        // /////////////////////////////////////////////////////////////////////////////////////////

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
    }

})();