
// a service for displaying alerts
//define("services/AlertService", ["knockout", "config", "bindings"],
define("services/AlertService", ["knockout", "config"],
    function (ko, config) {

        // create view model and bind;  this will hold the messages to display
        function AlertViewModel() {

            var alerts = ko.observableArray();

            function closeAlert(alert) {
                alerts.remove(alert);
            }

            function addAlert(type, message) {
                var alert = { type: type, message: message };

                alerts.push(alert);

                setTimeout(function () {
                    closeAlert(alert);
                }, 5000);
            };

            return {
                alerts: alerts,
                addAlert: addAlert
            };
        }

        var alertViewModel = new AlertViewModel();
        ko.applyBindings(alertViewModel, document.getElementById(config.alertBoxControl));

        // service exposed functions for updating global alert viewmodel
        function displayMessage(type, message) {

            alertViewModel.addAlert(type, message);
        }

        function showAlert(message) {
            displayMessage('alert', message);
        }

        function showWarning(message) {
            displayMessage('warning', message);
        }

        function showInfo(message) {
            displayMessage('info', message);
        }

        function showSuccess(message) {
            displayMessage('success', message);
        }

        return {
            showAlert: showAlert,
            showWarning: showWarning,
            showSuccess: showSuccess,
            showInfo: showInfo
        };
    }
);