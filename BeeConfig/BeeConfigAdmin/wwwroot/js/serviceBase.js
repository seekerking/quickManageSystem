
(function ($, common) {
    $.extend({
        /*        
         * pagerData：包含分页信息
         * pagerData:{
         *     total:100,
         *     size:10,
         *     index:1,
         *  }
         */
        serviceCreatePaging: function(pagerData) {
            var pageHtmls = [];

            if (pagerData &&
                pagerData.total &&
                pagerData.total > 0 &&
                pagerData.index &&
                pagerData.index > 0 &&
                pagerData.size &&
                pagerData.size > 0
            ) {
                var totalIndex = pagerData.total / pagerData.size;
                if (pagerData.total % pagerData.size != 0) {
                    totalIndex += 1;
                }
                pageHtmls.push('<ul class="pagination justify-content-center">');
                pageHtmls.push('<li class="page-item">');
                pageHtmls.push('<a class="page-link" href="#" tabindex="-999">首页</a>');
                pageHtmls.push('</li>');
                pageHtmls.push('<li class="page-item">');
                pageHtmls.push('<a class="page-link" href="#" tabindex="-998">前一页</a>');
                pageHtmls.push('</li>');

                var pageIndex = [
                    pagerData.index - 2, pagerData.index - 1, pagerData.index, pagerData.index + 1, pagerData.index + 2
                ];
                if (pageIndex[0] < 1) {
                    var offset = 1 - pageIndex[0];
                    for (var i = 0; i < pageIndex.length; i++) {
                        pageIndex[i] += offset;
                    }
                }
                for (var j = 0; j < pageIndex.length; j++) {
                    if (pageIndex[j] <= totalIndex) {
                        pageHtmls.push('<li class="page-item ' +
                            (pagerData.index == pageIndex[j] ? "active" : "") +
                            '">');
                        pageHtmls.push('<a class="page-link" href="#" tabindex="' +
                            pageIndex[j] +
                            '">' +
                            pageIndex[j] +
                            '</a>');
                        pageHtmls.push('</li>');
                    }
                }

                pageHtmls.push('<li class="page-item">');
                pageHtmls.push('<a class="page-link" href="#" tabindex="-997">后一页</a>');
                pageHtmls.push('</li>');
                pageHtmls.push('<li class="page-item">');
                pageHtmls.push('<a class="page-link" href="#" tabindex="' + totalIndex + '">尾页</a>');
                pageHtmls.push('</li>');
                pageHtmls.push('</ul>');
            }
            return pageHtmls.join('');
        },
        /**
         *   * @param {tableColumn, tableData, pagerData, tableId, pagerId,canCreate,canDelete,callBack} options 传入参数
         * 创建业务逻辑展示table，返回table html
         * tableColumn, tableData, pagerData, tableId, pagerId
         * tableData:包含table头信息 以及数据信息        
         * tableData:[           
         *  { row1Data,rows2Data}
         * ]            
         */

        createPagerTbody: function(options) {

            var defaultOption = {
                canEdit: true,
                canCreate: true,
                canDelete: true,
                tableColumn: [],
                modals: { createModal: $("#modal-Create"), editModal: $("#modal-Update"), deleteModal: common.deleteModal },
                apiUrls: {
                    "Create": "",
                    "Update": "",
                    "Delete": ""
                },
                postCallBack: function (data) { },
                callBack: {
                    Update: function (data) { return data; },
                    Delete: function (data) { return data; },
                    Create: function (data) { return {}; },
                    ControlCallBack: null,
                    ModalValidate: function () {return true},
                    ModalInitValidateRule: function (data) { return data; },
                    ModalInitDomRule: function (data) { return data; },
                    ModalDataRule: function (data) { return data; },
                    ModalSubmitDataRule: function (data) { return data; }
                }

            };

            function processCallBack(event) {
                var modal = event.data.modal;
                if (event.data) {
                    var data = event.data.data;

                    if (event.data.callback) {
                        data=event.data.callback(event.data.data);
                    }
                    var form = modal.find("form");
                    if (form.length !== 0) {
                        form[0].reset();
                    }
                    
                    //todo： 此处需要加validater
                    if (defaultOption.callBack.ModalInitValidateRule) {
                        defaultOption.callBack.ModalInitValidateRule(data);
                    }
                    //此处绑定界面事件
                    if (defaultOption.callBack.ModalInitDomRule) {
                        defaultOption.callBack.ModalInitDomRule(data);
                    }
                    //在加载数据之前对数据做相应的变动
                    if (defaultOption.callBack.ModalDataRule) {
                        data = defaultOption.callBack.ModalDataRule(data);
                    }
                    $.formEdit(modal.find("form"), data);
                }
                if (event.data && event.data.modal) {

                    modal.modal('show');
                    modal.find(":submit").off("click").on("click",
                        function() {
                            var postData = $.getFormJson(modal.find("form"));
                            postData = $.extend(true, event.data.data, postData);
                            //在提交之前对数据做变动

                            if (!options.callBack.ModalValidate(modal.find("form"))) {
                                return false;
                            }

                            if (options.callBack.ModalSubmitDataRule) {
                                postData = options.callBack.ModalSubmitDataRule(postData);
                            }
                            modal.modal("hide");
                            $.submitAjaxData(event.data.url,
                                postData,
                                "POST",
                                function (data) {
                                    common.postBackNotify = common.notify({ message: "操作成功" });
                                    defaultOption.postCallBack(data);
                                });
                        });
                    return false;

                }
            };

            defaultOption = $.extend(true, {},defaultOption, options);
            var theaderbody = null;
            if (defaultOption.tableColumn && defaultOption.tableColumn.length > 0) {
                theaderbody = $("<thead>");
                var header = $("<tr>");
                theaderbody.append(header);
                for (var k = 0; k < defaultOption.tableColumn.length; k++) {
                    var title = defaultOption.tableColumn[k].headerTemplate
                        ? defaultOption.tableColumn[k].headerTemplate(defaultOption.tableColumn[k].title)
                        : defaultOption.tableColumn[k].title;
                    header.append("<td>" + title + "</td>");
                }
                if (defaultOption.canEdit || defaultOption.Delete || defaultOption.canCreate||defaultOption.callBack.ControlCallBack) {
                    if (defaultOption.canCreate) {
                        var container = $("<td>");
                        var warper = $('<div class="content-wapper"></div>');
                        container.append(warper);
                        var createButton =
                            $('<button type="button" ' +
                                'class="btn btn-success" data-toggle="modal" ' +
                                'data-target="#modal-Create">新增</button>');
                         warper.append(createButton);
                        if (defaultOption.callBack.Create) {
                            createButton.on("click",
                                {
                                    callback: defaultOption.callBack.Create,
                                    data: {},
                                    url: defaultOption.apiUrls.Create,
                                    modal: defaultOption.modals.createModal
                                },
                                processCallBack);
                        }
                        header.append(container);
                    } else {
                        header.append("<td>操作</td>");
                    }

                 
                }
            }
            var tbody = $("<tbody>");
            for (var i = 0; i < defaultOption.tableData.length; i++) {

                var row = $("<tr>");

                var rowItem = defaultOption.tableData[i];
                for (var j = 0; j < defaultOption.tableColumn.length; j++) {

                    var value = rowItem[defaultOption.tableColumn[j].name];
                    value=defaultOption.tableColumn[j].valueTemplate
                        ? defaultOption.tableColumn[j].valueTemplate(value)
                        : value;
                    row.append("<td>" +value + "</td>");
                }
                var operationGroup = $('<td class="btn-group">');
                if (defaultOption.canEdit) {

                    var editOperation = $('<button class="btn btn-info">编辑</button>');
                    if (defaultOption.callBack.Update) {
                        editOperation.on("click",
                            {
                                callback: defaultOption.callBack.Update,
                                data: defaultOption.tableData[i],
                                url: defaultOption.apiUrls.Update,
                                modal: defaultOption.modals.editModal
                            },
                            processCallBack);
                    }

                    operationGroup.append(editOperation);
                }


                if (defaultOption.callBack.ControlCallBack) {
                    var customOperations = defaultOption.callBack.ControlCallBack(rowItem, options, processCallBack);
                    operationGroup.append(customOperations);
                }
                if (defaultOption.canDelete) {
                    var deleteOperation = $('<button class="btn btn-danger">删除</button>');
                    if (defaultOption.callBack.Delete) {
                        deleteOperation.on("click",
                            {
                                callback: defaultOption.callBack.Delete,
                                data: defaultOption.tableData[i],
                                url: defaultOption.apiUrls.Delete,
                                modal:  defaultOption.modals.deleteModal
                            },
                            processCallBack);
                    }
                    operationGroup.append(deleteOperation);
                }
                if (defaultOption.canEdit || defaultOption.Delete) {
                    row.append(operationGroup);
                }
                tbody.append(row);
            }
            $(defaultOption.tableId).empty().append(theaderbody).append(tbody);
            $(defaultOption.pagerId).html($.serviceCreatePaging(defaultOption.pagerData));
        },


        /**
         * 提交数据到远程服务器
         * 
         */
        submitAjaxData: function(apiUrl, paramData, methodType, successFunc) {
            $.ajax({
                url: apiUrl,
                data: paramData,
                method: methodType,
                dataType: "json",
                error: function (xmlhttpReq, errorInfo) {
                    if (common.ajaxLoadingNotify != null) {
                        common.ajaxLoadingNotify.close();
                    }
                    alert("ajax请求发送错误:" + errorInfo);
                },
                success: function(returnObject) {
                    //返回结构必然为{status:0|1,msg:"",data:object}
                    if (common.ajaxLoadingNotify != null) {
                        common.ajaxLoadingNotify.close();
                    }
                    //如果有之前的提示框，立即删除掉,如果操作比较快的话可能会有多个结果提示框
                    //if (common.postBackNotify == null) {
                    //} else {
                    //    common.postBackNotify.close();
                    //}
                    if (returnObject && returnObject.status == 0) {
                        successFunc(returnObject);
                    } else if (returnObject && returnObject.status == 2) {
                        alert(returnObject.data.errorMessage);
                        return;
                    }

                    else {
                        alert("ajax请求成功,但是响应错误:" + returnObject.msg);
                    }
                }
            });
        },
        /**
         * 序列化表单数据
         * @param {} form 
         * @returns {} 
         */

        getFormJson: function(form) {
            var o = {};
            var a = form.serializeArray();
            $.each(a,
                function() {
                    if (o[this.name] !== undefined) {
                        if (!o[this.name].push) {
                            o[this.name] = [o[this.name]];
                        }
                        o[this.name].push(this.value || '');
                    } else {
                        o[this.name] = this.value || '';
                    }
                });
            return o;
        },
        /**
         * 将数据填充到form中
         * @param {} data 
         * @returns {} 
         */
        formEdit: function(form, data) {
            var formData = form;
            formData.each(function() {
                var input, name;
                if (data == null || data instanceof Array) {
                    this.reset();
                    return;
                }
                for (var i = 0; i < this.length; i++) {
                    input = this.elements[i];
                    name = (input.type == "checkbox") ? input.name.replace(/(.+)\[\]$/, "$1") : input.name;
                    if (typeof data[name] == 'undefined') continue;
                    switch (input.type) {
                    case "checkbox":
                        if (data[name] === "") {
                            input.checked = false;
                        } else {
                            input.checked = (data[name].indexOf(input.value) > -1) ? true : false;
                        }
                        break;
                    case "radio":
                        if (data[name] === "") {
                            input.checked = false;
                        } else if (input.value == data[name]) {
                            input.checked = true;
                        }
                    case "file":
                        $('img[src=' + name + ']').attr('src', data[name]);
                        break;
                    default:
                        input.value = data[name];
                    }
                }
            });
        },


        /**
         * 创建一个提示框
         */
        alertWarnDialog: function(message) {
            var dialogId = "";
        },

        /**
         * 注册分页组件点击事件
         */
        registePageClick: function(action) {
            $("a.page-link").on("click",
                function() {
                    action($(this));
                });
        }

    });

})(jQuery, window.domCommon)