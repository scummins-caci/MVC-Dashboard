// a service for retrieving data from http data endpoints
define("services/HttpService", [],
    function () {

        function requestData(url) {
            var deferred = $.Deferred();

            $.getJSON(url).then(
                    function (data) {
                        if (data.Success) {
                            deferred.resolve(data.Data);

                        } else {
                            // something went wrong during the request, return nothing
                            deferred.reject();
                        }
                    },
                    function () {
                        deferred.reject();
                    });

            return deferred.promise();
        }

        // exposed functions
        return {
            requestData: requestData
        };
    }
);