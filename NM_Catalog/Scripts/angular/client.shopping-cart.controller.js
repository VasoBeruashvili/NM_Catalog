app.controller("shoppingCartController", function ($scope, $http) {
    $scope.getCartItems = function () {
        showSpinner();
        $http.get('/products/getCartItems').then(function (resp) {
            $scope.cartItems = resp.data.products;
            $scope.totalSum = resp.data.totalSum;
            $(".comment").shorten({
                "showChars": 300
            });
            hideSpinner();
        });
    }
    $scope.getCartItems();

    $scope.removeFromCart = function (pid) {
        if (confirm('გინდათ კალათიდან ამოშლა?')) {
            showSpinner();
            $http.get('/products/removeFromChart/' + pid).then(function (resp) {
                if (resp.data) {
                    $scope.getCartItemsCount();
                    $scope.getCartItems();
                }
            });
        }
    }

    $scope.changeCartProductQuantity = function (index) {
        $scope.cartItems[index].sum = ($scope.cartItems[index].quantityToAdd * $scope.cartItems[index].price);
        $scope.totalSum = $scope.cartItems.sum(function (ci) { return ci.sum; });
    }

    $scope.sendOrderToEmail = function () {
        showSpinner();
        var orders = [];
        angular.forEach($scope.cartItems, function (ci) {
            orders.push({
                productId: ci.id,
                quantity: ci.quantityToAdd
            });
        });
        $http.post('/products/sendOrderToEmail', { orders: orders }).then(function (resp) {
            if (resp.data) {
                $scope.getCartItemsCount();
                $scope.getCartItems();
            }
        });
    }
});