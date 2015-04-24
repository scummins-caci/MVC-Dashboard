
define("viewmodels/ReceiptModalViewModel", ["knockout", "services/DataflowDataService", "services/AlertService"],
    function (ko, dataflowDataService, alertService) {

        return function ReceiptModalViewModel() {

            var selectedItem = ko.observable(undefined),
                selectedRouteInfo = ko.observable(undefined),
                changesLoading = ko.observable(false),
                template = "changeinfo-template";

            // paging binary routes
            var pageSize = 5,
                bCurrentPage = ko.observable(1);

            var bEndIndex = ko.computed(function () {
                return pageSize * bCurrentPage();
            });

            var bStartIndex = ko.computed(function () {
                return bEndIndex() - pageSize + 1;
            });

            var binaryRoutes = ko.computed(function () {
                return selectedRouteInfo() !== undefined ?
                    selectedRouteInfo().BinaryRoutes.slice(bStartIndex(), bStartIndex() + pageSize) : [];
            });

            var bTotalItems = ko.computed(function () {
                return selectedRouteInfo() !== undefined ? selectedRouteInfo().BinaryRoutes.length : 0;
            });

            var bTotalPages = ko.computed(function () {
                // if null return 0
                if (selectedRouteInfo() === undefined) {
                    return 0;
                }

                var pages = Math.floor(selectedRouteInfo().BinaryRoutes.length / pageSize);
                pages += selectedRouteInfo().BinaryRoutes.length % pageSize > 0 ? 1 : 0;
                return pages - 1;
            });

            var bHasPrevious = ko.computed(function () {
                return bCurrentPage() !== 1;
            });

            var bHasNext = ko.computed(function () {
                return bCurrentPage() !== bTotalPages();
            });

            var bNext = function () {
                if (bCurrentPage() < bTotalPages()) {
                    bCurrentPage(bCurrentPage() + 1);
                }
            };

            var bPrevious = function () {
                if (bCurrentPage() != 0) {
                    bCurrentPage(bCurrentPage() - 1);
                }
            };

            function close() {
                this.modal.close();
            };

            function loadChangeInfo(changeId) {
                changesLoading(true);

                dataflowDataService.getChangeInfo(changeId)
                    .then(
                        function (data) {
                            selectedRouteInfo(data);
                            // reset paging to first page
                            bCurrentPage(1);
                        },
                        function (error) {
                            alertService.showAlert("change info failed to load");
                        }
                    ).always(
                        function () {
                            // done loading
                            changesLoading(false);
                        }
                    );
            }

            return {
                selectedItem: selectedItem,
                selectedRouteInfo: selectedRouteInfo,
                loadChangeInfo: loadChangeInfo,
                changesLoading: changesLoading,
                template: template,
                Close: close,
                //binary route paging
                binaryRoutes: binaryRoutes,
                bTotalItems: bTotalItems,
                bStartIndex: bStartIndex,
                bEndIndex: bEndIndex,
                bPrevious: bPrevious,
                bHasPrevious: bHasPrevious,
                bCurrentPage: bCurrentPage,
                bNext: bNext,
                bHasNext: bHasNext
            };
        };
    }
);

define("viewmodels/DataflowReceiptsViewModel", ["knockout", "modal", "config", "services/DataflowDataService", "services/AlertService", "viewmodels/ReceiptModalViewModel"],
    function (ko, modal, config, dataflowDataService, alertService, ReceiptModalViewModel) {

        return function DataflowReceiptsViewModel() {

            // properties
            var updateTimer = undefined,
                receipts = ko.observableArray(),
                receiptsLoading = ko.observable(false),
                receiptViewModel = new ReceiptModalViewModel(),
                
                pageSize = 13,
                currentPage = ko.observable(1),
                receiptCount = ko.observable(0);

            // computed properties
            var totalPages = ko.computed(function () {
                var pages = Math.floor(receiptCount() / pageSize);
                pages += receiptCount() % pageSize > 0 ? 1 : 0;

                return pages - 1;
            });

            var endIndex = ko.computed(function () {
                return pageSize * currentPage();
            });

            var startIndex = ko.computed(function () {
                return endIndex() - pageSize + 1;
            });

            var hasPrevious = ko.computed(function () {
                return currentPage() !== 1;
            });

            var hasNext = ko.computed(function () {
                return currentPage() !== totalPages();
            });

            var next = function () {
                if (currentPage() < totalPages()) {
                    currentPage(currentPage() + 1);
                }
                loadReceipts();
            };

            var previous = function () {
                if (currentPage() != 0) {
                    currentPage(currentPage() - 1);
                }
                loadReceipts();
            };


            // functions
            function loadReceipts() {
                receiptsLoading(true);

                dataflowDataService.getReceipts(currentPage(), pageSize)
                    .then(
                        function (data) {
                            receipts(data.Items);
                            receiptCount(data.ItemCount);
                        },
                        function (error) {
                            alertService.showAlert("receipts failed to load");
                        }
                    ).always(
                        function () {
                            // done loading
                            receiptsLoading(false);
                        }
                    );
            }



            function init(bindControl) {
                // initialize data
                loadReceipts();

                // set interval to update
                updateTimer = setInterval(loadReceipts, config.loadInterval + 60000); // extend 60 seconds
                ko.applyBindings(this, bindControl);
            }

            function dispose() {
                clearInterval(updateTimer);
            }

            function refresh() {
                loadReceipts();
            }

            function showChangeInfo() {
                var item = this;

                modal.showModal({
                    viewModel: receiptViewModel,
                    context: item
                });

                receiptViewModel.selectedItem(item);
                receiptViewModel.loadChangeInfo(item.ChangeId);
            }

            // public pointers
            return {
                updateTimer: updateTimer,
                receipts: receipts,
                receiptCount: receiptCount,
                receiptsLoading: receiptsLoading,

                loadReceipts: loadReceipts,
                showChangeInfo: showChangeInfo,

                // paging
                hasPrevious: hasPrevious,
                hasNext: hasNext,
                next: next,
                previous: previous,
                currentPage: currentPage,
                endIndex: endIndex,
                startIndex: startIndex,

                init: init,
                refresh: refresh,
                dispose: dispose
            };
        };
    }
);