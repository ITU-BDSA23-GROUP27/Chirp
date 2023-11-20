document.addEventListener('keydown', function (event) {
    if (event.altKey) {
        switch (event.key) {
            case '1':
                window.location.href = '/';
                break;
            case '2':
                window.location.href = '/Privacy';
                break;
            case '3':
                window.location.href = '/' + username;
                break;
            case '4':
                window.location.href = '/Profile';
                break;
            case '5':
                window.location.href = '/SeedDB';
                break;
        }
    }
});


// add/remove class on scroll
document.addEventListener("scroll", function() {
    var navbar = document.querySelector(".navbar");
    var scrolled = window.scrollY;

    if (scrolled > 50) {
        navbar.classList.add("scrolled");
    } else {
        navbar.classList.remove("scrolled");
    }
});
