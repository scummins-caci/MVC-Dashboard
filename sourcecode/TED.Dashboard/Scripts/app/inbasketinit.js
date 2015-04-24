requirejs.config({
    urlArgs: "bust=" + (new Date()).getTime(),
    baseUrl: "Scripts",
    shim: {
        "bootstrap": { "deps": ["jquery"] }
    },
    paths: {

        jquery: "jquery-2.1.3",
        knockout: "knockout-3.2.0",
        modal: "bootstrapko-modal",
        delegates: "knockout-delegatedEvents",
        components: "app/common/knockoutComponents",
        bindings: "app/common/knockoutBindingHandlers",
        services: "app/services",
        vm: "app/viewmodels",
        config: "app/config"
    }
});

// initialize application
require(["knockout", "vm/ComponentInbasketViewModel", "domReady!", "bootstrap"], function (ko, InbasketViewModel) {
    ko.applyBindings(new InbasketViewModel(), document.getElementById("inbasketDisplay"));
});