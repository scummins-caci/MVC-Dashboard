describe("Testing WorkitemCountsViewModel", function () {

    debugger;

    // build test data service
    var mockData = [{ "Count": 1, "Activity": "Indicate UUID Addressed for Batch", "Type": "Automated" },
                    { "Count": 369, "Activity": "OCR Error", "Type": "Manual" },
                    { "Count": 319, "Activity": "QA Review", "Type": "Manual" },
                    { "Count": 212, "Activity": "Multimedia QA", "Type": "Manual" },
                    { "Count": 49, "Activity": "Text Extraction Error", "Type": "Manual" },
                    { "Count": 49, "Activity": "Missing Required Fields", "Type": "Manual" },
                    { "Count": 46, "Activity": "Arabic Screening", "Type": "Manual" },
                    { "Count": 37, "Activity": "QA Perform Merge", "Type": "Manual" },
                    { "Count": 34, "Activity": "QA Review Batch", "Type": "Manual" },
                    { "Count": 10, "Activity": "QA Define LOV Mappings", "Type": "Manual" },
                    { "Count": 6, "Activity": "Phonetic Indexing Error", "Type": "Manual" },
                    { "Count": 6, "Activity": "Media Conversion Error", "Type": "Manual" },
                    { "Count": 5, "Activity": "Metadata NEE Error", "Type": "Manual" },
                    { "Count": 5, "Activity": "QA Reused Batch Name", "Type": "Manual" },
                    { "Count": 4, "Activity": "QA Batch Name Mismatch", "Type": "Manual" },
                    { "Count": 4, "Activity": "QA Perform Batch Merge", "Type": "Manual" },
                    { "Count": 4, "Activity": "Delete Sections Error", "Type": "Manual" },
                    { "Count": 3, "Activity": "QA Checkout Error", "Type": "Manual" },
                    { "Count": 3, "Activity": "NEE Error", "Type": "Manual" },
                    { "Count": 2, "Activity": "English Screening", "Type": "Manual" },
                    { "Count": 2, "Activity": "SCAP Error", "Type": "Manual" },
                    { "Count": 2, "Activity": "KFE Error", "Type": "Manual" },
                    { "Count": 1, "Activity": "QA Batch Missing Required Fields", "Type": "Manual" },
                    { "Count": 1, "Activity": "QA Invalid Classifications", "Type": "Manual" },
                    { "Count": 1, "Activity": "Expedited Review", "Type": "Manual" },
                    { "Count": 1, "Activity": "QA Auto Update Failed", "Type": "Manual" }, 
                    { "Count": 1, "Activity": "General Screening", "Type": "Manual"}];

    var mockWorkflowService = {
        getInbasketCounts: function () {
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
            builder.require(["viewmodels/WorkitemCountsViewModel"], function (WorkitemCountsViewModel) {
                viewModel = new WorkitemCountsViewModel();
                viewModel.init(koBindDiv);
                done();
            });
        });
    });

    afterEach(function () {
        viewModel.dispose();
    });

    it("should return only first 8", function () {
        expect(viewModel.workitemCounts().length).toEqual(8);
    });

    // paging
    it("total items count", function () {
        expect(viewModel.totalItems()).toEqual(mockData.length);
    });

    it("testing the first page", function () {
        expect(viewModel.currentPage()).toEqual(1);

        expect(viewModel.hasPrevious()).toEqual(false);
        expect(viewModel.hasNext()).toEqual(true);
        expect(viewModel.startIndex()).toEqual(1);
        expect(viewModel.endIndex()).toEqual(8);
    });

    it("testing paging twice", function () {

        // act
        viewModel.next();
        viewModel.next();

        expect(viewModel.currentPage()).toEqual(3);

        expect(viewModel.hasPrevious()).toEqual(true);
        expect(viewModel.hasNext()).toEqual(true);
        expect(viewModel.startIndex()).toEqual(17);
        expect(viewModel.endIndex()).toEqual(24);
    });
});
