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

define("viewmodels/BasicInbasketViewModel", ["knockout", "config", "services/WorkflowDataService", "services/AlertService"],
    function(ko, config, workflowDataService, alertService) {

        // queue column definitions
        var QueueColumnsOptions = {
            "Default": [
                { "columnName" : "ShortName", "displayName" : "Name" },
                { "columnName": "LoginName", "displayName": "Assigned User" },
                { "columnName": "StatusChanged", "displayName": "Assign Date" },
                { "columnName": "DateSubmitted", "displayName": "Date Submitted" }
            ],
            "QAQueues": [
                { "columnName": "ShortName", "displayName": "Name" },
                { "columnName": "LoadId", "displayName": "Load Id" },
                
                { "columnName": "LoginName", "displayName": "Assigned User" },
                { "columnName": "StatusChanged", "displayName": "Assign Date" },
                { "columnName": "DateSubmitted", "displayName": "Date Submitted" }
            ],
            "NMECQueues": [
                { "columnName": "ShortName", "displayName": "Name" },
                { "columnName": "BatchName", "displayName": "Batch Name" },
                { "columnName": "Priority", "displayName": "Priority" },
                { "columnName": "MediaLength", "displayName": "Media Length" },
                { "columnName": "TotalPages", "displayName": "Page Count" },
                { "columnName": "LoginName", "displayName": "Assigned User" },
                { "columnName": "StatusChanged", "displayName": "Assign Date" },
                { "columnName": "DateSubmitted", "displayName": "Date Submitted" }
            ],
            "SectionErrorQueues": [
                { "columnName": "ShortName", "displayName": "Name" },
                { "columnName": "ParentHarmonyNumber", "displayName": "Parent Harmony Number" },
                { "columnName": "LoginName", "displayName": "Assigned User" },
                { "columnName": "StatusChanged", "displayName": "Assign Date" },
                { "columnName": "DateSubmitted", "displayName": "Date Submitted" }
            ]  
        };

        var QueueColumnMapping = {
            "Text Extraction Error": "SectionErrorQueues",
            "NEE Error": "SectionErrorQueues",
            "OCR Error": "SectionErrorQueues",
            "KFE Error": "SectionErrorQueues",
            "MT Error": "SectionErrorQueues",
            "Media Conversion Error": "SectionErrorQueues",
            "SCAP Error": "SectionErrorQueues",
            "Missing Page Count": "NMECQueues",
            "Screening Team": "NMECQueues",
            "QC Team": "NMECQueues",
            "QA": "NMECQueues",
            "PreScreening": "NMECQueues",
            "QA Batch Checkout Error": "QAQueues",
            "QA Batch Name Mismatch": "QAQueues",
            "QA Batch Upgraded Classification": "QAQueues",
            "QA Batch define LOV Mappings": "QAQueues",
            "QA Perform Batch Merge": "QAQueues",
            "QA Reused Batch Name": "QAQueues",
            "QA Auto Update Failed": "QAQueues",
            "QA Checkout Error": "QAQueues",
            "QA Define LOV Mappings": "QAQueues",
            "QA Delete Requested": "QAQueues",
            "QA Downgrade Classification": "QAQueues",
            "QA Harmony Number Mismatch": "QAQueues",
            "QA Perform Merge": "QAQueues",
            "QA Upgraded Classification": "QAQueues",
            "QA Batch Missing Required Fields": "QAQueues",
            "QA Review Batch": "QAQueues",
            "Missing Required Fields": "QAQueues",
            "QA Duplicate Harmony Number": "QAQueues",
            "QA Invalid Classifications": "QAQueues",
            "QA Review": "QAQueues"
        };

        return function BasicInbasketViewModel() {

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
            var previousSelectedIndex = -1,
                currentView = ko.observable("process"),
                selectedProcess = ko.observable({}),
                selectedQueue = ko.observable({}),
                selectedUser = ko.observable({}),
                processesLoading = ko.observable(false),
                queuesLoading = ko.observable(false),
                workitemsLoading = ko.observable(false);

            var isLoading = ko.computed(function() {
                if (processesLoading() || queuesLoading() || workitemsLoading()) {
                    return true;
                }
                return false;
            }, this);

            // process/queue/workitem related variables
            var loadUsers = function (queue) {
                var deferred = $.Deferred();

                if (queue.ID == undefined) {
                    deferred.resolve([]);
                    return deferred.promise();
                }

                workflowDataService.getQueueUsers(queue.ID)
                    .then(
                        function (data) {
                            var items = [];
                            data.forEach(function (item) {
                                items.push({ id: item.ID, text: item.UserName });
                            });

                            deferred.resolve(items);
                        },
                        function () {
                            alertService.showAlert("queue users failed to load");
                            deferred.reject();
                        });
                return deferred.promise();
            }

            var Processes = ko.observableArray([]),
                WorkItems = ko.observableArray([]),
                QueueColumns = ko.observableArray([]),  // columns to display for workitems
                Filters = ko.observableArray([]),       // filters for workitem display
                ColumnSorts = ko.observableArray([]),   // columns to sort by
                WorkItemCount = ko.observable(0),
                QueueUsers = ko.computed(function () {
                    return loadUsers(selectedQueue());
                }).extend({ async: true });

            var getQueueColumns = function (queueName) {
                var mappingName = QueueColumnMapping[queueName] !== undefined ?
                    QueueColumnMapping[queueName] : "Default";

                var mapping = QueueColumnsOptions[mappingName];

                // add display observable
                mapping.forEach(function(item) {
                    if (item.columnVisible === undefined) {
                        item.columnVisible = ko.observable(true);
                    }
                    if (item.columnSort === undefined) {
                        item.columnSort = ko.observable("none");
                    }
                });

                return mapping;
            }

            var sortColumn = function(column) {
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

            var GetCellDisplayText = function(columnData, workitem) {
                return workitem[columnData.columnName];
            };

            var addFilter = function(columnName, filterValue, operator, dataType) {

                // TODO:  handle different data types
                var filter = {};
                filter.column = columnName;
                filter.value = filterValue;
                filter.operator = operator;
                filter.dataType = dataType !== undefined ? dataType : "string";

                Filters.push(filter);
            };

            var clearFilters = function() {
                Filters([]);
            };

            var loadWorkitems = function() {

                if (selectedQueue().ID == undefined) {
                    WorkItems([]);
                    WorkItemCount(0);
                }

                currentView("workitems");
                QueueColumns(getQueueColumns(selectedQueue().Name));

                // screen is going to be resized, so notify
                Resizing(true);
                workitemsLoading(true);
                workflowDataService.getWorkitems(selectedProcess().ID, selectedQueue().ID, 1, 50, Filters(), ColumnSorts())
                    .then(
                        function(data) {
                            data.Items.forEach(function(item) {
                                item.selected = false;
                            });
                            WorkItems(data.Items);
                            WorkItemCount(data.ItemCount);
                        },
                        function() {
                            alertService.showAlert("workitems failed to load");
                        })
                    .always(
                        function() {
                            // done loading
                            workitemsLoading(false);
                            Resizing(false);
                        }
                    );
            };

            Filters.subscribe(function() {
                // filter updated;  load workitems again
                loadWorkitems();
            });

            ColumnSorts.subscribe(function() {
                // sorts updated; load workitems again
                loadWorkitems();
            });

            var queueSelected = function (process, queue) {
                if (process != undefined) {
                    selectedProcess(process);
                }
                selectedQueue(queue);
                loadWorkitems();
            };

            /* workitem related stuff */
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

            /* table related stuff */
            var doWork = function () {
                var itemList = "";

                WorkItems().forEach(function (item) {
                    if (item.selected) {
                        itemList += item.ID + ", ";
                    }
                });

                alertService.showInfo("Do work on these items: " + itemList);
            };

            var resetSelected = function () {
                WorkItems().forEach(function (item) {
                    item.selected = false;
                });
            };

            var refresh = function () {
                var items = WorkItems();
                WorkItems([]);
                WorkItems(items);
            };

            var rowSelected = function (itemIndex, item, e) {
                var newState = !item.selected;
                var currentIndex = itemIndex();

                if (!e.ctrlKey && !e.shiftKey && !e.metaKey) {
                    resetSelected();
                }

                if (e.shiftKey && previousSelectedIndex !== -1) {
                    var startIndex = 0;
                    var endIndex = 0;
                    if (currentIndex > previousSelectedIndex) {
                        startIndex = previousSelectedIndex;
                        endIndex = currentIndex;
                    }
                    else {
                        startIndex = currentIndex;
                        endIndex = previousSelectedIndex;
                    }

                    // set all items to the new state
                    for (var i = startIndex; i < endIndex; i++) {
                        WorkItems()[i].selected = newState;
                    }
                }

                item.selected = newState;
                refresh();

                // retain selected index
                previousSelectedIndex = currentIndex;
            };

            function init(bindControl) {
                ko.applyBindings(this, bindControl);

                // check to see if the url has a specific process to go to
                var processId = getURLParameter("process");
                processId = parseInt(processId) !== NaN ? parseInt(processId) : undefined;

                // check to see if the url has a specific queue to go to
                var queueId = getURLParameter("queue");
                queueId = parseInt(queueId) !== NaN ? parseInt(queueId) : undefined;

                // load up the initial processes
                loadProcesses(processId, queueId);
            }

            return {
                init: init,
                isLoading: isLoading,
                WorkItems: WorkItems,
                WorkItemCount: WorkItemCount,
                Processes: Processes,
                QueueUsers: QueueUsers,
                QueueColumns: QueueColumns,
                sortColumn: sortColumn,
                GetCellDisplayText: GetCellDisplayText,
                selectedProcess: selectedProcess,
                selectedQueue: selectedQueue,
                selectedUser: selectedUser,
                showQueues: showQueues,
                queueSelected: queueSelected,

                //filtering
                addFilter: addFilter,
                clearFilters: clearFilters,
                Filters: Filters,

                //panel display items
                ShowLeftSideBar: ShowLeftSideBar,
                RightPinned: RightPinned,
                Resizing: Resizing,
                RightVisible: RightVisible,
                ToggleLeftSideBar: ToggleLeftSideBar,
                ToggleRightPin: ToggleRightPin,

                // table related items
                doWork: doWork,
                rowSelected: rowSelected,
                currentView: currentView
            };
        };
    }

);
