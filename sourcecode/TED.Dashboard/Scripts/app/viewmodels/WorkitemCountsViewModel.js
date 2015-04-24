
define("viewmodels/WorkitemCountsViewModel", ["knockout", "config", "services/WorkflowDataService", "services/AlertService"],
    function (ko, config, workflowDataService, alertService) {

        return function WorkitemCountsViewModel() {

            var updateTimer,
                isLoading = ko.observable(false),
                workitemInfo = ko.observableArray();

            // paging properties
            var pageSize = 8,
                currentPage = ko.observable(1);

            var totalItems = ko.computed(function () {
                return workitemInfo().length;
            });

            var endIndex = ko.computed(function () {
                return totalItems() < pageSize ? totalItems() : pageSize * currentPage();
            });  

            var startIndex = ko.computed(function () {
                return pageSize * currentPage() - pageSize + 1;
            });

            var sliceIndex = ko.computed(function () {
                return startIndex() - 1;
            }); 

            var totalPages = ko.computed(function () {
                var pages = Math.floor(workitemInfo().length / pageSize);
                pages += workitemInfo().length % pageSize > 0 ? 1 : 0;
                
                return pages;
            });

            var workitemCounts = ko.computed(function () {
                return workitemInfo().slice(sliceIndex(), sliceIndex() + pageSize);
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
            };

            var previous = function () {
                if (currentPage() != 0) {
                    currentPage(currentPage() - 1);
                }
            };

            function loadWorkitemData() {

                // retrieve data from service and load
                isLoading(true);

                workflowDataService.getInbasketCounts()
                    .then(
                        function (data) {
                            workitemInfo(data);
                        },
                        function (error) {
                            alertService.showAlert("workitem counts failed to load");
                        })
                    .always(
                        function () {
                            // done loading
                            isLoading(false);
                        }
                    );
            }

            function init(bindControl) {
                loadWorkitemData();

                // set interval for updates
                updateTimer = setInterval(loadWorkitemData, config.loadInterval);
                ko.applyBindings(this, bindControl);
            }

            function dispose() {
                clearInterval(updateTimer);
            }

            function refresh() {
                loadWorkitemData();
            }

            return {
                init: init,
                refresh: refresh,
                dispose: dispose,
                
                updateTimer: updateTimer,
                isLoading: isLoading,
                workitemCounts: workitemCounts,
                
                // paging
                hasPrevious: hasPrevious,
                hasNext: hasNext,
                next: next,
                previous: previous,
                currentPage: currentPage,
                endIndex: endIndex,
                startIndex: startIndex,
                totalItems: totalItems
            };
        };
    }
);