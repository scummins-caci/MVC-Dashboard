// a service for retrieving workflow related dashboard data
define("services/WorkflowDataService", ["config", "services/HttpService"],
    function (config, httpService) {

        function getInbasketCounts() {
            return httpService.requestData(config.baseUrl + "workflow/queuecounts");
        }

        function getStatusCounts() {
            return httpService.requestData(config.baseUrl + "workflow/statuscounts");
        }

        function getNotifications(page, pageSize) {
            var url = config.baseUrl + "notifications";
            if (page !== undefined) {
                url += "?page=" + page;
            }
            if (pageSize !== undefined) {
                url += "&pageSize=" + pageSize;
            }

            return httpService.requestData(url);
        }

        function getServiceInfo() {
            return httpService.requestData(config.baseUrl + "workflowstatus");
        }

        function buildPageUrl(page, pageSize, filters, orderColumns) {

            // take the page and pageSize and convert to odata skip/top
            page = page != undefined ? page : 1;
            pageSize = pageSize != undefined ? pageSize : 50;
            var skip = (page - 1) * pageSize;

            var url = "$skip=" + skip + "&$top=" + pageSize;

            if (filters != undefined && filters.length > 0) {

                var filter = "";
                filters.forEach(function (item) {
                    filter += " and " + item.column + " " + item.operator + " '" + item.value + "' ";
                });

                url += "&$filter=" + filter.substring(5);
            }

            if (orderColumns != undefined && orderColumns.length > 0) {
                var orderBy = "";
                orderColumns.forEach(function(item) {
                    orderBy += "," + item.column + " " + item.order;
                });

                url += "&$orderby=" + orderBy.substring(1);
            }

            return url;
        }

        /**** inbasket related items   ****/
        function getProcesses(page, pageSize, filters, orderBy) {
            var url = config.baseUrl + "workflow/processes?" + buildPageUrl(page, pageSize, filters, orderBy);
            return httpService.requestData(url);
        }

        function getAdminProcesses(page, pageSize, filters, orderBy) {
            var url = config.baseUrl + "workflow/adminprocesses?" + buildPageUrl(page, pageSize, filters, orderBy);
            return httpService.requestData(url);
        }

        function getQueues(processId, page, pageSize, filters, orderBy) {
            processId = processId != undefined ? processId : 0;
            var url = config.baseUrl + "workflow/queues/" + processId + "?" + buildPageUrl(page, pageSize, filters, orderBy);
            return httpService.requestData(url);
        }

        function getWorkitems(processId, queueId, page, pageSize, filters, orderBy) {
            processId = processId != undefined ? processId : 0;
            var url = config.baseUrl + "workflow/workitems/" + processId + "/" + queueId + "?" + buildPageUrl(page, pageSize, filters, orderBy);
            return httpService.requestData(url);
        }

        function getQueueUsers(queueId) {
            return httpService.requestData(config.baseUrl + "workflow/users/" + queueId);
        }

        // exposed functions
        return {
            getNotifications: getNotifications,
            getServiceInfo: getServiceInfo,
            getInbasketCounts: getInbasketCounts,
            getStatusCounts: getStatusCounts,
            getProcesses: getProcesses,
            getAdminProcesses: getAdminProcesses,
            getQueues: getQueues,
            getWorkitems: getWorkitems,
            getQueueUsers: getQueueUsers
        };
    }
);
