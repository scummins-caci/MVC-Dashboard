﻿require(["knockout"],
    function(ko) {

        // creates a binding to use select2
        ko.bindingHandlers.select2 = {
            init: function (el, valueAccessor, allBindingsAccessor) {
                ko.utils.domNodeDisposal.addDisposeCallback(el, function () {
                    $(el).select2("destroy");
                });

                var allBindings = allBindingsAccessor(),
                    select2 = ko.utils.unwrapObservable(allBindings.select2);
                $(el).select2(select2);
            },
            update: function (el, valueAccessor, allBindingsAccessor) {
                var allBindings = allBindingsAccessor();
                var select2 = ko.utils.unwrapObservable(allBindings.select2);

                var previousCount = ko.utils.domData.get(el, "userCount");
                ko.utils.domData.set(el, "userCount", select2.data.length);

                if (previousCount != undefined && previousCount !== select2.data.length) {
                    //$(el).select2({ data: function () { return { results: allBindings.options() } } });
                    $(el).select2("destroy");
                    $(el).select2(select2);
                    return;
                }

                if ("value" in allBindings) {
                    if (allBindings.select2.multiple && allBindings.value().constructor !== Array) {
                        //$(el).select2("val", allBindings.value().split(","));
                        $(el).val(allBindings.value().split(","));
                    }
                    else {
                        //$(el).select2("val", allBindings.value());
                        $(el).val(allBindings.value());
                    }
                } else if ("selectedOptions" in allBindings) {
                    var converted = [];
                    var textAccessor = function (value) { return value; };
                    if ("optionsText" in allBindings) {
                        textAccessor = function (value) {
                            var valueAccessor = function (item) { return item; }
                            if ("optionsValue" in allBindings) {
                                valueAccessor = function (item) { return item[allBindings.optionsValue]; }
                            }
                            var items = $.grep(allBindings.options(), function (e) { return valueAccessor(e) === value });
                            if (items.length === 0 || items.length > 1) {
                                return "UNKNOWN";
                            }
                            return items[0][allBindings.optionsText];
                        }
                    }
                    $.each(allBindings.selectedOptions(), function (key, value) {
                        converted.push({ id: value, text: textAccessor(value) });
                    });
                    $(el).select2("data", converted);
                }
            }
        };
});