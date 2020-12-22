jQuery(function () {
	$("form").submit(function () {
		// submit more than once returns false
		$(this).submit(function () {
			return false;
		});
		// submit once returns true
		return true;
	});
});


// show navbar when scrolling up
$(document).ready(function () {
	var previousScroll = 0;
	$(window).scroll(function () {
		var currentScroll = $(this).scrollTop();
		if (currentScroll < 100) {
			showNav();
		} else if (currentScroll > 0 && currentScroll < $(document).height() - $(window).height()) {
			if (currentScroll > previousScroll) {
				hideNav();
			} else {
				showNav();
			}
			previousScroll = currentScroll;
		}
	});

	function hideNav() {
		$(".navbar").removeClass("is-visible").addClass("is-hidden");
	}

	function showNav() {
		$(".navbar").removeClass("is-hidden").addClass("is-visible").addClass("scrolling");
	}
});