define("viewmodels/SearchAuditCountsViewModel", ["knockout", "services/SearchAuditService", "services/AlertService"],
    function (ko, searchAuditService, alertService) {

        return function SearchAuditCountsViewModel() {

            var isLoading = ko.observable(false),
                searchCounts = ko.observableArray();

            function loadAuditData() {

                // retrieve data from service and load
                isLoading(true);

                searchAuditService.getSearchCounts()
                    .then(
                        function (data) {
                            searchCounts(data);
                        },
                        function (error) {
                            alertService.showAlert("search audit info failed to load");
                        })
                    .always(
                        function () {
                            // done loading
                            isLoading(false);
                        }
                    );
            }

            function init(bindControl) {
                loadAuditData();
                ko.applyBindings(this, bindControl);
            }

            function refresh() {
                loadAuditData();
            }

            function dispose() {
                //
            }

            return {
                init: init,
                dispose: dispose,
                refresh: refresh,
                isLoading: isLoading,
                searchCounts: searchCounts

            };
        };
    }
);