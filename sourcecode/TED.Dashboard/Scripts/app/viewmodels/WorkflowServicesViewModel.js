
// view model for modal pop-up
define("viewmodels/ConnectorModalViewModel", ["knockout"],
    function (ko) {
        return function ConnectorModalViewModel() {

            var connectors = ko.observableArray(),
                template = "connectors-template";

            function close() {
                this.modal.close();
            };

            // public pointers
            return {
                template: template,
                Close: close,
                Connectors: connectors
            };
        };
    }
);

// view model for modal pop-up
define("viewmodels/HostModalViewModel", ["knockout"],
    function (ko) {
        return function HostModalViewModel() {

            var hosts = ko.observableArray(),
                template = "hosts-template";

            function close() {
                this.modal.close();
            };

            // public pointers
            return {
                template: template,
                Close: close,
                Hosts: hosts
            };
        };
    }
);

    // view model for modal pop-up
define("viewmodels/ProcessModalViewModel", ["knockout"],
    function (ko) {
        return function ProcessModalViewModel() {

            var processes = ko.observableArray(),
                template = "processes-template";

            function close() {
                this.modal.close();
            };

            // public pointers
            return {
                template: template,
                Close: close,
                Processes: processes
            };
        };
    }
);

define("viewmodels/WorkflowServicesViewModel", ["knockout", "modal", "config", "services/WorkflowDataService", "services/AlertService", "viewmodels/ConnectorModalViewModel", "viewmodels/HostModalViewModel", "viewmodels/ProcessModalViewModel"],
    function (ko, modal, config, workflowDataService, alertService, ConnectorModalViewModel, HostModalViewModel, ProcessModalViewModel) {

        return function WorkflowServicesViewModel() {

            // properties
            var updateTimer = undefined,
                workflowConnectors = ko.observableArray(),
                workflowHosts = ko.observableArray(),
                serviceHosts = ko.observableArray(),
                processInfo = ko.observableArray(),
                serviceHostCount = ko.observable(),
                workflowHostCount = ko.observable(),
                connectorCount = ko.observable(),
                runningConnectorCount = ko.observable(),
                processCount = ko.observable(),
                enabledProcessCount = ko.observable(),
                isLoading = ko.observable(false),
                connectorDetails = new ConnectorModalViewModel(),
                hostDetails = new HostModalViewModel(),
                processDetails = new ProcessModalViewModel();

            var workflowHostsStatus = ko.pureComputed(function () {
                return workflowHostCount() > 0 ? "panel-green" : "panel-red";
            }, this),
            serviceHostsStatus = ko.pureComputed(function () {
                return serviceHostCount() > 0 ? "panel-green" : "panel-red";
            }, this),
            connectorStatus = ko.pureComputed(function () {
                // return whether everything is running
                if (runningConnectorCount() == 0) {
                    return 'panel-red';
                }

                if (connectorCount() > runningConnectorCount()) {
                    return 'panel-yellow';
                }

                return 'panel-green';
            }, this),
            processStatus = ko.pureComputed(function () {
                // return whether everything is running
                if (enabledProcessCount() == 0) {
                    return 'panel-red';
                }

                if (processCount() > enabledProcessCount()) {
                    return 'panel-yellow';
                }

                return 'panel-green';
            }, this);


            // functions
            function loadData() {

                // retrieve data from service and load
                isLoading(true);

                workflowDataService.getServiceInfo()
                    .then(
                        function (data) {
                            workflowConnectors(data.WorkflowConnectors);
                            connectorDetails.Connectors(data.WorkflowConnectors);
                            workflowHosts(data.WorkflowHosts);
                            serviceHosts(data.ServiceHosts);
                            processInfo(data.ProcessInfo);
                            serviceHostCount(data.ServiceHostCount);
                            workflowHostCount(data.WorkflowHostCount);
                            connectorCount(data.ConnectorCount);
                            runningConnectorCount(data.RunningConnectorCount);
                            processCount(data.ProcessCount);
                            enabledProcessCount(data.EnabledProcessCount);
                            processDetails.Processes(data.ProcessInfo);
                        },
                        function (error) {
                            alertService.showAlert("workflow status failed to load");
                        })
                    .always(
                        function () {
                            // done loading
                            isLoading(false);
                        }
                    );
            };

            // open the connector info modal page
            function showConnectorInfo() {
                modal.showModal({
                    viewModel: connectorDetails,
                    context: this
                });
            };

            function showServiceHostInfo() {
                hostDetails.Hosts(serviceHosts());

                modal.showModal({
                    viewModel: hostDetails,
                    context: this
                });
            }

            function showWorkflowHostInfo() {
                hostDetails.Hosts(workflowHosts());

                modal.showModal({
                    viewModel: hostDetails,
                    context: this
                });
            }

            function showProcessInfo() {
                modal.showModal({
                    viewModel: processDetails,
                    context: this
                });
            }

            function init(bindControl) {
                // get initial data
                loadData();

                // set interval for updates
                updateTimer = setInterval(loadData, config.loadInterval);
                ko.applyBindings(this, bindControl);
            }

            function dispose() {
                clearInterval(updateTimer);
            }

            function refresh() {
                loadData();
            }

            // public pointers
            return {
                updateTimer: updateTimer,
                workflowConnectors: workflowConnectors,
                workflowHosts: workflowHosts,
                serviceHosts: serviceHosts,
                serviceHostCount: serviceHostCount,
                workflowHostCount: workflowHostCount,
                connectorCount: connectorCount,
                runningConnectorCount: runningConnectorCount,
                isLoading: isLoading,
                connectorDetails: connectorDetails,
                workflowHostsStatus: workflowHostsStatus,
                serviceHostsStatus: serviceHostsStatus,
                connectorStatus: connectorStatus,
                processInfo: processInfo,
                enabledProcessCount: enabledProcessCount,
                processCount: processCount,
                processStatus: processStatus,


                loadData: loadData,
                showConnectorInfo: showConnectorInfo,
                showWorkflowHostInfo: showWorkflowHostInfo,
                showServiceHostInfo: showServiceHostInfo,
                showProcessInfo: showProcessInfo,

                init: init,
                refresh: refresh,
                dispose: dispose
            };
        };
    }
);

