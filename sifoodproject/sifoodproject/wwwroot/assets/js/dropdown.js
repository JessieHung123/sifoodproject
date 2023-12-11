document.addEventListener("DOMContentLoaded", function() {
    var selectBoxs = document.querySelectorAll(".selectBox");
    Array.from(selectBoxs).forEach(function (selectBox) {
        var selectBoxValue = selectBox.querySelector('.selectValue');
        var selectBoxUL = selectBox.querySelector('ul');
        Array.prototype.forEach.call(
            selectBoxUL.querySelectorAll('li'),
            function(element) {
                element.onclick = function(){
                    Array.prototype.forEach.call(
                        selectBoxUL.querySelectorAll('.dropdown-item'),
                            function(item) {
                                item.classList.remove('active')
                            }
                    );
                    element = this.querySelector('a')
                    element.classList.add('active')
                    selectBoxValue.innerHTML = element.innerHTML;
                };
            }
        );
    })
});
