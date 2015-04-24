
define("viewmodels/WorkitemStatusViewModel", ["knockout", "config", "services/WorkflowDataService", "services/AlertService"],
    function (ko, config, workflowDataService, alertService) {

        return function WorkitemStatusViewModel() {

            var updateTimer,
                isLoading = ko.observable(false),
                statuses = ko.observableArray([]);

            var chartdata = ko.computed(function () {
                var data = [];

                if (statuses().length > 0) {
                    statuses().forEach(function (element) {
                        data.push({ label: element.Status + " work items", value: element.Count });
                    });
                } else {
                    // data hasn't loaded yet;  put a dummy row in for the charting plugin
                    data.push({ label: "loading...", value: 0 });
                }
                return data;
            });

            function loadStatusData() {
                // retrieve data from service and load
                isLoading(true);

                workflowDataService.getStatusCounts()
                    .then(
                        function (data) {
                            isLoading(false);   // because morris charts needs an area to create the chart in,
                                                // the div has to be visible prior to setting the data
                            statuses(data);
                        },
                        function (error) {
                            alertService.showAlert("workitem statuses failed to load");
                        })
                    .always(
                        function () {
                            // done loading
                            isLoading(false);
                        }
                    );
            }

            function init(bindControl) {
                loadStatusData();

                // set interval for updates
                updateTimer = setInterval(loadStatusData, config.loadInterval);
                // initialize bindings
                ko.applyBindings(this, bindControl);
            }

            function dispose() {
                clearInterval(updateTimer);
            }
            
            function refresh() {
                loadStatusData();
            }

            return {
                init: init,
                dispose: dispose,
                refresh: refresh,
                isLoading: isLoading,
                statuses: statuses,
                chartdata: chartdata
            };
        };

    }
);