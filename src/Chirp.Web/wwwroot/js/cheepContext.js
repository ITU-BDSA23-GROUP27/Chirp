// Heart button toggle
var buttons = document.querySelectorAll('.likes-btn');

buttons.forEach(function(button) {
    button.addEventListener('click', function() {
        var likesBtn = this.querySelector('i.fa-heart');
        likesBtn.classList.toggle('fa-solid');
        likesBtn.classList.toggle('red');
    });
});