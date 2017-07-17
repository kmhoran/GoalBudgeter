(function () {
    "use strict";

    angular.module(APPNAME)
    .factory("$reportService", LogHttpService);

    LogHttpService.$inject = ['$http'];

    function LogHttpService($http) {

        var baseUrl = "/api/reports"

        // Public Methods
        return {
            getMonthlySpendingByCategory: _getMonthlySpendingByCategory
        };

        // /////////////////////////////////////////////////////////////////////////////////////////

        function _getMonthlySpendingByCategory() {
            var settings = {
                method: "GET"
                , url: baseUrl.concat("/categorymonthlyspending")
            };

            return $http(settings);
        }
    }

})();