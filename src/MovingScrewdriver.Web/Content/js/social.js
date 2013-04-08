
/**
 * facebook-jssdk - FB Like Box & FB Like Button
 * twitter-wjs - Twitter timeline & Twitter Tweet Button
 */
(function (d, s, ids) {
    var js, fjs = d.getElementsByTagName(s)[0], i = 0, elem;

    for (i = 0; i < ids.length; i++) {
        elem = ids[i];
        
        if (d.getElementById(elem.id)) {
            continue;
        }

        js = d.createElement(s);
        js.id = elem.id;
        js.src = elem.url;
        fjs.parentNode.insertBefore(js, fjs);
    }
    
}(document, 'script', [ {
    id: 'facebook-jssdk',
    url: '//connect.facebook.net/pl_PL/all.js#xfbml=1'
    }, {
        id: 'twitter-wjs',
        url: '//platform.twitter.com/widgets.js'
    }]
));

/**
 * Google +1 Button
 */
window.___gcfg = { lang: 'pl' };

(function () {
    var po = document.createElement('script');
    po.type = 'text/javascript';
    po.async = true;
    po.src = 'https://apis.google.com/js/plusone.js';
    var s = document.getElementsByTagName('script')[0];
    s.parentNode.insertBefore(po, s);
})();