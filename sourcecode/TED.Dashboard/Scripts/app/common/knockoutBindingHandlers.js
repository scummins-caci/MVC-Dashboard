require(["jquery", "knockout"],
    function ($, ko) {
        var defaultClosedWidth = 26;

        // fade items as they are shown/hidden
        ko.bindingHandlers.fadeVisible = {
            init: function (element, valueAccessor) {
                // Initially set the element to be instantly visible/hidden depending on the value
                var value = valueAccessor();
                $(element).toggle(ko.unwrap(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
            },
            update: function (element, valueAccessor) {
                // Whenever the value subsequently changes, slowly fade the element in or out
                var value = valueAccessor();
                ko.unwrap(value) ? $(element).fadeIn() : $(element).fadeOut();
            }
        };

        ko.bindingHandlers.pinnedSideBar = {
            init: function init(element, valueAccessor) {
                // set variable for initialize
                ko.utils.domData.set(element, "initialized", true);
                var value = valueAccessor();
                var sidebar = $(element);

                // add hover events
                var onHover = function () {
                    // only expand if bar isn't pinned
                    if (!value.pin()) {
                        sidebar.stop().animate({ width: value.width }, function () { value.visible(true); });
                    }
                }
                var onHoverOut = function () {
                    if (!value.pin()) {
                        value.visible(false);
                        sidebar.stop().animate({ width: defaultClosedWidth });
                    }
                }
                sidebar.hover(onHover, onHoverOut);
            },
            update: function update(element, valueAccessor) {
                var value = valueAccessor();
                var pinned = ko.utils.unwrapObservable(value.pin());
                // if initializing, leave current state
                var initializing = ko.utils.domData.get(element, "initialized");
                ko.utils.domData.set(element, "initialized", false);
                if (initializing) return;

                // check to see if a resizing variable exists
                if (value.resizing != undefined) {
                    value.resizing(true);
                }
                var panelWidth = 0;
                var sidebar = $(element);
                var mainPanel = $("#" + value.maindiv);
                if (pinned) {
                    panelWidth = value.width;
                } else {
                    panelWidth = defaultClosedWidth;
                }
                if (value.side === "left") {
                    sidebar.css({ width: panelWidth });
                    mainPanel.css({ left: panelWidth });
                }
                else if (value.side === 'right') {
                    sidebar.css({ width: panelWidth });
                    mainPanel.css({ right: panelWidth });
                }
                if (value.resizing != undefined) {
                    value.resizing(false);
                }
            }
        };

        ko.bindingHandlers.colapsingSideBar = {
            init: function init(element) {

                // set variable for initialize
                ko.utils.domData.set(element, "initialized", true);
            },
            update: function update(element, valueAccessor) {
                var value = valueAccessor();
                var show = ko.utils.unwrapObservable(value.show);

                // if initializing, leave current state
                var initializing = ko.utils.domData.get(element, "initialized");
                ko.utils.domData.set(element, "initialized", false);
                if (initializing) return;

                // check to see if a resizing variable exists
                if (value.resizing != undefined) {
                    value.resizing(true);
                }
                var panelWidth = 0;
                var sidebar = $(element);
                var mainPanel = $("#" + value.maindiv);
                if (show) {
                    panelWidth = value.width;
                } else {
                    panelWidth = defaultClosedWidth;
                }
                var animations = 0;
                var animationCount = 2;
                var animationDone = function () {
                    animations++;
                    if (value.resizing != undefined && animations === animationCount) {
                        value.resizing(false);
                    }
                };
                if (value.side === "left") {
                    sidebar.animate({ width: panelWidth }, animationDone);
                    mainPanel.animate({ left: panelWidth }, animationDone);
                }
                else if (value.side === 'right') {
                    sidebar.animate({ width: panelWidth }, animationDone);
                    mainPanel.animate({ right: panelWidth }, animationDone);
                }
            }
        };

        ko.bindingHandlers.resizeableColumns = {
            init: function init(element, valueAccessor) {
                var value = ko.utils.unwrapObservable(valueAccessor());

                var liveDrag = value != undefined && value.liveDrag != null ? value.liveDrag : false;
                $(element).colResizable({ liveDrag: liveDrag });
            },

            update: function update(element, valueAccessor) {
                var value = ko.utils.unwrapObservable(valueAccessor());
                if (value == undefined || value.resizeTable == undefined) return;

                // refresh table size
                var resizing = ko.utils.unwrapObservable(value.resizeTable);

                if (resizing) {
                    $(element).colResizable({ disable: true });
                }

                if (!resizing) {
                    $(element).colResizable({ refresh: true });
                }
            }
        };

        ko.bindingHandlers.resizeSlider = {

            init: function init(element, valueAccessor) {

                var value = ko.utils.unwrapObservable(valueAccessor());

                $(element).mousedown(function (e) {
                    var sideWidth = parseInt($("#" + value.additionalWidth).css("width"));
                    e.preventDefault();
                    $(document).mousemove(function (e) {
                        $("#" + value.leftSide).css("width", e.pageX + 2 - sideWidth);
                        $('#' + value.rightSide).css("left", e.pageX + 2 - sideWidth);
                    });
                });

                $(document).mouseup(function () {
                    $(document).unbind("mousemove");
                });
            }
        };

        ko.bindingHandlers.treeBranch = {
            update: function update(element, valueAccessor) {
                var show = ko.utils.unwrapObservable(valueAccessor());
                var children = $(element).parent("li.parent_li").find(" > ul > li");

                if (show) {
                    children.show(400);
                } else {
                    children.hide(400);
                }
            }
        };

        ko.bindingHandlers.rightClickMenu = {
            init: function init(element, valueAccessor) {
                var menu = $(element);
                var item = valueAccessor();

                $("body").on("contextmenu", "#" + item.bindItem, function (e) {
                    menu.css({
                        display: "block",
                        left: e.pageX,
                        top: e.pageY
                    });
                    return false;
                });

                $("#" + item.closeButton).on("click", function () {
                    menu.hide();
                });

                $(document).on("click", function (event) {
                    if (!$(event.target).closest("#" + menu.attr("id")).length) {
                        menu.hide();
                    }
                });
            }
        };

        /*  extenders */

        ko.extenders.async = function (computedDeferred, initialValue) {

            var plainObservable = ko.observable(initialValue), currentDeferred;
            plainObservable.inProgress = ko.observable(false);

            ko.computed(function () {
                if (currentDeferred) {
                    currentDeferred.reject();
                    currentDeferred = null;
                }

                var newDeferred = computedDeferred();
                if (newDeferred &&
                    (typeof newDeferred.done == "function")) {

                    // It's a deferred
                    plainObservable.inProgress(true);

                    // Create our own wrapper so we can reject
                    currentDeferred = $.Deferred().done(function (data) {
                        plainObservable.inProgress(false);
                        plainObservable(data);
                    });
                    newDeferred.done(currentDeferred.resolve);
                } else {
                    // A real value, so just publish it immediately
                    plainObservable(newDeferred);
                }
            });

            return plainObservable;
        };
    }
);