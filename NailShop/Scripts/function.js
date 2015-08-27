

function showImageBrowser(container, name, multiple) {
    multiple = multiple || false;
    var windowContent = getImageBrowserTemplate(true);
    var win = MRTcreateDialogImageBrowser(windowContent, container, name, multiple);
}

function getImageBrowserTemplate(showBrowser) {
    var messages = {
        emptyFolder: "Empty Folder",
        uploadFile: "Upload",
        orderBy: "Arrange by:",
        orderBySize: "Size",
        orderByName: "Name",
        invalidFileType: "The selected file \"{0}\" is not valid. Supported file types are {1}.",
        deleteFile: 'Are you sure you want to delete "{0}"?',
        overwriteFile: 'A file with name "{0}" already exists in the current directory. Do you want to overwrite it?',
        directoryNotFound: "A directory with this name was not found.",
        imageWebAddress: "Web address",
        imageAltText: "Alternate text",
        linkWebAddress: "Web address",
        linkText: "Text",
        linkToolTip: "ToolTip",
        linkOpenInNewWindow: "Open link in new window",
        dialogInsert: "Insert",
        dialogButtonSeparator: "or",
        dialogCancel: "Cancel"
    };

    return kendo.template(
            '<div class="k-editor-dialog">' +
                '# if (showBrowser) { #' +
                    '<div class="k-imagebrowser"></div>' +
                '# } #' +
                '<div style="padding:0 !important" class="k-button-wrapper">' +
                    '<button class="k-dialog-insert k-button">#: messages.dialogInsert #</button>' +
                    '&nbsp;#: messages.dialogButtonSeparator #&nbsp;' +
                    '<a href="\\#" class="k-dialog-close k-link">#: messages.dialogCancel #</a>' +
                '</div>' +
            '</div>'
        )({
            messages: messages,
            showBrowser: showBrowser
        });
}

function MRTcreateDialogImageBrowser(windowContent, container, name, multiple) {
    var KEDITORIMAGEURL = "#k-editor-image-title";
    var win = $(windowContent).appendTo(document.body).kendoWindow({
        title: "Insert image",
        modal: true,
        resizable: false,
        draggable: true,
        animation: false,
        visible: false,
        width: "960px",
        height: "550px",
        activate: function () {
            var that = this;

            that.element.find(".k-imagebrowser").kendoImageBrowser({
                transport: {
                    read: {
                        url: MRT_ReadImageURL
                    },
                    destroy: {
                        url: MRT_DestroyImageURL,
                        type: "POST"
                    },
                    create: {
                        url: MRT_CreateImageURL,
                        type: "POST"
                    },
                    thumbnailUrl: MRT_ThumbnailImageURL,
                    uploadUrl: MRT_UploadImageURL,
                    imageUrl: MRT_GetImageImageURL + "?path={0}"
                },
                change: function () {
                    that.element.find(KEDITORIMAGEURL).val(this.value());
                },
                apply: function () {
                    var imageBrowser = this;
                    var value = imageBrowser.value();
                    var listView = imageBrowser.listView;
                    var datasource = imageBrowser.dataSource;
                    var selected = listView.select();
                    if (selected.length) {
                        var dataItem = datasource.getByUid(selected.attr(kendo.attr("uid")));
                        var imagename = dataItem.get(imageBrowser._getFieldName("name"));

                        var thumb = imageBrowser.options.transport.thumbnailUrl + "?path=" + imageBrowser.path() + encodeURIComponent(imagename);
                        if (multiple)
                        {
                            name = name + "[]";
                        }
                        var img_tmpl = MRTimageTemplate(name, thumb, value, imagename);
                        if (multiple) {
                            $(container).append(img_tmpl);
                        } else {
                            $(container).html(img_tmpl);
                        }
                        $("a[rel=lightbox]", $(container)).lightBox();
                        //$(container).sortable();
                    }

                    win.destroy();
                }
            })
        }
    }).find(".k-dialog-close").click(function (e) {
        e.preventDefault(); win.destroy();
    }).end().find(".k-dialog-insert").click(function () {
        var imageBrowser = win.element.find(".k-imagebrowser").data("kendoImageBrowser");
        var value = imageBrowser.value();
        var listView = imageBrowser.listView;
        var datasource = imageBrowser.dataSource;
        var selected = listView.select();
        if (selected.length) {
            var dataItem = datasource.getByUid(selected.attr(kendo.attr("uid")));
            var imagename = dataItem.get(imageBrowser._getFieldName("name"));
            var thumb = imageBrowser.options.transport.thumbnailUrl + "?path=" + imageBrowser.path() + encodeURIComponent(imagename);
            if (multiple) {
                name = name + "[]";
            }
            var img_tmpl = MRTimageTemplate(name, thumb, value, imagename);
            if (multiple) {
                $(container).append(img_tmpl);
            } else {
                $(container).html(img_tmpl);
            }
            $("a[rel=lightbox]", $(container)).lightBox();
            //$(container).sortable();
        }

        win.destroy();

    }).end().data("kendoWindow");
    win.center();
    win.open();
    return win;
}

function MRTimageTemplate(name, thumb, image, imagename) {
    return kendo.template(
            '<div class="image_item">' +
                '<a rel="lightbox" href="#: image #"><img src="#: thumb #" /></a>' +
                '<input class="hidden_imgbrowse" type="hidden" name="#: name #" value="#: imagename #"/>' +
                '<a onclick="MRTdeleteImg(this)" class="close_btn_img"><i class="icon-remove"></i></a>' +
            '</div>'
        )({
            name: name,
            image: image,
            thumb: thumb,
            imagename: imagename
        });
}

function MRTdeleteImg(el) {
    $(el).closest("div.image_item").remove();
}

function MRTshowEditorApp(el) {
    var userfolder = "";
    if (typeof ($(el).data("folder")) != "undefined")
    {
        userfolder = "userfolder="+ $(el).data("folder");
    }

    $(el).kendoEditor({
        tools: [
            "bold",
            "italic",
            "underline",
            "strikethrough",
            "fontName",
            "fontSize",
            "foreColor",
            "backColor",
            "justifyLeft",
            "justifyCenter",
            "justifyRight",
            "justifyFull",
            "insertUnorderedList",
            "insertOrderedList",
            "indent",
            "outdent",
            "formatBlock",
            "createLink",
            "unlink",
            "insertImage",
            "viewHtml",
            "createTable",
                "addRowAbove",
                "addRowBelow",
                "addColumnLeft",
                "addColumnRight",
                "deleteRow",
                "deleteColumn"
        ],
        imageBrowser: {
            messages: {
                dropFilesHere: "Kéo và thả file vào đây để tãi lên",
                uploadFile: "Tải lên",
                orderBy: "Lọc bởi",
                orderByName: "Tên",
                orderBySize: "Kích thước",
                directoryNotFound: "Thư mục này không tồn tại.",
                emptyFolder: "Thư mục rỗng",
                deleteFile: 'Bạn chắc chắn muốn xóa "{0}"?',
                invalidFileType: "File vừa chọn \"{0}\" không hợp lê. Các file hỗ trợ là {1}.",
                overwriteFile: "File với tên \"{0}\" luôn luôn tồn tại trong thư mục. Bạn có muốn ghi đè không?",
                search: "Tìm kiếm"
            },
            transport: {
                read: MRT_ReadImageURL + "?" + userfolder,
                destroy:{
                    url: MRT_DestroyImageURL + "?" + userfolder,
                    type: "POST"
                },
                create: { 
                    url: MRT_CreateImageURL + "?" + userfolder,
                    type: "POST"
                },
                thumbnailUrl: MRT_ThumbnailImageURL + "?" + userfolder,
                uploadUrl: MRT_UploadImageURL + "?" + userfolder,
                imageUrl: MRT_GetImageImageURL + "?path={0}" + "&" + userfolder,
            }
        }
    });
}




function loadEditor(el) {
    var userfolder = "";
    if (typeof ($(el).data("folder")) != "undefined") {
        userfolder = "userfolder=" + $(el).data("folder");
    }
    kendo.ui.editor.ColorTool.prototype.options.palette = ["#ffffff", "#000000", "#eeece1", "#1f497d", "#4f81bd", "#c0504d", "#9bbb59", "#8064a2", "#8064a2", "#f79646",
                        "#f2f2f2", "#7f7f7f", "#ddd9c3", "#c6d9f0", "#dbe5f1", "#f2dcdb", "#ebf1dd", "#e5e0ec", "#dbeef3", "#fdeada",
                        "#d8d8d8", "#595959", "#c4bd97", "#8db3e2", "#b8cce4", "#e5b9b7", "#d7e3bc", "#ccc1d9", "#b7dde8", "#fbd5b5",
                        "#bfbfbf", "#3f3f3f", "#938953", "#548dd4", "#95b3d7", "#d99694", "#c3d69b", "#b2a2c7", "#92cddc", "#fac08f",
                        "#a5a5a5", "#262626", "#494429", "#17365d", "#366092", "#953734", "#76923c", "#5f497a", "#31859b", "#e36c09",
                        "#7f7f7f", "#0c0c0c", "#1d1b10", "#0f243e", "#244061", "#632423", "#4f6128", "#3f3151", "#205867", "#974806"];
    $(el).kendoEditor({
        tools: [
            "bold",
            "italic",
            "underline",
            "strikethrough",
            "fontSize",
            "foreColor",
            "backColor",
            "justifyLeft",
            "justifyCenter",
            "justifyRight",
            "justifyFull",
            "insertUnorderedList",
            "insertOrderedList",
            "indent",
            "outdent",
            "formatBlock",
            "createLink",
            "unlink",
            "insertImage",
            "viewHtml",
            "createTable",
                "addRowAbove",
                "addRowBelow",
                "addColumnLeft",
                "addColumnRight",
                "deleteRow",
                "deleteColumn"

            
        ],
        imageBrowser: {
            messages: {
                dropFilesHere: "Kéo và thả file vào đây để tãi lên",
                uploadFile: "Tải lên",
                orderBy: "Lọc bởi",
                orderByName: "Tên",
                orderBySize: "Kích thước",
                directoryNotFound: "Thư mục này không tồn tại.",
                emptyFolder: "Thư mục rỗng",
                deleteFile: 'Bạn chắc chắn muốn xóa "{0}"?',
                invalidFileType: "File vừa chọn \"{0}\" không hợp lê. Các file hỗ trợ là {1}.",
                overwriteFile: "File với tên \"{0}\" luôn luôn tồn tại trong thư mục. Bạn có muốn ghi đè không?",
                search: "Tìm kiếm"
            },
            transport: {
                read: MRT_ReadImageURL + "?" + userfolder,
                destroy: {
                    url: MRT_DestroyImageURL + "?" + userfolder,
                    type: "POST"
                },
                create: {
                    url: MRT_CreateImageURL + "?" + userfolder,
                    type: "POST"
                },
                thumbnailUrl: MRT_ThumbnailImageURL + "?" + userfolder,
                uploadUrl: MRT_UploadImageURL + "?" + userfolder,
                imageUrl: MRT_GetImageImageURL + "?path={0}" + "&" + userfolder,
            }
        }
    });
}



