describe("Testing SearchAuditViewModel", function () {

    // build test data service
    var mockData = {
        "ItemCount": 100,
        "Items": [{
                "UserName": "Dean Eileen",
                "DateExecuted": "2015-01-12T15:40:39.617",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["AFGP-2015-TEDW8042001"]
                }],
                "DisplayTime": "26 minutes ago",
                "DisplayDate": "1/12/2015 3:40 PM"
            },
            {
                "UserName": "Dean Eileen",
                "DateExecuted": "2015-01-12T14:38:40.126",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["AFGP-2015-AAT8042002"]
                }],
                "DisplayTime": "1 hour ago",
                "DisplayDate": "1/12/2015 2:38 PM"
            },
            {
                "UserName": "Coulson Mathew",
                "DateExecuted": "2015-01-12T14:37:05.317",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["66MI-2013-CCX1001"]
                }],
                "DisplayTime": "1 hour ago",
                "DisplayDate": "1/12/2015 2:37 PM"
            },
            {
                "UserName": "Dean Eileen",
                "DateExecuted": "2015-01-12T14:34:34.386",
                "Criteria": [{
                    "FilterName": "Date Created (System)",
                    "Operator": "between",
                    "Operands": ["1/5/2015",
                        "1/13/2015"]
                }],
                "DisplayTime": "1 hour ago",
                "DisplayDate": "1/12/2015 2:34 PM"
            },
            {
                "UserName": "Dean Eileen",
                "DateExecuted": "2015-01-12T14:18:41.325",
                "Criteria": [{
                    "FilterName": "Date Created (System)",
                    "Operator": "between",
                    "Operands": ["1/5/2015",
                        "1/13/2015"]
                }],
                "DisplayTime": "1 hour ago",
                "DisplayDate": "1/12/2015 2:18 PM"
            },
            {
                "UserName": "Coulson Mathew",
                "DateExecuted": "2015-01-12T14:18:40.638",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["66MI-2013-CCX1001"]
                }],
                "DisplayTime": "1 hour ago",
                "DisplayDate": "1/12/2015 2:18 PM"
            },
            {
                "UserName": "Dean Eileen",
                "DateExecuted": "2015-01-12T14:14:51.846",
                "Criteria": [{
                    "FilterName": "Date Created (System)",
                    "Operator": "between",
                    "Operands": ["1/5/2015",
                        "1/13/2015"]
                }],
                "DisplayTime": "1 hour ago",
                "DisplayDate": "1/12/2015 2:14 PM"
            },
            {
                "UserName": "Dean Eileen",
                "DateExecuted": "2015-01-12T14:11:26.274",
                "Criteria": [{
                    "FilterName": "Date Created (System)",
                    "Operator": "between",
                    "Operands": ["1/5/2015",
                        "1/13/2015"]
                }],
                "DisplayTime": "1 hour ago",
                "DisplayDate": "1/12/2015 2:11 PM"
            },
            {
                "UserName": "Coulson Mathew",
                "DateExecuted": "2015-01-12T13:49:57.824",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["66MI-2013-CCX1001"]
                }],
                "DisplayTime": "2 hours ago",
                "DisplayDate": "1/12/2015 1:49 PM"
            },
            {
                "UserName": "ORA TEST 2",
                "DateExecuted": "2015-01-12T13:46:19.971",
                "Criteria": [{
                    "FilterName": "Date Created (System)",
                    "Operator": "between",
                    "Operands": ["1/5/2015",
                        "1/13/2015"]
                }],
                "DisplayTime": "2 hours ago",
                "DisplayDate": "1/12/2015 1:46 PM"
            },
            {
                "UserName": "Coulson Mathew",
                "DateExecuted": "2015-01-12T13:44:38.044",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["66MI-2013-CCX1001"]
                }],
                "DisplayTime": "2 hours ago",
                "DisplayDate": "1/12/2015 1:44 PM"
            },
            {
                "UserName": "Coulson Mathew",
                "DateExecuted": "2015-01-12T13:43:54.245",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["66MI-2013-CCX1001"]
                }],
                "DisplayTime": "2 hours ago",
                "DisplayDate": "1/12/2015 1:43 PM"
            },
            {
                "UserName": "Durant Angela",
                "DateExecuted": "2015-01-12T13:23:04.449",
                "Criteria": [{
                    "FilterName": "Date Created (System)",
                    "Operator": "between",
                    "Operands": ["1/5/2015",
                        "1/13/2015"]
                }],
                "DisplayTime": "2 hours ago",
                "DisplayDate": "1/12/2015 1:23 PM"
            },
            {
                "UserName": "ORA TEST 2",
                "DateExecuted": "2015-01-12T08:56:45.948",
                "Criteria": [{
                    "FilterName": "Text",
                    "Operator": "contains",
                    "Operands": ["engli%"]
                }],
                "DisplayTime": "7 hours ago",
                "DisplayDate": "1/12/2015 8:56 AM"
            },
            {
                "UserName": "ORA TEST 2",
                "DateExecuted": "2015-01-12T08:56:30.431",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["LOV-2014-000001"]
                }],
                "DisplayTime": "7 hours ago",
                "DisplayDate": "1/12/2015 8:56 AM"
            },
            {
                "UserName": "ORA TEST 2",
                "DateExecuted": "2015-01-12T08:56:12.821",
                "Criteria": [{
                    "FilterName": "Text",
                    "Operator": "contains",
                    "Operands": ["engli%"]
                }],
                "DisplayTime": "7 hours ago",
                "DisplayDate": "1/12/2015 8:56 AM"
            },
            {
                "UserName": "ORA TEST 2",
                "DateExecuted": "2015-01-12T08:55:49.085",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["470MI-2014%"]
                }],
                "DisplayTime": "7 hours ago",
                "DisplayDate": "1/12/2015 8:55 AM"
            },
            {
                "UserName": "ORA TEST 2",
                "DateExecuted": "2015-01-12T08:48:56.752",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["URM-2014-99H98%"]
                }],
                "DisplayTime": "7 hours ago",
                "DisplayDate": "1/12/2015 8:48 AM"
            },
            {
                "UserName": "ORA TEST 2",
                "DateExecuted": "2015-01-12T08:48:06.89",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["WATIN-2014-011215084410URM"]
                }],
                "DisplayTime": "7 hours ago",
                "DisplayDate": "1/12/2015 8:48 AM"
            },
            {
                "UserName": "ORA TEST 2",
                "DateExecuted": "2015-01-12T08:47:18.887",
                "Criteria": [{
                    "FilterName": "Harmony Number",
                    "Operator": "like",
                    "Operands": ["WATIN-2014-011215084410TRM"]
                }],
                "DisplayTime": "7 hours ago",
                "DisplayDate": "1/12/2015 8:47 AM"
            }]
    };

        var mockSearchService = {
        getSearches: function () {
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
                "services/SearchAuditService": mockSearchService,
                "config": mockConfig
            });

            // create a div to bind viewmodel to
            var koBindDiv = document.createElement('div');
            document.body.appendChild(koBindDiv);

            // build view model with mocked service
            builder.require(["viewmodels/SearchAuditViewModel"], function (SearchAuditViewModel) {
                viewModel = new SearchAuditViewModel();
                viewModel.init(koBindDiv);
                done();
            });
        });
    });

    afterEach(function () {
        viewModel.dispose();
    });

    it("searches should match", function () {
        expect(viewModel.searchAudits()).toEqual(mockData.Items);
    });

    it("full item count should match", function () {
        expect(viewModel.searchCount()).toEqual(mockData.ItemCount);
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
    it("move to next page, settings should match", function () {

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
