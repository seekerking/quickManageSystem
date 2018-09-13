
(function ($, servicebll) {
    if (!servicebll) {
        window.servicebll = {};
        servicebll = window.servicebll;
    }

    var initData = function(options) {

        var normalUrls = {
            "Get": options.apiUrls.Get,
            "Update": options.apiUrls.Update,
            "Create": options.apiUrls.Create,
            "Delete": options.apiUrls.Delete
        };

        var result = {
            apiUrls: $.extend(true,normalUrls,options.apiUrls),
            pageData: {
                tableData: options.tableData instanceof Array ? options.tableData.concat() : [],
                tableIndex: options.pageData.tableIndex,
                tableSize: options.pageData.tableSize,
                totalSize: options.pageData.totalSize
            }

        };
        return result;

    };

    servicebll.loadData = function(options) {

        options.tableColumn = options.tableColumn ? options.tableColumn : [];
        options.createModal = options.createModal ? options.createModal : $("#modal-Create");
        options.editModal= options.editModal ? options.editModal : $("#modal-Update");
        options.searchFilter = options.searchFilter
            ? options.searchFilter
            : {
                searchFormId: "searchFilter",
                resetButton: "searchReset",
                searchButton: "search"
            };

        var defaultOptions = {
            callBack: {
                Update: function(data) { return data; },
                Delete: function(data) { return data; },
                Create: function (data) { return {}; },
                ControlCallBack: null,
                ModalValidate: function () { return true },
                ModalInitValidateRule: function(data) { return data; },
                ModalInitDomRule: function(data) { return data; },
                ModalDataRule: function(data) { return data; },
                ModalSubmitDataRule: function(data) { return data; }
            },
            postCallBack: function(data) {
                window.WindowData = initData(options.windowData);
                servicebll.loadPage(defaultOptions);
            }

        };

        defaultOptions = $.extend(true, {},defaultOptions, options);


        window.WindowData = window.WindowData ? window.WindowData : initData(options.windowData);
        servicebll.searchFilter(defaultOptions);
        servicebll.loadPage(defaultOptions);
    };

    servicebll.loadPage = function (options) {
        $.submitAjaxData(WindowData.apiUrls.Get,
            { "index": WindowData.pageData.tableIndex, "filter": servicebll.getSearchFilterData(options.searchFilter.searchFormId), "size": WindowData.pageData.tableSize },
            "POST",
            function (resultData) {
                var tableData = resultData.data;
                var pagerData = {
                    total: resultData.total,
                    size: resultData.pagesize,
                    index: resultData.pageindex,
                };
                WindowData.pageData.tableIndex = resultData.pageindex;
                WindowData.pageData.tableSize = resultData.pagesize;
                WindowData.pageData.totalSize = resultData.total;

                $.createPagerTbody({
                    "tableColumn": options.tableColumn,
                    "tableData": tableData,
                    "pagerData":pagerData,
                    "tableId": "#myTable",
                    "pagerId": "#pager",
                    canEdit: options.canEdit,
                    canCreate: options.canCreate,
                    canDelete: options.canDelete,
                    apiUrls: WindowData.apiUrls,
                    modals: { createModal: options.createModal, editModal: options.editModal },
                    callBack: options.callBack,
                    postCallBack: options.postCallBack
                });
                servicebll.registerPager(options);
            });
    }

    servicebll.searchFilter = function (options) {
        var defaultOptions = {
            searchFormId: "searchFilter",
            resetButton: "searchReset",
            searchButton: "search"
        };
        defaultOptions = $.extend(false, defaultOptions, options);

        var form = $("#" + defaultOptions.searchFormId);
        if (form.length === 0) return;
        var searchButton = form.find("input[name='" + defaultOptions.searchButton + "']");
        var resetButton = form.find("input[name='" + defaultOptions.resetButton + "']");
        if (searchButton.length !== 0) {
            searchButton.on("click",
                function (event) {
                    servicebll.loadPage(defaultOptions);
                });
        }
        if (resetButton.length !== 0) {
            resetButton.on("click",
                function (event) {
                    form[0].reset();
                    servicebll.loadPage(defaultOptions);
                });
        }


    };
    servicebll.getSearchFilterData = function (formId) {
        var form = $("#" + formId);
        if (form.length === 0) return {};
        return $.getFormJson(form);
    }


    servicebll.registerPager = function(options) {
        $.registePageClick(function(clickObj) {
            var tabindex = parseInt($(clickObj[0]).attr("tabindex"), 10);
            //-999 首页 -998 前一页 -997 后一页 -996 尾页
            switch (tabindex) {
            case -999:
                WindowData.pageData.tableIndex = 1;
                break;
            case -998:
                WindowData.pageData.tableIndex += -1;
                if (WindowData.pageData.tableIndex < 1) {
                    WindowData.pageData.tableIndex = 1;
                }
                break;
            case -997:
                WindowData.pageData.tableIndex += 1;
                var totalIndex = parseInt(WindowData.pageData.totalSize / WindowData.pageData.tableSize, 10) +
                    (WindowData.pageData.totalSize % WindowData.pageData.tableSize == 0 ? 0 : 1);
                if (WindowData.pageData.tableIndex > totalIndex) {
                    WindowData.pageData.tableIndex = totalIndex;
                }
                break;
            case -996:
                WindowData.pageData.tableIndex = WindowData.pageData.totalSize;
                break;
            default:
                WindowData.pageData.tableIndex = tabindex;
                break;
            }
            servicebll.loadData(options);
            return;
        });
    };

    servicebll.createSelectList = function(selectKey, dataList) {
        var selectDom = $(selectKey);
        var optionList = [];
        dataList.forEach(function(data) {
            var option = $("<option value=" + data.value + ">" + data.text + "</option>");
            optionList.push(option);
        });
        selectDom.append(optionList);
    };

})(jQuery,window.servicebll)