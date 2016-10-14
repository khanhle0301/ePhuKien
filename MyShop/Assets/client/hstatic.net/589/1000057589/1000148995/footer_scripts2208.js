//responsive menu
$("#menu-toggle").click(function(e) {
	e.preventDefault();
	var $showNav = $('html, body, #menu-wrapper, #wrapper, #header, #top-search-trigger, #top-cart');
	$showNav.toggleClass("toggled");
});
//end responsive menu 

// sidebar menu
$('.sidebar_menu > li > i').click(function() {
	//$(this).closest('li').find('ul').toggleClass('toggled');
	$(this).closest('li').find('ul').toggle(500);
});
// end sidebar menu


// top link toggle
$(window).load(function() {
	if($(window).width() <= 991) {
		$(document).click(function() {
			$('.top-links').hide();
		});
		$('#top_link_trigger').click(function(e) {
			e.preventDefault();
			e.stopPropagation();
			$('.top-links').toggle();
		});
	}
});
// end top link toggle

// change state of collapse arrow
$('.filter_group a').click(function() {
	$(this).find('i').toggleClass('icon-angle-down');
	$(this).find('i').toggleClass('icon-angle-right');
});
// end change state of collapse arrow

// mark the chosen color
$('.color_block').click(function() {
	$(this).parent().toggleClass('bordercolor');
});
// end mark the chosen color 

// scroll
//http://stackoverflow.com/questions/7717527/jquery-smooth-scrolling-when-clicking-an-anchor-link
$('.pagination li a').click(function() {scrollToShop(0)});

function scrollToShop(margin) {
	var locate = parseInt($('#content').offset().top) + margin;
	$('html, body').animate({
		scrollTop: locate
	},1000);
	return false;
}
// end scroll


/*** top search ***/
$(window).load(function() {
	$(this).scroll(function() {
		if($('#header').hasClass('sticky-header')) {
			$('.top_search').addClass('top_search_sticker');
		}
		else {
			$('.top_search').removeClass('top_search_sticker');
		}
	});
});

$('.top_search form a').click(function(e) {
	e.preventDefault();
	setSearchStorage('.top_search form');
	searchCollection();
});

$('.top_search input').keypress(function(e) {
	if(e.which == 13) {
		e.preventDefault();
		setSearchStorage('.top_search form');
		searchCollection();
	}
});

function searchCollection() {
    var collectionId = $('.top_search .collection').val();
    var searchVal = $('.top_search input[type="text"]').val();
    var url = '';
    if (collectionId == 0) {
        url = '/tim-kiem.html?type=product&keyword=' + searchVal;
    }
    else {
        if (searchVal != '') {
            url = '/tim-kiem.html?type=product&keyword=' + searchVal + '&filter=' + collectionId;
        }
        else {
            url = '/tim-kiem.html?type=product&keyword=filter=' + collectionId;
        }
    }
    window.location = url;
}



function setSearchStorage(form_id) {
	var seach_input = $(form_id).find('.search_input').val();
	var search_collection = $(form_id).find('.search_collection').val();
	sessionStorage.setItem('search_input', seach_input);
	sessionStorage.setItem('search_collection', search_collection);
}
function getSearchStorage(form_id) {
	var search_input_st = ''; // sessionStorage.getItem('search_input');
	var search_collection_st = ''; // sessionStorage.getItem('search_collection');
	if(sessionStorage.search_input != '') {
		search_input_st = sessionStorage.search_input;
	}
	if(sessionStorage.search_collection != '') {
		search_collection_st = sessionStorage.search_collection;
	}
	$(form_id).find('.search_input').val(search_input_st);
	$(form_id).find('.search_collection option[value="'+search_collection_st+'"]').prop('selected', true);
}
function resetSearchStorage() {
	sessionStorage.removeItem('search_input');
	sessionStorage.removeItem('search_collection');
}
$(window).load(function() {
	getSearchStorage('.top_search form');
	resetSearchStorage();
});