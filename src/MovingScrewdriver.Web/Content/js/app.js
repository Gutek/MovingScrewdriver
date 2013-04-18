(function (window, $, prettyPrint, undefined) {

    'use strict';

    var $pre = $('pre');
    if ($pre.length) {
        $pre.addClass('prettyprint').addClass('linenums');
        prettyPrint();
    }

})(window, jQuery, prettyPrint)