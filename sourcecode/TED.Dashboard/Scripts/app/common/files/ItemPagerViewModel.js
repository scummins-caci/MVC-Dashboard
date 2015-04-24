define(["knockout"], function(ko) {

    return function DynamicTableViewModel(params) {
        var self = this;

        self.currentPage = params.CurrentPage;
        self.itemCount = params.TotalCount;

        var pageSize = ko.computed(function () {
            return parseInt(params.PageCount());
        });

        // if pagesize changes, go back to page 1
        pageSize.subscribe(function() {
            self.currentPage(1);
        });

        self.totalPages = ko.computed(function () {
            var pages = Math.floor(self.itemCount() / pageSize());
            pages += self.itemCount() % pageSize() > 0 ? 1 : 0;
            return pages;
            //return pages - 1;
        });

        self.endIndex = ko.computed(function () {
            var end = pageSize() * self.currentPage();
            return end > self.itemCount() ? self.itemCount() : end;
        });

        self.startIndex = ko.computed(function () {
            return self.endIndex() > pageSize() ? self.endIndex() - pageSize() + 1 : 1;
        });

        self.hasPrevious = ko.computed(function () {
            return self.currentPage() !== 1;
        });

        self.hasNext = ko.computed(function () {
            return self.currentPage() < self.totalPages();
        });

        self.next = function () {
            if (self.currentPage() < self.totalPages()) {
                self.currentPage(self.currentPage() + 1);
            }
        };

        self.previous = function () {
            if (self.currentPage() !== 0) {
                self.currentPage(self.currentPage() - 1);
            }
        };
    }
});