(function (window, $, undefined) {

    'use strict';

    if (!String.prototype.formatWith) {
        String.prototype.formatWith = function () {
            var str = this;
            for (var i = 0; i < arguments.length; i++) {
                str = str.replace('{' + i + '}', arguments[i]);
            }
            return str;
        };
    }

    var app = window.App = {};

    var alerts = (function () {

        // global alerts placeholder, where alerts will be added
        var $alerts = $('.alerts');
        // function that checks what has been provided and returns defaults
        // for instance if title is bool it probably is what user provided for pernament
        //
        // rules:
        //
        //  - if `title` is undefined, use defaultTitle value
        //  - if `title` is bool, it means it's `permanent` value
        //  - if `permanent` is undefined, use `false` value for it
        var getSettings = function (defaultTitle, message, title, permanent) {
            if (title && !permanent) {
                if (title === true || title === false) {
                    permanent = title;
                    title = undefined;
                } else {
                    permanent = false;
                }
            }

            if (!title) {
                title = defaultTitle;
            }

            return {
                message: message,
                title: title,
                permanent: permanent
            };
        };
        // method constructs an alert element - HTML string based on the options
        // passed by `getSettings`
        //
        // `type` defines a type of the alert to show:
        //
        //  - success
        //  - warning
        //  - error
        //  - info
        var getAlert = function (type, options) {
            // default alert template
            var msg = '<div class="alert alert-block {0} fade in" id="licence-submit-error">' +
                '{1}' +
                '<h4 class="alert-heading">{2}</h4>' +
                '<p>' +
                '{3}' +
                '</p>' +
            '</div>';
            // checks if permanent is true or false, if true do not show X button
            var permanent = function (isPermanent) {
                if (isPermanent) {
                    return '';
                }

                return '<a class="close" data-dismiss="alert" href="#">×</a>';
            };
            // checks for type of alert, if it's `warning` there is no point
            // of adding a class
            var alertType = function (baseType) {
                if (baseType === 'warning') {
                    return '';
                }

                return 'alert-' + baseType;
            };

            msg = msg.formatWith(alertType(type), permanent(options.permanent), options.title, options.message);

            return msg;
        };

        return {
            // access property to alerts functionality
            msg: {
                // show warning alert
                warn: function (message, title, permanent) {
                    var alert = getAlert('warning', getSettings('Warning!', message, title, permanent)),
                        $alert = $(alert);
                    $alerts.append($alert);
                    $alert.alert();
                },
                // show success alert
                success: function (message, title, permanent) {
                    var alert = getAlert('success', getSettings('Success!', message, title, permanent)),
                        $alert = $(alert);
                    $alerts.append($alert);
                    $alert.alert();
                },
                // show error alert
                error: function (message, title, permanent) {
                    var alert = getAlert('error', getSettings('Error!', message, title, permanent)),
                        $alert = $(alert);
                    $alerts.append($alert);
                    $alert.alert();
                },
                // show info alert
                info: function (message, title, pernament) {
                    var alert = getAlert('info', getSettings('Information', message, title, pernament)),
                        $alert = $(alert);
                    $alerts.append($alert);
                    $alert.alert();
                }
            }
        };

    })();

    app.Alerts = alerts;

})(window, jQuery)