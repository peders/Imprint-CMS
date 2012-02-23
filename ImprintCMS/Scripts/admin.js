$(function () {

	$(':input:first').focus();

	$('.datepicker').datepicker({
		dateFormat: 'dd.mm.yy',
		firstDay: 1
	});

	$('.listing.sortable li').each(function () {
		$(this).append('<div class="handle">' + $(this).closest('.sortable').data('handlelabel') + '</div>');
	});
	$('.listing.sortable').sortable({
		handle: '.handle',
		placeholder: 'placeholder',
		update: function (event, ui) {
			var listing = $(this);
			$.ajax({
				url: listing.data('persisturl'),
				type: 'POST',
				data: listing.sortable('serialize'),
				dataType: 'text',
				error: function () {
					listing.sortable('cancel');
					alert(listing.data('errorlabel'));
				}
			});
		}
	});

});