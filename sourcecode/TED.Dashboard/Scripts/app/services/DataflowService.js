// a service for retrieving workflow related dashboard data
define("services/DataflowDataService", ["config", "services/HttpService"],
    function (config, httpService) {

        function getReceipts(page, pageSize) {
            var url = config.baseUrl + "dataflow/receipts";
            if (page !== undefined) {
                url += "?page=" + page;
            }
            if (pageSize !== undefined) {
                url += "&pageSize=" + pageSize;
            }

            return httpService.requestData(url);
        }

        function getChangeInfo(changeId) {
            return httpService.requestData(config.baseUrl + "dataflow/changetracking/" + changeId);
        }

        // exposed functions
        return {
            getReceipts: getReceipts,
            getChangeInfo: getChangeInfo
        };
    }
);