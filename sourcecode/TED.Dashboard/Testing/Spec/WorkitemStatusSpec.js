describe("Testing WorkitemStatusViewModel", function () {

    debugger;

    // build test data service
    var mockData = [{ "Count": 1168, "Status": "ready" }, { "Count": 33, "Status": "failed"}];

    var mockWorkflowService = {
        getStatusCounts: function () {
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
            builder.require(["viewmodels/WorkitemStatusViewModel"], function (WorkitemStatusViewModel) {
                viewModel = new WorkitemStatusViewModel();
                viewModel.init(koBindDiv);
                done();
            });
        });
    });

    afterEach(function () {
        viewModel.dispose();
    });

    it("statuses should match", function () {
        expect(viewModel.statuses()).toEqual(mockData);
    });
});
