'use strict';
app.controller("stockCtrl", ['$scope', 'apiClientService',
    function ($scope, apiClientService) {
        
        $scope.errorMessage = '';
        $scope.pages = {
            start: 0,
            stockDetails: 1,
            calculations: 2
        };

        var stockDataEmpty = {
            Name: '',
            Price: 0,
            Quantity: 0,
            Percentage: 0,
            Years: 0,
            Calculations: []
        }

        $scope.start = function () {
            switchPage($scope.pages.start, 'Stock productivity calculator');
        }

        $scope.enterStockDetails = function () {
            switchPage($scope.pages.stockDetails, 'Enter stock details');
        }

        $scope.veiwClculatedResults = function () {
            switchPage($scope.pages.calculations, 'Stock productivity Result');

            //Load saved stocks:
            apiClientService.get('stocks')
                .success(function (data) {
                    $scope.stockDatas = data;
                })
                .error(function (data) {
                    $scope.errorMessage = data.Message;
                });

        }    

        $scope.calculate = function () {

            //Hide error message if any
            $scope.errorMessage = '';

            //validate Stock Data
            if (!validate()) {
                $scope.stockData.Calculations = [];
                return;
            }

            //Post to server
            apiClientService.post('stocks', $scope.stockData)
                .success(function (data) {
                    $scope.pageTitle = 'Calculation result';
                    $scope.stockData = data;
                    if ($scope.stockData.Calculations.length) {
                        $scope.calculationResultIsVisible = true;
                    }
                })
                .error(function (data) {
                    console.log(data);
                    $scope.calculationResultIsVisible = false;
                    $scope.errorMessage = data.Message;
                });
        }

        $scope.getCalculations = function (stock) {
            //Hide error message if any
            $scope.errorMessage = '';

            apiClientService.get('stocks/' + stock.Id + '/calculations')
                .success(function (data) {
                    stock.Calculations = data;
                    $scope.stockData = stock;
                    $scope.calculationResultIsVisible = true;
                })
                .error(function (data) {
                    $scope.calculationResultIsVisible = false;
                    console.log(data);
                    $scope.errorMessage = data.Message;
                });
        }

        var switchPage = function(page, title) {
            $scope.pageTitle = title;
            $scope.calculationResultIsVisible = false;
            $scope.page = page;
            $scope.stockData = stockDataEmpty;
        }

        var validate = function() {
            var errors = [];

            if (!$scope.stockData.Name) {
                errors.push("Stock name is required");
            }

            if (!$scope.stockData.Price) {
                errors.push("Price is required");
            }

            if (!$scope.stockData.Quantity) {
                errors.push("Quantity is required");
            }

            if (!$scope.stockData.Percentage) {
                errors.push("Percentage is required");
            }

            if (!$scope.stockData.Years) {
                errors.push("Years is required");
            }

            if (!errors.length) {
                return true;
            }

            $scope.errorMessage = errors.join(', ');
            return false;
        }

    }]);