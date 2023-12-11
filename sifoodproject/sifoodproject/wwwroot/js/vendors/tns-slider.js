

// Tiny Slider js

  // Modal product
  if ($('.productModal').length > 0) {
    var slider = tns({
      "container": "#productModal",
      "items": 1,
      "startIndex": 0,
      "navContainer": "#productModalThumbnails",
      "navAsThumbnails": true,
      "autoplay": false,
      "autoplayTimeout": 1500,
      "swipeAngle": false,
      "speed": 1500,
      "controls": false,
      "autoplayButtonOutput": false,
      "loop": false


    });


  }


  // slider for product
  if ($('.product').length > 1) {
    var slider = tns({
      "container": "#product",
      "items": 1,
      "startIndex": 0,
      "navContainer": "#productThumbnails",
      "navAsThumbnails": true,
      "autoplay": false,
      "autoplayTimeout": 1500,
      "swipeAngle": false,
      "speed": 1500,
      "controls": false,
      "autoplayButtonOutput": false,

    });


  }
