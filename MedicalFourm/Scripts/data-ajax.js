function initAjax(obj, $select)
{
    if ($select == null)
        $select = function (str) { return $(obj).find(str); }

    // TYPE: select
    // REQUIRED:        href, data-ajax
    //
    // data-ajax:       content container 
    //
    $select("a[data-ajax]").click(function () {
        $this = $(this);
        $.get($this.attr('href'), function (html) {
            var container = $this.data('ajax');
            $(container).html(html);
            initAjax(container);
        });
        return false;
    });

    // TYPE: select
    // REQUIRED,        data-ajax, data-ajax-url
    //
    // data-ajax-url:   url
    // data-ajax:       content container 
    //
    $select("select[data-ajax]").change(function () {
        $this = $(this);
        $.post($this.data('ajax-url'), 'value=' + $this.val(), function (html) {
            var container = $this.data('ajax');
            $(container).html(html);
            initAjax(container);
        });
    });

    // TYPE: form
    // REQUIRED,        data-ajax
    //
    // data-ajax:       content container 
    //
    $select("form[data-ajax]").submit(function () {
        $this = $(this);

        var url = $this.attr('action');
        if (url == null)
            url = window.location.href;
        
        var method = $this.attr('method');
        if (method == null)
            method = 'post';

        var container = $this.data('ajax');

        if (method == 'post') {
            $.post(url, $this.serialize(), function (html) {
                $(container).html(html);
                initAjax(container);
            });
        }
        else // get
        {
            $.get(url + '?' + $this.serialize(), function (html) {
                $(container).html(html);
                initAjax(container);
            });
        }

        return false;
    });
}

// ----------

initAjax(document);