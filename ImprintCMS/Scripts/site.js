$(function () {
	$('#content.home.search #searchform #query').focus();

	$('#shopcounter').each(function () {
		var element = $(this);
		var url = element.data('shop-count-url');
		$.get(url, function (data) {
			if (data.shopCount > 0) element.append(' <span class="count">(' + data.shopCount + ')</span>');
		});
	});
});