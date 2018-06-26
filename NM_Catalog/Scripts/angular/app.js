function showSpinner() {
    document.body.className = "body-opacity";
    document.getElementById("spinner").style.display = "block";
}
function hideSpinner() {
    document.body.className = "";
    document.getElementById("spinner").style.display = "none";
}

var app = angular.module("app", ['ui.bootstrap']);

function loadImageToBase64DataURL() {
    var filesSelected = document.getElementById("inputFileToLoad").files;
    if (filesSelected.length > 0) {
        var fileToLoad = filesSelected[0];
        var fileReader = new FileReader();
        fileReader.onload = function (fileLoadedEvent) {
            app.imageData = fileLoadedEvent.target.result;
            $('#productImage').attr('src', app.imageData);
        };
        fileReader.readAsDataURL(fileToLoad);
    }
}

app.controller('globalController', function ($scope, $http) {
    $scope.getCartItemsCount = function () {
        showSpinner();
        $http.get('/products/getCartItemsCount').then(function (resp) {
            $scope.cartItemsCount = resp.data;
            hideSpinner();
        });
    }
    $scope.getCartItemsCount();

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