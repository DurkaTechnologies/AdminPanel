function registerSummernote(element, max, callbackMax, h, maxh) {
	$(element).summernote({
		tabsize: 2,
		height: h,
		maxHeight: maxh,
		callbacks: {
			onInit: function () {
				var length = $('#summernote').summernote('code').replace(/<[^>]+>/g, "").length;
				$('#lenghtContentPost').text(max - length);
				$('#maxContentPost').text(`/ ${max} символів`);
			},
			onKeydown: function (e) {
				var t = e.currentTarget.innerText;
				if (t.trim().length >= max) {
					if (e.keyCode != 8 && !(e.keyCode >= 37 && e.keyCode <= 40) && e.keyCode != 46 && !(e.keyCode == 88 && e.ctrlKey) && !(e.keyCode == 67 && e.ctrlKey) && !(e.keyCode == 65 && e.ctrlKey))
						e.preventDefault();
				}
			},
			onKeyup: function (e) {
				var t = e.currentTarget.innerText;
				if (typeof callbackMax == 'function')
					callbackMax(max - t.length);
			},
			onPaste: function (e) {
				var t = e.currentTarget.innerText;
				var bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData).getData('Text');
				e.preventDefault();
				var maxPaste = bufferText.length;

				if (t.length + bufferText.length > max)
					maxPaste = max - t.length - 1;

				if (maxPaste > 0)
					document.execCommand('insertText', false, bufferText.substring(0, maxPaste));

				$('#lenghtContentPost').text(max - t.length);
			}
		}
	});
}
