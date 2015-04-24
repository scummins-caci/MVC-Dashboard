describe("Testing DataflowReceiptsViewModel", function() {

    debugger;

    // build test data service
    var mockData = {
        "ItemCount": 1106,
        "Items": [{
                "HarmonyNumber": "DF-DERIVTEST-SEC-0201",
                "ChangeId": 1191,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:19",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0200",
                "ChangeId": 1190,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:18",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0199",
                "ChangeId": 1189,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:18",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0198",
                "ChangeId": 1188,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:17",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0197",
                "ChangeId": 1187,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:16",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0196",
                "ChangeId": 1186,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:15",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0195",
                "ChangeId": 1185,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:14",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0194",
                "ChangeId": 1184,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:13",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0193",
                "ChangeId": 1183,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:12",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0192",
                "ChangeId": 1182,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:11",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0191",
                "ChangeId": 1181,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:10",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0190",
                "ChangeId": 1180,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:09",
                "DisplayExtractDate": "12/22/2014"
            },
            {
                "HarmonyNumber": "DF-DERIVTEST-SEC-0189",
                "ChangeId": 1179,
                "ReceiptType": "R",
                "Action": "I",
                "ExtractDate": "2014-12-22T12:20:08",
                "DisplayExtractDate": "12/22/2014"
            }]
    };

    var mockWorkflowService = {
        getReceipts: function(page, pageSize) {
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
    beforeEach(function(done) {
        require(["squire"], function(Squire) {
            var injector = new Squire();

            // mock service and config
            var builder = injector.mock({
                "services/DataflowDataService": mockWorkflowService,
                "config": mockConfig
            });

            // create a div to bind viewmodel to
            var koBindDiv = document.createElement('div');
            document.body.appendChild(koBindDiv);

            // build view model with mocked service
            builder.require(["viewmodels/DataflowReceiptsViewModel"], function(DataflowReceiptsViewModel) {
                viewModel = new DataflowReceiptsViewModel();
                viewModel.init(koBindDiv);
                done();
            });
        });
    });

    afterEach(function() {
        viewModel.dispose();
    });

    it("should return receipts", function() {
        expect(viewModel.receipts()).toEqual(mockData.Items);
    });

    it("should return receipt count", function() {
        expect(viewModel.receiptCount()).toEqual(mockData.ItemCount);
    });
});
