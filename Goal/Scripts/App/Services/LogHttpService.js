(function () {
    "use strict";

    angular.module(APPNAME)
    .factory("$logHttpService", LogHttpService);

    LogHttpService.$inject = ['$http'];

    function LogHttpService($http) {
        
        var baseUrl = "/api/log"

        return {
            insertTransaction: _insertTransaction
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
    }

})();