
describe("Testing NotificationsViewModel", function () {

    debugger;

    // build test data service
    var mockData = {
        "ItemCount": 100,
        "Items":
        [
            {
                "LogId": "fc46e421-4d64-4af4-86eb-9d9c70298578",
                "ClientMachine": "10.19.132.52",
                "TimeStamp": "2014-11-18T15:02:40.511",
                "Location": "HighView.Services",
                "Severity": "INFO",
                "Message": "Service UserPermissions created.",
                "DisplayTime": "1 hour ago",
                "DisplayLocation": "HighViews"
            },
            {
                "LogId": "f1f48935-e153-422a-9f17-f0087ec406b1",
                "ClientMachine": "CATWFH",
                "TimeStamp": "2014-11-17T17:06:58.245",
                "Location": "HighView.Services.RuleValidation.Service",
                "Severity": "ERROR",
                "Message": "Loaded export validation rules.",
                "DisplayTime": "23 hours ago",
                "DisplayLocation": "RuleValidation"
            },
            {
                "LogId": "415b5d45-783b-4841-a6a9-69396c297b7d",
                "ClientMachine": "CATWFH",
                "TimeStamp": "2014-11-17T16:54:22.685",
                "Location": "HighView.Services.QA.TestForClassificationIssues",
                "Severity": "INFO",
                "Message": "Test For Classification Issues -- Completed Execute on instId=29583603",
                "DisplayTime": "23 hours ago",
                "DisplayLocation": "QA.TestForClassificationIssues"
            },
            {
                "LogId": "81ac2613-c049-40c8-9382-eb8d985d3d4a",
                "ClientMachine": "CATWFH",
                "TimeStamp": "2014-11-17T16:54:22.67",
                "Location": "HighView.Services.QA.TestForClassificationIssues",
                "Severity": "WARNING",
                "Message": "Test For Classification Issues -- Begin Execute on instId=29583603",
                "DisplayTime": "23 hours ago",
                "DisplayLocation": "QA.TestForClassificationIssues"
            }
        ]
    };

    var mockWorkflowService = {
        getNotifications: function () {
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

            // mock service and config
            var builder = injector.mock({
                "services/WorkflowDataService": mockWorkflowService,
                "config": mockConfig
            });

            // create a div to bind viewmodel to
            var koBindDiv = document.createElement('div');
            document.body.appendChild(koBindDiv);

            // build view model with mocked service
            builder.require(["viewmodels/NotificationsViewModel"], function (NotificationsViewModel) {
                viewModel = new NotificationsViewModel();
                viewModel.init(koBindDiv);
                done();
            });
        });
    });

    afterEach(function () {
        viewModel.dispose();
    });

    it("logs should match", function () {
        expect(viewModel.workflowLogs()).toEqual(mockData.Items);
    });

    it("full item count should match", function () {
        expect(viewModel.logCount()).toEqual(mockData.ItemCount);
    });

    // testing paging
    it("should be on first page, settings should match", function () {
        expect(viewModel.currentPage()).toEqual(1);

        expect(viewModel.hasPrevious()).toEqual(false);
        expect(viewModel.hasNext()).toEqual(true);
        expect(viewModel.startIndex()).toEqual(1);
        expect(viewModel.endIndex()).toEqual(10);
    });

    // testing paging
    it("move to next page page, settings should match", function () {

        // act
        viewModel.next();

        // assess
        expect(viewModel.currentPage()).toEqual(2);

        expect(viewModel.hasPrevious()).toEqual(true);
        expect(viewModel.hasNext()).toEqual(true);
        expect(viewModel.startIndex()).toEqual(11);
        expect(viewModel.endIndex()).toEqual(20);
    });
});

