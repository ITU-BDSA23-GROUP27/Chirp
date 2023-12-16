var seconds = 5;

function countdown() {
    document.getElementById('countdown').innerHTML = seconds;
    if (seconds <= 0) {
        window.location.href = "/";
    } else {
        seconds--;
        setTimeout(countdown, 1000);
    }
}

countdown();