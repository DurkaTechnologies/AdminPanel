$(document).ready(function () {
	$('.form-image').click(function () { $('#customFile').trigger('click'); });

	$(function () {
		$('.selectpicker').selectpicker();
	});

	$("center").remove();

	setTimeout(function () {
		$('body').addClass('loaded');
	}, 200);

	//phone number mask
	$('.ukrainephone').mask("(999) 999-9999");

	jQueryModalGet = (url, title) => {
		try {
			$.ajax({
				type: 'GET',
				url: url,
				contentType: false,
				processData: false,
				success: function (res) {
					$('#form-modal .modal-body').html(res.html);
					$('#form-modal .modal-title').html(title);
					$('#form-modal').modal('show');
					console.log(res);
				},
				error: function (err) {
					console.log(err)
				}
			})
			//to prevent default form submit event
			return false;
		} catch (ex) {
			console.log(ex)
		}
	}

	jQueryModalPost = form => {
		try {
			$.ajax({
				type: 'POST',
				url: form.action,
				data: new FormData(form),
				contentType: false,
				processData: false,
				success: function (res) {
					if (res.isValid) {
						$('#viewAll').html(res.html)
						$('#form-modal').modal('hide');
					}
				},
				error: function (err) {
					console.log(err)
				}
			})
			return false;
		} catch (ex) {
			console.log(ex)
		}
	}

	jQueryModalDelete = form => {
		if (confirm('Ви впевнені що хочете видалити це ?')) {
			try {
				$.ajax({
					type: 'POST',
					url: form.action,
					data: new FormData(form),
					contentType: false,
					processData: false,
					success: function (res) {
						if (res.isValid) {
							$('#viewAll').html(res.html)
						}
					},
					error: function (err) {
						console.log(err)
					}
				})
			} catch (ex) {
				console.log(ex)
			}
		}

		//prevent default form submit event
		return false;
	}

	jQueryModalDeactivate = form => {
		if (confirm('Ви впевнені що хочете деактивувати це ?')) {
			try {
				$.ajax({
					type: 'POST',
					url: form.action,
					data: new FormData(form),
					contentType: false,
					processData: false,
					success: function (res) {
						if (res.isValid) {
							$('#viewAll').html(res.html)
						}
					},
					error: function (err) {
						console.log(err)
					}
				})
			} catch (ex) {
				console.log(ex)
			}
		}

		//prevent default form submit event
		return false;
	}
});