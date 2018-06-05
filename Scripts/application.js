	
		
	// TRIGGER ANIMATIONS
	// http://www.oxygenna.com/tutorials/scroll-animations-using-waypoints-js-animate-css 
	$(window).bind('load resize', function(){
		
		function onScrollInit( items, trigger ) {
		
			items.each( function() {
				var osElement = $(this),
				osAnimationClass = osElement.attr('data-os-animation'),
				osAnimationDelay = osElement.attr('data-os-animation-delay');

				osElement.css({
					'-webkit-animation-delay':  osAnimationDelay,
					'-moz-animation-delay':     osAnimationDelay,
					'-ms-animation-delay':     	osAnimationDelay,
					'animation-delay':          osAnimationDelay
				});

				var osTrigger = ( trigger ) ? trigger : osElement;

				osTrigger.waypoint(function() {
					osElement.addClass('animated').addClass(osAnimationClass);
					$('.slick-slide .os-animation').removeClass('fadeInUp animated');
					$('.slick-active .os-animation').addClass('fadeInUp animated');
				},{
					//triggerOnce: true,
					offset: '90%'
				});
			});
		
		}
		onScrollInit( $('.os-animation') );

		// IOS BROWSER CLASS
		if( navigator.userAgent.match(/iP(hone|od|ad)/i) ) {
			jQuery('body').addClass('browser-ios');
		}
	
	});	
	
	$(document).ready(function() {

	    cookiePolicy();

	    // LAZYSIZES PRELOAD
	    $('img.lazyload').addClass('lazypreload');
		
		// EXPAND MOBILE NAVIVAGTION  
		$("header a.expand").click(function(){
			if ($(".navigation .reveal").length ) { 
				$("header a.expand").toggleClass('active');
				$("html").toggleClass('reveal-out');
			}
			else{
				$("header a.expand").toggleClass('active');
				$("html").toggleClass('reveal-out');
			}
		});
		$(".navigation nav.main ul li span i").click(function(){
			if ($(".navigation nav.main ul li span i").length ) { 
				$(this).parent().parent().toggleClass("open");
				$(this).parent().parent().siblings().removeClass("open");
			}
			else{
				$(this).parent().parent().toggleClass("open");
			}
		});
	    $(".navigation nav.main ul li span.active").parent("li").addClass("open");
		$("html").click(function() {
			$(".navigation nav.main ul li.open").removeClass("open");
		});
		$(".navigation nav.main ul li span i, header a.expand").click(function(e){
			e.stopPropagation(); 
		});
		$("html").click(function() {
			$("header a.expand").removeClass('active');
			$("html.reveal-out").removeClass("reveal-out");
		});
		$(".navigation").click(function(e){
			e.stopPropagation(); 
		});
		
		// BANNER
		// PLAYS VIDEO IN BANNER
		$('.banner .slides').on('init', function(ev, el){
		    $('video').each(function () {
		        this.play();
		    });
		});
		// BANNER ITSELF
      	$(".banner .slides").slick({
			arrows: false,
			dots: true,
			infinite: true,
			speed: 600,
			fade: true,
  			adaptiveHeight: true,
			prevArrow: '<div class="slick-prev"><span class="btn c1-bg c5-text"><i class="ion-chevron-left"></i>',
			nextArrow: '<div class="slick-next"><span class="btn c1-bg c5-text"><i class="ion-chevron-right"></i>',
				responsive: [
				{
					breakpoint: 767,
					settings: {
						arrows: false
					}
				}
			]
		});
		// BANNER INFO IS ANIMATED FOR EACH SLIDE
		$('.banner .slides').on('afterChange', function(event, slick, currentSlide){
		    $('.slick-active .os-animation').removeClass('fadeInUp animated');
		    $('.slick-active .os-animation').addClass('fadeInUp animated');
		});
		$('.banner .slides').on('beforeChange', function(event, slick, currentSlide){
		    $('.slick-active .os-animation').removeClass('fadeInUp animated');
		});	
		
		// IMAGE SLIDESHOW
      	$(".slideshow .slides").slick({
			arrows: true,
			dots: false,
			infinite: true,
			speed: 600,
			fade: false,
  			adaptiveHeight: true,
			prevArrow: '<div class="slick-prev"><span class="btn c1-bg c5-text"><i class="ion-chevron-left"></i>',
			nextArrow: '<div class="slick-next"><span class="btn c1-bg c5-text"><i class="ion-chevron-right"></i>'
		});
		
		// IMAGE CAROUSEL
      	$(".image-carousel .slides").slick({
			arrows: true,
			prevArrow: '<div class="slick-prev"><span class="btn c1-bg c5-text"><i class="ion-chevron-left"></i>',
			nextArrow: '<div class="slick-next"><span class="btn c1-bg c5-text"><i class="ion-chevron-right"></i>',
			dots: true,
			infinite: true,
			speed: 600,
			easing: 'linear',
			slidesToShow: 4,
			slidesToScroll: 4,
				responsive: [
				{
					breakpoint: 1200,
					settings: {
						slidesToShow: 3,
						slidesToScroll: 3
					}
				},
				{
					breakpoint: 768,
					settings: {
						slidesToShow: 2,
						slidesToScroll: 2
					}
				},
				{
					breakpoint: 480,
					settings: {
						slidesToShow: 1,
						slidesToScroll: 1
					}
				}
			]
		});   
		
		// FEATURED BLOG POSTS CAROUSEL
      	$(".featured-blogs .slides").slick({
			arrows: true,
			prevArrow: '<div class="slick-prev"><span class="btn c1-bg c5-text"><i class="ion-chevron-left"></i>',
			nextArrow: '<div class="slick-next"><span class="btn c1-bg c5-text"><i class="ion-chevron-right"></i>',
			dots: false,
			infinite: true,
			speed: 600,
			easing: 'linear',
  			adaptiveHeight: true,
			slidesToShow: 1,
			slidesToScroll: 1
		});  	
		
		// SCROLL PROMPT LINK
		// ADVANCED PAGE
		$('.advanced-page .scroll-prompt').click(function() {
			
  			var target;
  			$("section").not($(this).parent().parent().parent().parent().parent()).each(function(i, element) {

			target = ($(element).offset().top - 0);
			if (target - 10 > $(document).scrollTop()) {
				    return false; // break
				}
			});
			$("html, body").animate({
				scrollTop: target
			}, 600);

		});
		// STANDARD PAGE
		$('.standard-page .scroll-prompt').click(function() {
			
  			var target;
  			$("section").not($(this).parent().parent()).each(function(i, element) {

			target = ($(element).offset().top - 0);
			if (target - 10 > $(document).scrollTop()) {
				    return false; // break
				}
			});
			$("html, body").animate({
				scrollTop: target
			}, 600);

		});
		
		// BACK TO TOP
		if ( ($(window).height() + 100) < $(document).height() ) {
			
	        $('#top-link-block').addClass('show').affix({
	            // how far to scroll down before link "slides" into view
	            offset: {top:140}
	        });
	
		}
		
	});
	
	// MATCH HEIGHTS
	$(window).bind('load resize', function(){
		
		// TEXT WITH VIDEO OR IMAGE
		$(".apc.text-with-image-or-video .image, .apc.text-with-image-or-video .item").matchHeight({
			byRow: true
		});
		
		// LISTINGS
		$(".listing.grid .item .inner").matchHeight({
			byRow: true
		});
		$(".listing.grid .item .heading").matchHeight({
			byRow: true
		});
		
		$(".pricing-menu .item .inner").matchHeight({
			byRow: true
		});
		
		// WIDE COLUMN SITE WIDE POD
		$(".swp-wide .item").matchHeight({
			byRow: true
		});
		$(".swp-wide .item .text").matchHeight({
			byRow: true
		});
		
	});
	
	// LIGTHBOX
	$(document).delegate('*[data-toggle="lightbox"]', 'click', function(event) {
    	event.preventDefault();
        $(this).ekkoLightbox();
	}); 

	// HEADER SCROLLING
	var didScroll;
	var lastScrollTop = 0;
	var delta = 160;
	var navbarHeight = $('header').outerHeight();

	$(window).scroll(function(event){
	    didScroll = true;
	});

	setInterval(function() {
	    if (didScroll) {
	        hasScrolled();
	        didScroll = false;
	    }
	}, 160);

	function hasScrolled() {
	    var st = $(this).scrollTop();
    
	    // Make sure they scroll more than delta
	    if(Math.abs(lastScrollTop - st) <= delta)
	        return;
	
	    if (st > lastScrollTop && st > navbarHeight){
	        // Scroll Down
	        $('html').removeClass('nav-down').addClass('nav-up');
	    } else {
	        // Scroll Up
	        $('html').addClass('nav-down').removeClass('nav-up');
	    }
    
	    lastScrollTop = st;
	}
	
	// FIXED HEADER
    $(window).scroll(function() {
	
        var scroll = $(window).scrollTop();

        if (scroll >= 160) {
            $("html").removeClass("reached-top");
        } else {
            $("html").addClass("reached-top");
        }

    });


	function cookiePolicy() {
        var cookiePanel = $('.cookie-notice'),
	        cookieName = "cookieNotice";

	    checkCookie();

	    $('.accept-cookies').on('click', function (e) {
	        e.preventDefault();
	        setCookie();
	    });

	    // Get cookie
	    function getCookie(c_name) {

	        var i, x, y, ARRcookies = document.cookie.split(";");
	        for (i = 0; i < ARRcookies.length; i++) {
	            x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
	            y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
	            x = x.replace(/^\s+|\s+$/g, "");
	            if (x == c_name) {
	                return unescape(y);
	            }
	        }

	    }

	    // Set cookie
	    function setCookie() {

	        var exdate = new Date();
	        exdate.setDate(exdate.getDate() + exdays);
	        var c_value = "accepted" + ((exdays == null) ? "" : "; path=/; expires=" + exdate.toUTCString());
	        document.cookie = cookieName + " =" + c_value;
	        cookiePanel.addClass("closed");
	        cookiePanel.removeClass("open");
	    }

	    // Check cookie
	    function checkCookie() {

	        var username = getCookie(cookieName);
	        if (username != null && username != "") {
	            cookiePanel.addClass("closed");
	            cookiePanel.removeClass("open");
	        }
	        else {
	            cookiePanel.addClass("open");
	            cookiePanel.removeClass("closed");
	        }

	    }

	};
	

