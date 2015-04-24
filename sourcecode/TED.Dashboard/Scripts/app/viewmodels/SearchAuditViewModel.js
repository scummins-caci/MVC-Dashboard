// view model for modal pop-up
define("viewmodels/AuditModalViewModel", ["knockout"],
    function (ko) {
        return function AuditModalViewModel() {

            var searchAuditEntry = ko.observable(),
                template = "audit-template";

            function close() {
                this.modal.close();
            };

            // public pointers
            return {
                template: template,
                Close: close,
                SearchAuditEntry: searchAuditEntry
            };
        };
    }
);

define("viewmodels/SearchAuditViewModel", ["knockout", "modal", "config", "services/SearchAuditService", "services/AlertService", "viewmodels/AuditModalViewModel"],
    function (ko, modal, config, searchAuditService, alertService, AuditModalViewModel) {

        return function SearchAuditViewModel() {

            // properties
            var updateTimer = undefined,
                searchAudits = ko.observableArray(),
                isLoading = ko.observable(false),
                singleLogModel = new AuditModalViewModel(),

                pageSize = 10,
                currentPage = ko.observable(1),
                searchCount = ko.observable(0);

            // computed properties
            var totalPages = ko.computed(function () {
                var pages = Math.floor(searchCount() / pageSize);
                pages += searchCount() % pageSize > 0 ? 1 : 0;

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
                loadSearches();
            };

            var previous = function () {
                if (currentPage() != 0) {
                    currentPage(currentPage() - 1);
                }
                loadSearches();
            };

            // functions
            function loadSearches() {
                isLoading(true);

                searchAuditService.getSearches(currentPage(), pageSize)
                    .then(
                        function (data) {
                            searchAudits(data.Items);
                            searchCount(data.ItemCount);
                        },
                        function (error) {
                            alertService.showAlert("Search audits failed to load");
                            console.error(error);
                        }
                    ).always(
                        function () {
                            // done loading
                            isLoading(false);
                        }
                );

            }

            function showLogEntry() {
                singleLogModel.SearchAuditEntry(this);

                modal.showModal({
                    viewModel: singleLogModel,
                    context: this
                });
            }

            function init(bindControl) {
                // initialize data
                loadSearches();

                // set interval to update
                updateTimer = setInterval(loadSearches, config.loadInterval + 60000); // extend 60 seconds
                ko.applyBindings(this, bindControl);
            }

            function dispose() {
                clearInterval(updateTimer);
            }

            function refresh() {
                loadSearches();
            }

            // public pointers
            return {
                updateTimer: updateTimer,
                searchAudits: searchAudits,
                searchCount: searchCount,
                isLoading: isLoading,
                singleLogModel: singleLogModel,

                loadSearches: loadSearches,
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