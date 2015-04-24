define("viewmodels/SearchStatsViewModel", ["knockout", "services/SearchAuditService", "services/AlertService"],
    function (ko, searchAuditService, alertService) {

        return function SearchStatsViewModel() {

            var isLoading = ko.observable(false),
                filterStats = ko.observableArray(),
                operandStats = ko.observableArray(),
                userStats = ko.observableArray();

            function loadSearchData() {

                // retrieve data from service and load
                isLoading(true);

                searchAuditService.getSearchStatistics()
                    .then(
                        function (data) {
                            filterStats(data.FilterStats.slice(0, 5));
                            operandStats(data.OperandStats.slice(0, 5));
                            userStats(data.UserStats.slice(0, 5));
                        },
                        function () {
                            alertService.showAlert("search stats info failed to load");
                        })
                    .always(
                        function () {
                            // done loading
                            isLoading(false);
                        }
                    );
            }

            function init(bindControl) {
                loadSearchData();
                ko.applyBindings(this, bindControl);
            }

            function dispose() {
                //
            }

            return {
                init: init,
                dispose: dispose,
                isLoading: isLoading,
                filterStats: filterStats,
                operandStats: operandStats,
                userStats: userStats

            };
        };
    }
);