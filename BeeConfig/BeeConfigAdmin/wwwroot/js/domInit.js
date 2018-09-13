(function($,common) {

    if (!common) {
        window.domCommon = {};
        common = window.domCommon;
    }


    //Update,Create,Delete操作后的提示框
    common.postBackNotify = null;
    //异步操作时的加载提示框
    common.ajaxLoadingNotify = null;

    common.deleteModal = function() {
        var deleteModalHtml = '<div class="modal fade" id="modal-Delete" style="display: none;">' +
            '<div class="modal-dialog">' +
            '<div class="modal-content col-sm-10">' +
            '<div class="modal-header">' +
            '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
            '<span aria-hidden="true">×</span>' +
            '</button>' +
            '<h4 class="modal-title">警告</h4> ' +
            '</div>' +
            '<div class="modal-footer">' +
            '<button type="button" class="btn btn-default" data-dismiss="modal">取消</button>' +
            '<button type="submit" class="btn btn-primary">删除</button><' +
            '/div>' +
            '</div>' +
            '</div>' +
            '</div>';
        var modal = $(deleteModalHtml);
        $(window.body).append(modal);
        return modal;
    }();


    common.notify = function (textcontent, options,backdrop) {

        var needbackdrop = typeof(backdrop)==="undefined"?false:backdrop;
        var defaultContent = {
            title: '提示',
            message: '成功'
        }
        var defaultOptions = {
            type: 'success',
            allow_dismiss: true,
            showProgressbar: true,
            delay: 2000,
            placement: {
                from: 'bottom',
                align: 'right'
            }
        }

        function initbackdrop() {
            var backdrop = $('<div class="modal-backdrop fade in"></div>');
            $("body").append(backdrop);
            return backdrop;

        }

        defaultContent = $.extend(defaultContent, textcontent);
        defaultOptions = $.extend(defaultOptions, options);

        var notify = $.notify(defaultContent, defaultOptions);
        var wappernotify = {
            notify: notify, close: function() {
                this.notify.close();
                if (this.backdrop) {
                    this.backdrop.remove();
                }
                
            } };
        if (needbackdrop) {
            wappernotify.backdrop = initbackdrop();

        }
        return wappernotify;

    };




    common.ajaxLoading = function() {
            $.ajaxSetup({
                beforeSend: function() {

                    if (common.ajaxLoadingNotify != null) {
                        common.ajaxLoadingNotify.close();
                    }
                    common.ajaxLoadingNotify = common.notify({
                            title: "Loading....",
                            message: "加载数据中，请耐心等待"
                        },
                        {
                            type: "info",
                            allow_dismiss: false,
                            showProgressbar: false,
                            z_index: 9999,
                            delay: 0,
                            placement: {
                                align: 'center'
                            }
                        },true);
        }
        });
    }();


})(jQuery,window.domCommon)