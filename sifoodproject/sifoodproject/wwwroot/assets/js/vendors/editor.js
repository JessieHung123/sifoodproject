
  // Quill editor

  if ($("#editor").length) {
    var quill = new Quill('#editor', {
      modules: {
        toolbar: [
          [{ header: [1, 2, false] }],
          [{ 'font': [] }],
          ['bold', 'italic', 'underline', 'strike'],
          [{ 'size': ['small', false, 'large', 'huge'] }],
          [{ 'list': 'ordered'}, { 'list': 'bullet' }],
          [{ 'color': [] }, { 'background': [] }, { 'align': [] }],
          ['link', 'image', 'code-block', 'video'],
        ]
      },
       theme: 'snow'
    });
  }
