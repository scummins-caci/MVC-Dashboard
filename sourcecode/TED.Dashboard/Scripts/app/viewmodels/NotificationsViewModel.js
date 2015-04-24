
// view model for modal pop-up
define("viewmodels/LogEntryModalViewModel", ["knockout"],
    function (ko) {
        return function LogEntryModalViewModel() {

            var logEntry = ko.observable(),
                template = "log-template";
            
            function close() {
                this.modal.close();
            };

            // public pointers
            return {
                template: template,
                Close: close,
                LogEntry: logEntry
            };
        };
    }
);

define("viewmodels/NotificationsViewModel", ["knockout", "modal", "config", "services/WorkflowDataService", "services/AlertService", "viewmodels/LogEntryModalViewModel"],
    function (ko, modal, config, workflowDataService, alertService, LogEntryModalViewModel) {

        return function NotificationsViewModel() {

            // properties
            var updateTimer = undefined,
                workflowLogs = ko.observableArray(),
                isLoading = ko.observable(false),
                singleLogModel = new LogEntryModalViewModel(),

                pageSize = 10,
                currentPage = ko.observable(1),
                logCount = ko.observable(0);
            
            // computed properties
            var totalPages = ko.computed(function () {
                var pages = Math.floor(logCount() / pageSize);
                pages += logCount() % pageSize > 0 ? 1 : 0;

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
                loadNotifications();
            };

            var previous = function () {
                if (currentPage() != 0) {
                    currentPage(currentPage() - 1);
                }
                loadNotifications();
            };

            // functions
            function loadNotifications() {
                isLoading(true);

                workflowDataService.getNotifications(currentPage(), pageSize)
                    .then(
                        function (data) {
                            workflowLogs(data.Items);
                            logCount(data.ItemCount);
                        },
                        function (error) {
                            alertService.showAlert("Notifications failed to load");
                        }
                    ).always(
                        function () {
                            // done loading
                            isLoading(false);
                        }
                );

            }

            function showLogEntry() {
                singleLogModel.LogEntry(this);

                modal.showModal({
                    viewModel: singleLogModel,
                    context: this
                });
            }

            function init(bindControl) {
                // initialize data
                loadNotifications();

                // set interval to update
                updateTimer = setInterval(loadNotifications, config.loadInterval + 60000); // extend 60 seconds
                ko.applyBindings(this, bindControl);
            }

            function refresh() {
                loadNotifications();
            }

            function dispose() {
                clearInterval(updateTimer);
            }


            // public pointers
            return {
                updateTimer: updateTimer,
                workflowLogs: workflowLogs,
                logCount: logCount,
                isLoading: isLoading,
                singleLogModel: singleLogModel,

                loadNotifications: loadNotifications,
                showLogEntry: showLogEntry,

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

    