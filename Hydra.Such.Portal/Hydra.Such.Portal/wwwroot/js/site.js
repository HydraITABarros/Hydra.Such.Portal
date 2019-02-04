// Write your JavaScript code.
/* menu bar resizable*/
$(function () {
    startMenubarResizable();
    openSelectedMenu();

    window.addEventListener('WebComponentsReady', function (e) {
        //return;
        initGridLists();
    });
});


var startMenubarResizable = function () {
    var _this = this;
    var minWidth = 220;
    var maxWidth = 780;
    var currentMousePos = { x: -1, y: -1 };
    var isDragging = false;
    var wasDragging = false;
    var $menu = $('#menubar.menubar-resizable');
    var $navBarHeader = $('.navbar-header');
    var $appMain = $('.navbar-container, #app-main');

    var getWidthFromMousePosition = function () {
        return currentMousePos.x <= minWidth ? minWidth : currentMousePos.x >= maxWidth ? maxWidth : currentMousePos.x;
    };

    var setWidth = function (width) {
        if ($(window).width() < 1200 || $('body').hasClass('menubar-fold')) {
            $menu.css('width', '');
            $navBarHeader.css('width', '');
            $appMain.css({
                marginLeft: ''
            });
        } else {
            $menu.width(width);
            $navBarHeader.width(width);
            $appMain.css({
                marginLeft: width
            });

            if (typeof (Storage) !== "undefined") {
                localStorage['menu.width'] = width;
            }
        }
    }

    if (typeof localStorage != 'undefined' && localStorage['menu.width']) {
        setWidth((localStorage['menu.width'] * 1));
    }

    $('.js-hamburger').on('click', function () {
        setTimeout(function () {
            if (typeof localStorage != 'undefined' && localStorage['menu.width']) {
                return setWidth((localStorage['menu.width'] * 1));
            }
            return setWidth($menu.width());
        }, 60);
    });

    $(window).resize(function () {
        if (typeof localStorage != 'undefined' && localStorage['menu.width']) {
            return setWidth((localStorage['menu.width'] * 1));
        }
        return setWidth($menu.width());
    });

    $menu.find('.after').mousedown(function (e) {
        isDragging = true;
    }).mousemove(function (event) {
    }).mouseup(function () {
        wasDragging = isDragging;
        isDragging = false;
    });

    $(document).mouseup(function (e) {
        wasDragging = isDragging;
        isDragging = false;
        if (wasDragging) {
            e.preventDefault();
            e.stopPropagation();
            setWidth(getWidthFromMousePosition());
        }
    }).mousemove(function (e) {
        currentMousePos.x = e.pageX;
        if (isDragging) {
            e.preventDefault();
            e.stopPropagation();
            setWidth(getWidthFromMousePosition());
        }
    });
};

var openSelectedMenu = function () {
    return;

    var selectedMenu = $($("#menubar .selected")[0]);
    $("#menubar .selected").not(":first").removeClass("selected");
    if (selectedMenu.length > 0) {
        selectedMenu.parents('ul.submenu').css({ display: 'block' }).parents('li.has-submenu').addClass('open');

        var scrollMaxTimeout = false;
        setTimeout(function () {
            scrollMaxTimeout = true;
        }, 5000);

        (function tryScrollTo() {
            if (window.app.menubar == null || window.app.menubar.$scrollInner == null) {
                setTimeout(function () {
                    tryScrollTo();
                }, 300);
                return;
            }
            if (scrollMaxTimeout) {
                return;
            }
            var menuLinks = $("#menubar a");
            window.app.menubar.$scrollInner.slimScroll({ scrollTo: ((selectedMenu.index()) * 37) + "px" });
        })()

    }
};

var initGridLists = function () {
    var gridLists = $('[_permissions]')[0].root.querySelectorAll('[column-reordering-allowed]');
    gridLists.forEach(function (grid) {
        window.testes = grid;
        setGridListsColumnDragable(grid, grid.__dataHost);
    });
};

var setGridListsColumnDragable = function (grid, name) {
    var columnTree = grid._getColumnTree()[0];

    if (typeof localStorage !== 'undefined') {
        var localStorageName = 'grid-column-order-' + name;
        if (localStorage.getItem(localStorageName) !== null) {
            var columnOrder = JSON.parse(localStorage.getItem(localStorageName));
            if (columnOrder) {
                columnTree.forEach(function (column, index) {
                    column._order = columnOrder[index];
                });
            }
        }

        grid.ondragend = function () {
            var columnOrderUpdated = [];
            columnTree.forEach(function (column, index) {
                columnOrderUpdated[index] = column._order;
            });
            localStorage.setItem(localStorageName, JSON.stringify(columnOrderUpdated));
        };
    }
};