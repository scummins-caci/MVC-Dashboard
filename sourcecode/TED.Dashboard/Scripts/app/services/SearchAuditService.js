// a service for retrieving workflow related dashboard data
define("services/SearchAuditService", ["config", "services/HttpService"],
    function (config, httpService) {

        function getSearchCounts() {
            return httpService.requestData(config.baseUrl + "searchaudit/counts");
        }

        function getSearchStatistics() {
            return httpService.requestData(config.baseUrl + "searchaudit/statistics");
        }

        function getSearches(page, pageSize) {
            var url = config.baseUrl + "searchaudit/searches";
            if (page !== undefined) {
                url += "?page=" + page;
            }
            if (pageSize !== undefined) {
                url += "&pageSize=" + pageSize;
            }

            return httpService.requestData(url);
        }

        // exposed functions
        return {
            getSearchCounts: getSearchCounts,
            getSearches: getSearches,
            getSearchStatistics: getSearchStatistics
        };
    }
);