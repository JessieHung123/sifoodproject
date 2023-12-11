
// Slick Slider

'use strict';

(function () {

// category slider

if ($('.category-slider').length) {
    $('.category-slider').slick({
      infinite: true,
      slidesToShow: 6,
      slidesToScroll: 1,
      autoplay: true,
      dots: false,
      arrows: true,
       prevArrow: '<span class="slick-prev "><i class="feather-icon icon-chevron-left"></i></span>',
      nextArrow: '<span class="slick-next "><i class="feather-icon icon-chevron-right "></i></span>',
      responsive: [{
          breakpoint: 1400,
          settings: {
            slidesToShow: 4,
            slidesToScroll: 4,

          }
        },
        {
          breakpoint: 820,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 1
          }
        },
        {
          breakpoint: 480,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 1
          }
        }
        // You can unslick at a given breakpoint now by adding:
        // settings: "unslick"
        // instead of a settings object
      ]

    });

  }



  // product slider

  if ($('.product-slider').length) {
    $('.product-slider').slick({
      infinite: true,
      slidesToShow: 5,
      slidesToScroll: 1,
      autoplay: true,
      dots: false,
      arrows: true,
      prevArrow: '<span class="slick-prev "><i class="feather-icon icon-chevron-left"></i></span>',
      nextArrow: '<span class="slick-next "><i class="feather-icon icon-chevron-right "></i></span>',
      responsive: [{
          breakpoint: 1400,
          settings: {
            slidesToShow: 4,
            slidesToScroll: 4,

          }
        },
        {
          breakpoint: 820,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 1
          }
        },
        {
          breakpoint: 480,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,

          }
        }
        // You can unslick at a given breakpoint now by adding:
        // settings: "unslick"
        // instead of a settings object
      ]

    });

  }



  // team slider

  if ($('.team-slider').length) {
    $('.team-slider').slick({
      infinite: true,
      slidesToShow: 5,
      slidesToScroll: 1,
      autoplay: true,
      dots: false,
      arrows: true,
       prevArrow: '<span class="slick-prev"><i class="feather-icon icon-chevron-left"></i></span>',
      nextArrow: '<span class="slick-next "><i class="feather-icon icon-chevron-right "></i></span>',
      responsive: [{
          breakpoint: 1024,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 3

          }
        },
        {
          breakpoint: 820,
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
        // You can unslick at a given breakpoint now by adding:
        // settings: "unslick"
        // instead of a settings object
      ]

    });

  }


  // hero slider

  if ($('.hero-slider').length) {
    $('.hero-slider').slick({
      infinite: true,
      slidesToShow: 1,
      slidesToScroll: 1,
      autoplay: true,
      dots: true,
      arrows: false,


    });

  }

  // slider 8 columns
  if ($('.slider-8-columns').length) {
  $(".slider-8-columns").each(function (key, item) {
    var id = $(this).attr("id");
    var sliderID = "#" + id;
    var appendArrowsClassName = "#" + id + "-arrows";

    $(sliderID).slick({
      infinite: true,
      slidesToShow: 8,
      slidesToScroll: 1,
      autoplay: true,
      dots: false,
      arrows: true,

        speed: 1000,
        arrows: true,

        loop: true,
        adaptiveHeight: true,
        responsive: [
            {
                breakpoint: 1025,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            }
        ],
        prevArrow:'<span class="slick-prev "><i class="feather-icon icon-chevron-left"></i></span>',
      nextArrow: '<span class="slick-next "><i class="feather-icon icon-chevron-right "></i></span>',
        appendArrows: appendArrowsClassName
    });
});
  }

      // product slider
      if ($('.product-slider-second').length) {
        $(".product-slider-second").each(function (key, item) {
          var id = $(this).attr("id");
          var sliderID = "#" + id;
          var appendArrowsClassName = "#" + id + "-arrows";

          $(sliderID).slick({
            infinite: true,
            slidesToShow: 5,
            slidesToScroll: 1,
            autoplay: true,
            dots: false,
            arrows: true,


              speed: 1000,
              arrows: true,

              loop: true,
              adaptiveHeight: true,
              responsive: [{
                breakpoint: 1400,
                settings: {
                  slidesToShow: 4,
                  slidesToScroll: 4,

                }
              },
              {
                breakpoint: 990,
                settings: {
                  slidesToShow: 3,
                  slidesToScroll: 1
                }
              },
              {
                breakpoint: 480,
                settings: {
                  slidesToShow: 1,
                  slidesToScroll: 1,

                }
              }],
              prevArrow:'<span class="slick-prev "><i class="feather-icon icon-chevron-left"></i></span>',
            nextArrow: '<span class="slick-next"><i class="feather-icon icon-chevron-right "></i></span>',
              appendArrows: appendArrowsClassName
          });
      });
        }
        if ($('.slider-for').length) {
        $('.slider-for').slick({
          slidesToShow: 1,
          slidesToScroll: 1,
          arrows: false,
          fade: true,
          asNavFor: '.slider-nav'
        });
        $('.slider-nav').slick({
          slidesToShow: 4,
          slidesToScroll: 1,
          asNavFor: '.slider-for',
          dots: false,

          centerMode: false,
          focusOnSelect: true,
          prevArrow:'<span class="slick-prev "><i class="feather-icon icon-chevron-left"></i></span>',
            nextArrow: '<span class="slick-next "><i class="feather-icon icon-chevron-right "></i></span>'
        });
      }
    })();



      // product slider

  if ($('.product-slider-four-column').length) {
    $('.product-slider-four-column').slick({
      infinite: true,
      slidesToShow: 4,
      slidesToScroll: 1,
      autoplay: true,

      dots: false,
      arrows: true,
      prevArrow: '<span class="slick-prev "><i class="feather-icon icon-chevron-left"></i></span>',
      nextArrow: '<span class="slick-next "><i class="feather-icon icon-chevron-right "></i></span>',
      responsive: [{
          breakpoint: 1400,
          settings: {
            slidesToShow: 4,
            slidesToScroll: 4,

          }
        },

        {
          breakpoint: 1200,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 1
          }
        },
        {
          breakpoint: 480,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,

          }
        }
        // You can unslick at a given breakpoint now by adding:
        // settings: "unslick"
        // instead of a settings object
      ]

    });

  }
