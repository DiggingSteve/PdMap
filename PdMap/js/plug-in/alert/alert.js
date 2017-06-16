define(function (require, exports, moudles) {

    require("./alert.css");

    var $body = $(document.body);

    var _options =
        {
            timeout: 3000,
            msg: "已完成",
            type: 1,
            call: null,
        };

    var init = function (options) {

        $.extend(_options, options);

        if ($.isFunction(_options.type)) {
            _options.call = _options.type;
        }
        if ($.isFunction(_options.timeout)) {
            _options.call = _options.timeout;
        }

        if (!$.isNumeric(_options.type)) {
            _options.type = 1;
        }
        if (!_options.timeout || !parseInt(_options.timeout)) {
            _options.timeout = 2000;
        }

        var alert_type;
        switch (_options.type) {
            case 2:
                alert_type = "error";
                break;
            case 3:
                alert_type = "warnning"
                break;
            case 1:
            default:
                alert_type = "success";
                break;
        }

        var html = new StringBuilder();
        html.AppendFormat('<div class="dialog-alert {0}">', alert_type);
        html.Append('<span class="alert-icon"></span>');
        html.AppendFormat('<span class="message">{0}</span>', _options.msg);
        html.Append('<span class="close">×</span>');
        html.Append('</div>');
        var $alert = $(html.toString());
        $body.append($alert);

        $alert.css("margin-left", -(($alert.width() + 35) / 2));
        setTimeout(function () {
            $alert.find(".close").click();
        }, _options.timeout);
        $alert.find(".close").on("click", function () {
            $alert.fadeOut(function () {
                $alert.remove();
                if ($.isFunction(_options.call)) {
                    _options.call();
                }
            })
        });

        return $alert;
    };

    window.alert = function (msg, type) {
        _options.msg = msg;
        _options.type = type;
        init(_options);
    }

});