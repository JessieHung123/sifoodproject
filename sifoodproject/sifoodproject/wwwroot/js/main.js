//
// Main js
//


'use strict';

(function () {

  // Multi level menu dropdown

  if ($(".dropdown-menu a.dropdown-toggle").length) {
    $(".dropdown-menu a.dropdown-toggle").on("click", function (e) {
      if (!$(this)
        .next()
        .hasClass("show")
      ) {
        $(this)
          .parents(".dropdown-menu")
          .first()
          .find(".show")
          .removeClass("show");
      }
      var $subMenu = $(this).next(".dropdown-menu");
      $subMenu.toggleClass("show");

      $(this)
        .parents("li.nav-item.dropdown.show")
        .on("hidden.bs.dropdown", function (e) {
          $(".dropdown-submenu .show").removeClass("show");
        });

      return false;
    });
  }


  // Default Tooltip

  if ($('[data-bs-toggle="tooltip"]').length) {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
      return new bootstrap.Tooltip(tooltipTriggerEl)
    })
  }


  // Default Popover

  if ($('[data-bs-toggle="popover"]').length) {
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
      return new bootstrap.Popover(popoverTriggerEl)
    })
  }





  // rater

  if ($('#rater').length) {
    var starRating1 = raterJs({
      starSize: 20,
      element: document.querySelector("#rater"),
      rateCallback: function rateCallback(rating, done) {
        this.setRating(rating);
        done();
      }
    });


  }


  // rater
  if ($('#rate-2').length) {
    var starRating1 = raterJs({
      starSize: 20,
      element: document.querySelector("#rate-2"),
      rateCallback: function rateCallback(rating, done) {
        this.setRating(rating);
        done();
      }
    });

  }


  // rater
  if ($('#rate-3').length) {

    var starRating1 = raterJs({
      starSize: 20,
      element: document.querySelector("#rate-3"),
      rateCallback: function rateCallback(rating, done) {
        this.setRating(rating);
        done();
      }
    });

  }



  //rater

  if ($('#rate-4').length) {

    var starRating1 = raterJs({
      starSize: 20,
      element: document.querySelector("#rate-4"),
      rateCallback: function rateCallback(rating, done) {
        this.setRating(rating);
        done();
      }
    });

  }




  // Sidenav fixed for docs

  if ($(".sidebar-nav-fixed a").length) {
    $(".sidebar-nav-fixed a")
      // Remove links that don't actually link to anything
      .on("click", function (event) {
        // On-page links
        if (
          location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') &&
          location.hostname == this.hostname
        ) {
          // Figure out element to scroll to
          var target = $(this.hash);
          target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
          // Does a scroll target exist?
          if (target.length) {
            // Only prevent default if animation is actually gonna happen
            event.preventDefault();
            $('html, body').animate({
              scrollTop: target.offset().top - 90
            }, 1000, function () {
              // Callback after animation
              // Must change focus!
              var $target = $(target);
              $target.focus();
              if ($target.is(":focus")) { // Checking if the target was focused
                return false;
              } else {
                $target.attr('tabindex', '-1'); // Adding tabindex for elements not focusable
                $target.focus(); // Set focus again
              };
            });
          }
        };
        $('.sidebar-nav-fixed a').each(function () {
          $(this).removeClass('active');
        })
        $(this).addClass('active');
      });
  }



  // Flatpickr

  if ($(".flatpickr").length) {
    flatpickr(".flatpickr", {
      disableMobile: true

    });

  }


  //  Stopevent for dropdown

  if ($(".stopevent").length) {
    $(document).on("off.bs.collapse.data-api", ".stopevent", function (e) {
      e.stopPropagation();
    });
  }





  // Check all for  checkbox

  if ($("#checkAll").length) {
    $("#checkAll").on("click", function () {
      $('input:checkbox').not(this).prop('checked', this.checked);
    });
  }

  if ($("#liveAlertPlaceholder").length) {
  const alertPlaceholder = document.getElementById('liveAlertPlaceholder')

    const alert = (message, type) => {
      const wrapper = document.createElement('div')
      wrapper.innerHTML = [
        `<div class="alert alert-${type} alert-dismissible" role="alert">`,
        `   <div>${message}</div>`,
        '   <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
        '</div>'
      ].join('')

      alertPlaceholder.append(wrapper)
    }

    const alertTrigger = document.getElementById('liveAlertBtn')
    if (alertTrigger) {
      alertTrigger.addEventListener('click', () => {
        alert('Nice, you triggered this alert message!', 'success')
      })
    }
  }
  // price range

  if ($('#priceRange').length) {
    var nonLinearStepSlider = document.getElementById('priceRange');
    noUiSlider.create(nonLinearStepSlider, {
      connect: true,
      behaviour: 'tap',
      start: [49, 199],
      range: {
        // Starting at 500, step the value by 500,
        // until 4000 is reached. From there, step by 1000.
        'min': [6],

        'max': [300]
      },
      format: wNumb({
        decimals: 1,
        thousand: '.',
        prefix: '$'
      })


    });
    var nonLinearStepSliderValueElement = document.getElementById('priceRange-value');

    nonLinearStepSlider.noUiSlider.on('update', function (values) {
      nonLinearStepSliderValueElement.innerHTML = values.join(' - ');
    });

  }


  // File Input
  if ($(".file-input").length) {
    $('.file-input').change(function () {
      var curElement = $(this).parent().parent().find('.image');
      console.log(curElement);
      var reader = new FileReader();

      reader.onload = function (e) {
        // get loaded data and render thumbnail.
        curElement.attr('src', e.target.result);
      };

      // read the image file as a data URL.
      reader.readAsDataURL(this.files[0]);
    });

  }
})();