(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('wishlistPageController', WishlistPageController);

    WishlistPageController.$inject = ['$scope'];

    function WishlistPageController($scope) {

        // Injection
        var vm = this;
        vm.$scope = $scope;


    }
})();