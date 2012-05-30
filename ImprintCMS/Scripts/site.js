$(function () {
    $('#content.home.search #searchform #query').focus();

    $('#shopcounter').each(function () {
        var element = $(this);
        var url = element.data('shop-count-url');
        $.get(url, function (data) {
            if (data.shopCount > 0) element.append(' <span class="count">(' + data.shopCount + ')</span>');
        });
    });

    $('a.imagedownload.withnotice').each(function () {
        var link = $(this);
        var accept = $('<a href="' + link.attr('href') + '">' + link.data('acceptlabel') + '</a>');
        var cancel = $('<a href="#">' + link.data('cancellabel') + '</a>');
        var notice = $('<span class="notice"><span class="message">' + link.data('notice') + '</span></span>');
        notice.append('<br/>');
        notice.append(accept);
        notice.append(' | ');
        notice.append(cancel);
        cancel.click(function (e) {
            e.preventDefault();
            notice.hide();
            link.show(200);
        });
        notice.hide();
        notice.insertAfter(link);
    });

    $('a.imagedownload.withnotice').click(function (e) {
        e.preventDefault();
        $(this).hide().next('.notice').show(200);
    });

});