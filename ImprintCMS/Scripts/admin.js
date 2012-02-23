$(function () {

	$(':input:first').focus();

	$('.datepicker').datepicker({
		dateFormat: 'dd.mm.yy',
		firstDay: 1
	});

});