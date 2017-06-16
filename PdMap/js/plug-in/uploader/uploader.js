define(function (require, exports, moudles) {
    require("./plupload/plupload.js");
    $(function () {
        return (function ($) {

            $.fn.initUpload = function (options) {
                return new UploadAttachment(this, options);
            };

            var UploadAttachment = function (el, options) {

                var _this = this;

                var _source = { Status: true, Data: [] };

                _this.setting =
                {
                    url: lj.AjaxRequestUrl + "/Att.aspx",//上传地址
                    param: {
                        pid: lj.Platform,
                        method: "Upload",
                        uid: user.Id,

                    },//参数 以post 形式提交服务端
                    accept: "*",// 文件选择类型  图片： image/*
                    maxFileSize: 10485760,//最大文件大小 10M
                    $current: el,//当前辅助$对象
                    $drop_element: null,//拖拽上传区域
                    multiple: false,//多附件上传
                    extension: ["doc", "docx", "pdf", "txt", "ppt", "pptx", "jpg", "jpeg", "png", "gif", "bmp", "zip"],//容许的扩展名
                    filesAdded: function (files) { },//添加文件回调 返回false 则不提交
                    submitBefore: function (files) { },//提交之前  如果返回false  提交停止
                    fileUploaded: function (source, file) { },//当队列中的某一个文件上传完成后触发
                    uploadComplete: function (files) { },//当上传队列中所有文件都上传完成后触发
                    error: function (source) { },//上传失败回调
                    progress: function (file) { },//上传进度回调
                    autoUploader: true,//是否自动上传，即选中文件变更后自动上传
                };

                $.extend(options.param, _this.setting.param);

                $.extend(_this.setting, options);

                //plupload 组件对象
                _this.uploader = null;

                //初始化
                _this.init = function () {
                    if (!_this.setting.param) {
                        _this.setting.param = {};
                    }

                    _this.initUploader();
                };

                //初始化Uploader组件
                _this.initUploader = function () {

                    _this.uploader = new plupload.Uploader({
                        url: _this.setting.url,
                        browse_button: _this.setting.$current[0],//触发文件选择对话框的DOM元素，当点击该元素后便后弹出文件选择对话框。该值可以是DOM元素对象本身，也可以是该DOM元素的id
                        filters: {
                            prevent_duplicates: true, //不允许选取重复文件
                            /*
                            mime_types: [
                                { title: "允许的扩展名", extensions: _this.setting.extension.join() }
                            ],//：用来限定上传文件的类型，为一个数组，该数组的每个元素又是一个对象，该对象有title和extensions两个属性，title为该过滤器的名称，extensions为文件扩展名，有多个时用逗号隔开。该属性默认为一个空数组，即不做限制。
                            max_file_size: "10mb",//用来限定上传文件的大小，如果文件体积超过了该值，则不能被选取。值可以为一个数字，单位为b,也可以是一个字符串，由数字和单位组成，如'200kb'
                            */
                        },
                        multi_selection: _this.setting.multiple,//是否可以在文件浏览对话框中选择多个文件，true为可以，false为不可以。默认true，即可以选择多个文件。需要注意的是，在某些不支持多选文件的环境中，默认值是false。比如在ios7的safari浏览器中，由于存在bug，造成不能多选文件。当然，在html4上传方式中，也是无法多选文件的。
                        drop_element: _this.setting.$drop_element ? _this.setting.$drop_element[0] : null,//指定了使用拖拽方式来选择上传文件时的拖拽区域，即可以把文件拖拽到这个区域的方式来选择文件。该参数的值可以为一个DOM元素的id,也可是DOM元素本身，还可以是一个包括多个DOM元素的数组。如果不设置该参数则拖拽上传功能不可用。目前只有html5上传方式才支持拖拽上传。
                        max_retries: 0,//当发生plupload.HTTP_ERROR错误时的重试次数，为0时表示不重试,
                        multipart_params: _this.setting.param,//上传时的附加参数，以键/值对的形式传入，服务器端可是使用$_POST来获取这些参数(以php为例)。比如：
                    });
                    _this.uploader.init();

                    _this.bindpluploadEvent();
                };

                //绑定plupload 事件
                _this.bindpluploadEvent = function () {

                    //上传之前
                    _this.uploader.bind("BeforeUpload", function (uploader, files, header) {
                        if ($.isFunction(_this.setting.submitBefore)) {
                            if (_this.setting.submitBefore(files) == false) {
                                return false;
                            }
                        }
                    });

                    //当上传队列中某一个文件开始上传后触发
                    _this.uploader.bind("UploadFile", function (uploader, file) {

                    });

                    //当上传队列发生变化后触发，即上传队列新增了文件或移除了文件。QueueChanged事件会比FilesAdded或FilesRemoved事件先触发
                    _this.uploader.bind("UploadFile", function (uploader) {

                    });

                    //当文件从上传队列移除后触发
                    _this.uploader.bind("FilesRemoved", function (uploader, files) {

                    });

                    //该事件会在每一个文件被添加到上传队列前触发
                    _this.uploader.bind("FileFiltered", function (uploader, files) {

                    });

                    //当文件添加到上传队列后触发
                    _this.uploader.bind("FilesAdded", function (uploader, files) {
                        if (_this.checkFileSize(files)) {
                            if (_this.checkFileExtension(files)) {
                                if ($.isFunction(_this.setting.filesAdded)) {
                                    if (_this.setting.filesAdded(files) == false) {
                                        return false;
                                    }
                                }
                                if (_this.setting.autoUploader) {
                                    _this.start();
                                }
                            }
                        }
                    });

                    /*
                        当使用文件小片上传功能时，每一个小片上传完成后触发
                        uploader为当前的plupload实例对象，file为触发此事件的文件对象，responseObject为服务器返回的信息对象，它有以下5个属性：
                        offset：该文件小片在整体文件中的偏移量
                        response：服务器返回的文本
                        responseHeaders：服务器返回的头信息
                        status：服务器返回的http状态码，比如200
                        total：该文件(指的是被切割成了许多文件小片的那个文件)的总大小，单位为字节
                    */
                    _this.uploader.bind("ChunkUploaded", function (uploader, files, responseObject) {

                    });

                    //上传进度
                    _this.uploader.bind("UploadProgress", function (uploader, file) {
                        if ($.isFunction(_this.setting.progress)) {
                            _this.setting.progress(file);
                        }
                    });

                    /*
                        当队列中的某一个文件上传完成后触发
                        responseObject:
                        response：服务器返回的文本
                        responseHeaders：服务器返回的头信息
                        status：服务器返回的http状态码，比如200
                    */
                    _this.uploader.bind("FileUploaded", function (uploader, file, responseObject) {
                        var source = JSON.parse(responseObject.response);
                        if (source.Status) {
                            _source.Data.push(source.Data[0]);
                        }
                        if ($.isFunction(_this.setting.fileUploaded)) {
                            _this.setting.fileUploaded(source, file);
                        }
                    });

                    /*
                        当上传队列中所有文件都上传完成后触发
                    */
                    _this.uploader.bind("UploadComplete", function (uploader, files) {
                        if ($.isFunction(_this.setting.uploadComplete)) {
                            _this.setting.uploadComplete(_source.Data);
                            _source = { Status: true, Data: [] };
                        }
                    });

                    /*
                        当发生触发时触发
                        uploader为当前的plupload实例对象，errObject为错误对象，它至少包含以下3个属性(因为不同类型的错误，属性可能会不同)：
                        code：错误代码，具体请参考plupload上定义的表示错误代码的常量属性
                        file：与该错误相关的文件对象
                        message：错误信息
                    */
                    _this.uploader.bind("Error", function (uploader, error) {
                        if ($.isFunction(_this.setting.error)) {
                            _this.setting.error(error);
                        }
                        if (error.code == -602)//有重复文件
                        {
                            alert(error.file.name + "已上传");
                        }
                        else if (error.code == -200) {//跨域请求无正确响应
                            alert("上传异常(跨域请求未正确响应)" + error.message);
                            console.debug(error);
                        }
                        else {
                            alert(error.message);
                            console.log(error, 3);
                        }
                    });

                    //当调用destroy方法时触发
                    _this.uploader.bind("Destroy", function (uploader) {

                    });

                }

                //检查文件大小
                _this.checkFileSize = function (files) {
                    if (files && files.length > 0) {
                        for (var i = 0; i < files.length; i++) {
                            var fileInfo = files[i];
                            if (fileInfo.size >= _this.setting.maxFileSize) {
                                alert("文件大小不能大于" + fileLengthFormat(_this.setting.maxFileSize) + "：" + fileInfo.name + "(" + fileLengthFormat(fileInfo.size) + ")");
                                return false;
                            }
                        }
                        return true;
                    }
                    else {
                        return false;
                    }
                };

                //检查文件扩展名
                _this.checkFileExtension = function (files) {
                    if (files && files.length > 0) {
                        if (_this.setting.extension.length === 0) {
                            return true;
                        }
                        for (var i = 0; i < files.length; i++) {
                            var name = files[i].name;
                            var ext = /\.[^\.]+$/.exec(name);
                            if (ext.length > 0) {
                                var extension = ext[0];
                                if (extension.startsWith(".")) {
                                    extension = extension.substring(1, extension.length);
                                }
                                if (_this.setting.extension.indexOf(extension.toLowerCase()) == -1) {
                                    alert("不容许的扩展名：" + ext[0], 2);
                                    return false;
                                }
                            }
                            else {
                                alert("文件无扩展名", 2);
                                return false;
                            }
                        }
                        return true;
                    }
                    else {
                        return false;
                    }
                };

                //销毁组件
                _this.destroy = function () {
                    if (_this.uploader) {
                        _this.uploader.destroy();
                    }
                }

                //开始上传
                _this.start = function () {
                    if (_this.uploader) {
                        _this.uploader.start();
                    }
                };

                //停止上传队列
                _this.stop = function () {
                    if (_this.uploader) {
                        _this.uploader.stop();
                    }
                };

                //图片预览
                _this.previewImage = function (file, callback) {//file为plupload事件监听函数参数中的file对象,callback为预览图片准备完成的回调函数
                    if (!file || !/image\//.test(file.type)) return; //确保文件是图片
                    if (file.type == 'image/gif') {//gif使用FileReader进行预览,因为mOxie.Image只支持jpg和png
                        var fr = new mOxie.FileReader();
                        fr.onload = function () {
                            callback(fr.result);
                            fr.destroy();
                            fr = null;
                        }
                        fr.readAsDataURL(file.getSource());
                    } else {
                        var preloader = new mOxie.Image();
                        preloader.onload = function () {
                            preloader.downsize(300, 300);//先压缩一下要预览的图片,宽300，高300
                            var imgsrc = preloader.type == 'image/jpeg' ? preloader.getAsDataURL('image/jpeg', 80) : preloader.getAsDataURL(); //得到图片src,实质为一个base64编码的数据
                            callback && callback(imgsrc); //callback传入的参数为预览图片的url
                            preloader.destroy();
                            preloader = null;
                        };
                        preloader.load(file.getSource());
                    }
                }

                _this.init();
            };

        })(jQuery);
    })
})