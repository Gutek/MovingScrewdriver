(function (window, $, undefined) {

    'use strict';

    var $form = $('form'),
        $submit = $('#submit'),
        $comments = $('#form');

    $submit.click(function (evt) {
        var isValid, url, post, i, j, curr, len, jLen, jCurr;

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
            
            if (data.error && data.error.length) {

                len = data.error.length;
                for (i = 0; i < len; i++) {
                    curr = data.error[i];
                    jLen = curr.errors.length;
                    
                    for (j = 0; j < jLen; j++) {
                        jCurr = curr.errors[j];
                        
                        $('#' + curr.key).parsley('addError', { mycustomerror: jCurr });
                    }
                }
                
                $.pnotify({
                    title: 'Błąd Walidacji',
                    text: 'Popraw blędy na formularzu',
                    type: 'error'
                });

            } else {
                $comments.before(data);
                $form[0].reset();
                $.pnotify('Komentarz dodany');
            }
        });

        post.fail(function (xhr, textStatus, errorThrown) {
            // alert(textStatus);
        });

        post.always(function () {
            $submit.button('reset');
        });


    });

})(window, jQuery)