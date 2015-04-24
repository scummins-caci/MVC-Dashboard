// this needs to go in a better spot
Array.prototype.findById = function (id) {
    for (var i = 0; i < this.length; i++) {
        if (this[i].ID === id) {
            return this[i];
        }
    }

    return undefined;
}

function getURLParameter(name) {
    return decodeURIComponent((new RegExp("[?|&]" + name + "=" + "([^&;]+?)(&|#|;|$)").exec(location.search) || [, ""])[1].replace(/\+/g, "%20")) || null;
}

define("bulkEditViewModel", ["knockout"],
    function (ko) {
        return function BulkEditViewModel() {
            var template = "bulkedit-template";

            function close() {
                this.modal.close();
            };

            return {
                template: template,
                Close: close
            };
        }
    }
);

define(["knockout", "modal", "app/inbasketcolumns", "services/WorkflowDataService", "services/AlertService", "bulkEditViewModel", "components", "bindings"],
    function (ko, modal, columnConfig, workflowDataService, alertService, BulkEditViewModel) {

        return function ComponentInbasketViewModel() {
         
            /* panel display related stuff */
            var ShowLeftSideBar = ko.observable(true),
                RightPinned = ko.observable(false),
                Resizing = ko.observable(false),
                RightVisible = ko.observable(false);

            var ToggleLeftSideBar = function () {
                ShowLeftSideBar(!ShowLeftSideBar());
            };

            var ToggleRightPin = function () {
                RightPinned(!RightPinned());
                RightVisible(RightPinned);
            };

            // state related items
            var currentView = ko.observable("process"),
                selectedProcess = ko.observable({}),
                selectedQueue = ko.observable({}),
                selectedUser = ko.observable({}),
                processesLoading = ko.observable(false),
                queuesLoading = ko.observable(false),
                workitemsLoading = ko.observable(false),
                itemsPerPage = ko.observable(50),
                currentPage = ko.observable(1);

            var isLoading = ko.computed(function () {
                if (processesLoading() || queuesLoading() || workitemsLoading()) {
                    return true;
                }
                return false;
            });

            // inbasket data
            var Processes = ko.observableArray([]),
                WorkItems = ko.observableArray([]),
                QueueColumns = ko.observableArray([]), // columns to display for workitems
                Filters = ko.observableArray([]), // filters for workitem display
                ColumnSorts = ko.observableArray([]),  // columns to sort by
                WorkItemCount = ko.observable(0),
                ItemActions = ko.observableArray([]);

            var getQueueColumns = function (queueName) {
                var mappingName = columnConfig.QueueColumnMapping[queueName] !== undefined ?
                    columnConfig.QueueColumnMapping[queueName] : "Default";

                var mapping = columnConfig.QueueColumns[mappingName];

                // add display observable
                mapping.forEach(function (item) {
                    if (item.columnVisible === undefined) {
                        item.columnVisible = ko.observable(true);
                    }
                    if (item.columnSort === undefined) {
                        item.columnSort = ko.observable("none");
                    }
                });

                return mapping;
            }

            //* functions for retrieving processes and workitems  *//
            var loadWorkitems = function () {
                if (selectedQueue().ID == undefined) {
                    WorkItems([]);
                    WorkItemCount(0);
                }

                currentView("workitems");
                QueueColumns(getQueueColumns(selectedQueue().Name));

                // screen is going to be resized, so notify
                Resizing(true);
                workitemsLoading(true);
                workflowDataService.getWorkitems(selectedProcess().ID, selectedQueue().ID, currentPage(), itemsPerPage(), Filters(), ColumnSorts())
                    .then(
                        function (data) {
                            WorkItems(data.Items);
                            WorkItemCount(data.ItemCount);
                        },
                        function () {
                            alertService.showAlert("workitems failed to load");
                        })
                    .always(
                        function () {
                            // done loading
                            workitemsLoading(false);
                            Resizing(false);
                        }
                    );
            };

            var queueSelected = function (process, queue) {
                if (process != undefined) {
                    selectedProcess(process);
                }
                selectedQueue(queue);
                // clear filters and sorts
                //Filters
                loadWorkitems();
            };

            var loadQueues = function (process, queueId) {

                // only switch if we are going to display the process
                if (!process.display()) {
                    selectedProcess(process);
                    selectedQueue({});
                    currentView("queue");
                }

                // check to see if queues have been loaded yet
                if (process.Queues().length === 0) {
                    process.loading(true);
                    queuesLoading(true);

                    workflowDataService.getQueues(process.ID, 1, 50)
                        .then(
                            function (data) {
                                process.Queues(data.Items);

                                // there may have been a queue id we want to navigate to;
                                // if so, attempt to find and load
                                if (queueId == undefined) return;

                                var queue = data.Items.findById(queueId);
                                if (queue !== undefined) {
                                    queueSelected(process, queue);
                                }
                            },
                            function () {
                                alertService.showAlert("queues failed to load");
                            })
                        .always(
                            function () {
                                // done loading
                                queuesLoading(false);
                                process.loading(false);
                                process.display(!process.display());
                            }
                        );
                } else {
                    process.display(!process.display());
                }
            }

            var showQueues = function () {
                var process = this;
                loadQueues(process);
            }

            var loadProcesses = function (processId, queueId) {
                processesLoading(true);
                currentView("process");

                workflowDataService.getProcesses(1, 50)
                    .then(
                        function (data) {

                            // add queue placeholder
                            data.Items.forEach(function (item) {
                                item.Queues = ko.observableArray([]);
                                item.display = ko.observable(false);
                                item.loading = ko.observable(false);
                            });

                            Processes(data.Items);

                            // there may have been a process id we want to navigate to;
                            // if so, attempt to find and load
                            if (processId == undefined) return;

                            var process = data.Items.findById(processId);
                            if (process !== undefined) {
                                loadQueues(process, queueId);
                            }

                        },
                        function () {
                            alertService.showAlert("workitem counts failed to load");
                        })
                    .always(
                        function () {
                            // done loading
                            processesLoading(false);
                        }
                    );
            }

            //* functions for sorting and filtering *//
            var addFilter = function (columnName, filterValue, operator, dataType) {

                // TODO:  handle different data types
                var filter = {};
                filter.column = columnName;
                filter.value = filterValue;
                filter.operator = operator;
                filter.dataType = dataType !== undefined ? dataType : "string";

                Filters.push(filter);
            };

            var clearFilters = function () {
                Filters([]);
            };

            var sortColumn = function (column) {
                // toggle the sort setting
                if (column.columnSort() === "none") {
                    column.columnSort("asc");
                }
                else if (column.columnSort() === "asc") {
                    column.columnSort("desc");
                }
                else if (column.columnSort() === "desc") {
                    column.columnSort("none");
                }

                // refresh the sort columns collection
                var sorts = ColumnSorts();

                // find index of existing
                var columnIndex = -1;
                for (var i = 0, j = sorts.length; i < j; i++) {
                    if (sorts[i].column === column.columnName)
                        columnIndex = i;
                }

                var sort = {};
                sort.column = column.columnName;
                sort.order = column.columnSort();

                if (columnIndex === -1 && sort.order !== "none") {
                    sorts.push(sort);
                }
                else if (columnIndex > -1 && sort.order === "none") {
                    sorts.splice(columnIndex, 1);
                }
                else if (columnIndex > -1 && sort.order !== "none") {
                    sorts.splice(columnIndex, 1);
                    sorts.splice(columnIndex, 0, sort);
                }

                ColumnSorts(sorts);
            };

            //* changes to any of these following items should trigger a new workitem load *//
            Filters.subscribe(function (val) {
                // filter updated;  load workitems again
                loadWorkitems();
            });

            ColumnSorts.subscribe(function (val) {
                // sorts updated; load workitems again
                loadWorkitems();
            });

            currentPage.subscribe(function() {
                // page changed
                loadWorkitems();
            });

            itemsPerPage.subscribe(function() {
                // if the currentPage is 1, it wasn't changed;  load workitems
                if (currentPage() === 1 && currentView() === "workitems") {
                    loadWorkitems();
                }
            });

            //* functions for doing actions *//
            var doWork = function () {
                var itemList = "";

                WorkItems().forEach(function (item) {
                    if (item.selected()) {
                        itemList += item.ID + ", ";
                    }
                });

                alertService.showInfo("Do work on these items: " + itemList);
            };

            var bulkEdit = function() {

                modal.showModal({
                    viewModel: new BulkEditViewModel(),
                    context: this
                });
            };

            //* initialze data and interface *//

            function init() {
                // check to see if the url has a specific process to go to
                var processId = getURLParameter("process");
                processId = parseInt(processId) !== NaN ? parseInt(processId) : undefined;

                // check to see if the url has a specific queue to go to
                var queueId = getURLParameter("queue");
                queueId = parseInt(queueId) !== NaN ? parseInt(queueId) : undefined;

                // load up the initial processes
                loadProcesses(processId, queueId);
            }
            
            init();

            return {
                isLoading: isLoading,
                selectedProcess: selectedProcess,
                selectedQueue: selectedQueue,
                selectedUser: selectedUser,

                //panel display items
                ShowLeftSideBar: ShowLeftSideBar,
                RightPinned: RightPinned,
                Resizing: Resizing,
                RightVisible: RightVisible,
                ToggleLeftSideBar: ToggleLeftSideBar,
                ToggleRightPin: ToggleRightPin,

                // table related items
                currentView: currentView,

                // sort and filtering
                addFilter: addFilter,
                sortColumn: sortColumn,
                clearFilters: clearFilters,
                Filters: Filters,
                ColumnSorts: ColumnSorts,

                // inbasket data
                Processes: Processes,
                WorkItems: WorkItems,
                QueueColumns: QueueColumns,
                WorkItemCount: WorkItemCount,
                loadWorkitems: loadWorkitems,
                loadProcesses: loadProcesses,
                showQueues: showQueues,
                queueSelected: queueSelected,
                itemsPerPage: itemsPerPage,
                currentPage: currentPage,
                ItemActions: ItemActions,

                // actions
                doWork: doWork,
                bulkEdit: bulkEdit
            };
        }
    });
