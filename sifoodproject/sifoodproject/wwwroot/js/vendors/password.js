// Password js

var password = document.getElementById('fakePassword');
var toggler = document.getElementById('passwordToggler');

showHidePassword = () => {
  if (password.type == 'password') {
    password.setAttribute('type', 'text');
    toggler.classList.add('bi-eye');
    toggler.classList.remove('bi-eye-slash');
  } else {
    toggler.classList.remove('bi-eye')
    toggler.classList.add('bi-eye-slash');
    password.setAttribute('type', 'password');
  }
};

toggler.addEventListener('click', showHidePassword);