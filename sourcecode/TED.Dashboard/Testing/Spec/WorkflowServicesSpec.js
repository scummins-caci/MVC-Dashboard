
describe("Testing WorkflowServicesViewModel", 
    function () {

        debugger;

        // build test data service
        var mockData = {
            "WorkflowConnectors": [
                {
                    "Name": "Cleanup From Auto Update Batch Failure",
                    "IsEnabled": true,
                    "HostName": "CATWFH",
                    "Instances": 1
                },
                {
                    "Name": "Detect PDF Fonts",
                    "IsEnabled": true,
                    "HostName": "CATWFH",
                    "Instances": 1
                },
                {
                    "Name": "Export",
                    "IsEnabled": true,
                    "HostName": "",
                    "Instances": 0
                }
            ],
            "WorkflowHosts": [{
                "Name": "CATWFH",
                "TimeStamp": "11/18/2014 4:14 PM"
            }],
            "ServiceHosts": [{
                "Name": "CATWFH",
                "TimeStamp": "11/18/2014 4:14 PM"
            }],
            "ProcessInfo": [{
			    "ID": 1,
			    "Name": "Content Processing",
			    "IsEnabled": true
		    },
		    {
			    "ID": 2,
			    "Name": "Validation Process",
			    "IsEnabled": true
		    },
		    {
			    "ID": 3,
			    "Name": "Human Translation",
			    "IsEnabled": false
		    },
		    {
			    "ID": 4,
			    "Name": "Upload to Harmony",
			    "IsEnabled": false
		    }],
            "WorkflowHostCount": 1,
            "ServiceHostCount": 1,
            "ConnectorCount": 3,
            "RunningConnectorCount": 2,
            "ProcessCount": 4,
		    "EnabledProcessCount": 2
        };

        var mockWorkflowService = {
            getServiceInfo: function () {
                var deferred = $.Deferred();
                deferred.resolve(mockData);
                return deferred.promise();
            }
        };

        var mockConfig = {
            loadInterval: 5000
        };

        // inject mock service into view model instance
        var viewModel = undefined;
        beforeEach(function (done) {
            require(["squire"], function (Squire) {
                var injector = new Squire();

                // create a div to bind viewmodel to
                var koBindDiv = document.createElement('div');
                document.body.appendChild(koBindDiv);

                // mock service and config
                var builder = injector.mock({
                    "services/WorkflowDataService": mockWorkflowService,
                    "config": mockConfig
                });

                // build view model with mocked service
                builder.require(["viewmodels/WorkflowServicesViewModel"], function (WorkflowServicesViewModel) {
                    viewModel = new WorkflowServicesViewModel();
                    viewModel.init(koBindDiv);
                    done();
                });
            });
        });

        afterEach(function () {
            viewModel.dispose();
        });

        it("connector items should match", function () {
            expect(viewModel.workflowConnectors()).toEqual(mockData.WorkflowConnectors);
        });

        it("workflow host items should match", function () {
            expect(viewModel.workflowHosts()).toEqual(mockData.WorkflowHosts);
        });

        it("service host items should match", function () {
            expect(viewModel.serviceHosts()).toEqual(mockData.ServiceHosts);
        });

        it("process items should match", function () {
            expect(viewModel.processInfo()).toEqual(mockData.ProcessInfo);
        });

        it("service host counts should match", function () {
            expect(viewModel.serviceHostCount()).toEqual(mockData.ServiceHostCount);
        });

        it("workflow host counts should match", function () {
            expect(viewModel.workflowHostCount()).toEqual(mockData.WorkflowHostCount);
        });

        it("connector counts should match", function () {
            expect(viewModel.connectorCount()).toEqual(mockData.ConnectorCount);
        });

        it("running connector counts should match", function () {
            expect(viewModel.runningConnectorCount()).toEqual(mockData.RunningConnectorCount);
        });

        it("enabled process count should match", function () {
            expect(viewModel.enabledProcessCount()).toEqual(mockData.EnabledProcessCount);
        });

        it("process count should match", function () {
            expect(viewModel.processCount()).toEqual(mockData.ProcessCount);
        });

        it("host status should be green", function () {
            expect(viewModel.workflowHostsStatus()).toEqual("panel-green");
        });

        it("service host status should be green", function () {
            expect(viewModel.serviceHostsStatus()).toEqual("panel-green");
        });

        it("connector status should be yellow", function () {
            expect(viewModel.connectorStatus()).toEqual("panel-yellow");
        });

        it("process status should be yellow", function () {
            expect(viewModel.processStatus()).toEqual("panel-yellow");
        });

    }
);