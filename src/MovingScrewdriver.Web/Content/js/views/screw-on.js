(function (window, $, undefined) {

    'use strict';

    var $form = $('form'),
        $submit = $('#submit');

    $submit.click(function (evt) {
        var isValid, url, post;

        evt.preventDefault();
        $submit.button('loading');

        isValid = $form.parsley('validate');

        if (!isValid) {
            $submit.button('reset');
            return;
        }

        url = $form.attr('action');

        post = $.post(url, $form.serialize());

        post.done(function (data) {
            alert(data.message);
        });

        post.fail(function (xhr, textStatus, errorThrown) {
            alert(textStatus);
        });

        post.always(function () {
            $submit.button('reset');
        });


    });

})(window, jQuery)