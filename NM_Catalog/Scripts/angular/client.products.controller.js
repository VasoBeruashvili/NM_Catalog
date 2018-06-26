app.controller("clientProductsController", function ($scope, $http) {
    var _subCategoryId = null;
    $scope.searchParam = '';
    $scope.currentPage = 1;

    $scope.getProductsBySubCategory = function (subCategoryId) {  
        $scope.searchParam = '';
        var mvItems = document.getElementsByClassName("mv-item");
        for (var i = 0; i < mvItems.length; i++) {
            if (parseInt(mvItems[i].id) === subCategoryId) {
                mvItems[i].className = "mv-item active-sub-category";
            } else {
                mvItems[i].className = "mv-item";
            }
        }
        _subCategoryId = subCategoryId;
        getProducts(null, subCategoryId, $scope.searchParam, $scope.currentPage);
    }

    function getProducts(categoryId, subCategoryId, param, currentPage) {
        showSpinner();
        $http.get("/products/getProducts?categoryId=" + categoryId + "&subCategoryId=" + subCategoryId + '&param=' + param + '&page=' + currentPage).then(function (resp) {
            $scope.products = resp.data.products;
            $scope.totalItems = resp.data.totalItemsCount;
            hideSpinner();
            $("html, body").animate({ scrollTop: 0 }, "slow");
        });
    }

    getProducts(categoryId, null, $scope.searchParam, $scope.currentPage);

    $scope.addToCart = function (pid, quantityToAdd) {
        if (confirm('გინდათ კალათში დამატება?')) {
            showSpinner();
            $http.get('/products/addToCart?id=' + pid + '&quantity=' + quantityToAdd).then(function (resp) {
                if (resp.data) {
                    $scope.getCartItemsCount();
                }
            });
        }
    }

    $scope.searchProducts = function () {
        getProducts(categoryId, _subCategoryId, $scope.searchParam, $scope.currentPage);
    }

    $scope.pageChanged = function () {
        getProducts(categoryId, _subCategoryId, '', $scope.currentPage);
    }
});