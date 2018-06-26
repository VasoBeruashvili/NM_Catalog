app.controller("adminProductsController", function ($scope, $http) {
    $scope.currentPage = 1;

    $scope.openImageUpload = function () {
        $('#inputFileToLoad').click();
    }

    $scope.editProduct = function (product) {
        $scope.id = product.id;
        $scope.name = product.name;
        $scope.description = product.description;
        $scope.price = product.price;
        app.imageData = product.productImage;
        $('#productImage').attr('src', app.imageData);
    }

    $scope.resetForm = function () {
        $scope.name = '';
        $scope.description = '';
        $scope.price = '';
        app.imageData = '';
        $('#productImage').attr('src', '');
    }

    $scope.saveProduct = function () {
        showSpinner();
        var product = {
            id: $scope.id,
            name: $scope.name,
            description: $scope.description,
            price: $scope.price,
            categoryId: $scope.catId,
            subCategoryId: $scope.subCatId,
            productImage: app.imageData
        };

        $http.post('/products/saveProduct', { product: product }).then(function (resp) {
            if (resp.data) {
                $scope.getProducts($scope.subCatId);
            }
            else {
                alert('შენახვისას დაფიქსირდა შეცდომა!');
            }
        });
    }

    $scope.deleteProduct = function (productId) {
        if (confirm('გინდათ წაშლა?')) {
            showSpinner();
            $http.get('/products/deleteProduct/' + productId).then(function (resp) {
                if (resp.data) {
                    $scope.getProducts($scope.subCatId);
                }
            });
        }
    }





    $scope.getCategories = function () {
        showSpinner();
        $http.get('/products/getCategories').then(function (resp) {
            $scope.categories = resp.data;
            hideSpinner();
        });
    }
    $scope.getCategories();

    $scope.getSubCategories = function (categoryId) {
        showSpinner();
        $scope.catId = categoryId;
        angular.forEach($scope.categories, function (c) {
            c.activeClass = '';
        });
        $scope.categories.first(function (c) { return c.id === categoryId; }).activeClass = 'active';
        $http.get('/products/getSubCategories?categoryId=' + categoryId).then(function (resp) {
            $scope.subCategories = resp.data;
            $scope.resetForm();
            hideSpinner();
        });
    }

    $scope.getProducts = function (subCategoryId) {
        $scope.subCatId = subCategoryId;
        angular.forEach($scope.subCategories, function (sc) {
            sc.activeClass = '';
        });
        $scope.subCategories.first(function (sc) { return sc.id === subCategoryId; }).activeClass = 'active';

        showSpinner();
        $http.get("/products/getProducts?categoryId=" + $scope.catId + "&subCategoryId=" + subCategoryId + '&param=' + '' + '&page=' + null).then(function (resp) {
            $scope.products = resp.data.products;
            $scope.totalItems = resp.data.totalItemsCount;
            $scope.resetForm();
            hideSpinner();
            $("html, body").animate({ scrollTop: 0 }, "slow");
        });
    }


    
    $scope.saveSubCat = function () {
        showSpinner();
        $http.get('/products/saveSubCat?name=' + $scope.subCatName + '&catId=' + $scope.catId).then(function (resp) {
            if (resp.data) {
                $scope.subCatName = '';
                $scope.getSubCategories($scope.catId);
                hideSpinner();
            }
        });
    }

    $scope.deleteSubCat = function (index) {
        if (confirm('გინდათ წაშლა?')) {
            showSpinner();
            $http.get('/products/deleteSubCat?subCatId=' + $scope.subCategories[index].id).then(function (resp) {
                if (resp.data) {
                    $scope.getSubCategories($scope.catId);
                    hideSpinner();
                }
            });
        }
    }

    $scope.pageChanged = function () {
        getProducts($scope.subCatId);
    }


    $scope.openFancyBox = function (imageData, productName) {
        $.fancybox.open([
            {
                href: imageData,
                title: productName
            }
        ], {
                padding: 0
            });
        return false;
    }
});