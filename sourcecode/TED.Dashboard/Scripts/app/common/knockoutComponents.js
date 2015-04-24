define(["knockout"],
    function (ko) {

        // custom table
        ko.components.register("dynamic-table", {
            viewModel: { require: "app/common/files/DynamicTableViewModel" },
            template: { require: "text!app/common/files/DynamicTable.html" }
        });

        // pager
        ko.components.register("item-pager", {
            viewModel: { require: "app/common/files/ItemPagerViewModel" },
            template: { require: "text!app/common/files/ItemPager.html"}
        });
    }
);