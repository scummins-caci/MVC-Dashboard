define(["knockout", "delegates"], function (ko) {

    return function DynamicTableViewModel(params) {
        var self = this;

        self.previousSelectedIndex = -1;

        self.PopulateData = function (items) {
            items.forEach(function (item) {
                item.selected = false;
            });

            self.Items(items);
        };

        self.Columns = params.Columns;
        self.Items = ko.computed(function() {
            var items = params.Items();

            items.forEach(function (item) {
                if (item.selected == undefined) {
                    item.selected = ko.observable(false);
                }
            });
            params.Items(items);
            return items;
        });

        self.Resizing = ko.observable(false);

        var resetSelected = function () {
            self.Items().forEach(function (item) {
                item.selected(false);
            });
        };

        self.columnsRendered = function (elements) {

            // if all columns are rendered, resize
            if ($(elements).parent().children().length === self.Columns().length) {
                // flip resizing
                self.Resizing(true);
                self.Resizing(false);
            }
        };

        // check for a sort function
        self.sortColumn = undefined;
        if (params.Sort) {
            self.sortColumn = params.Sort;
        } else {
            self.sortColumn = function() {
                console.log("No sort method specified.");
            }
        }

        self.rowSelected = function (item, e, itemIndex, parent, parentIndex) {

            // shifted click event to cell, so need parent vs. item

            var newState = !parent.selected();
            var currentIndex = parentIndex();

            if (!e.ctrlKey && !e.shiftKey && !e.metaKey) {
                resetSelected();
            }

            if (e.shiftKey && self.previousSelectedIndex !== -1) {
                var startIndex = 0;
                var endIndex = 0;
                if (currentIndex > self.previousSelectedIndex) {
                    startIndex = self.previousSelectedIndex;
                    endIndex = currentIndex;
                }
                else {
                    startIndex = currentIndex;
                    endIndex = self.previousSelectedIndex;
                }

                // set all items to the new state
                for (var i = startIndex; i < endIndex; i++) {
                    self.Items()[i].selected(newState);
                }
            }

            parent.selected(newState);

            // retain selected index
            self.previousSelectedIndex = currentIndex;
        };
    }
});