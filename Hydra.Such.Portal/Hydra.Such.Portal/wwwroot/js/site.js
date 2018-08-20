// Write your JavaScript code.
/* menu bar resizable*/
$(function () {

    (function menubar_resizable() {
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


                console.log(86876876);

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

    })();

});