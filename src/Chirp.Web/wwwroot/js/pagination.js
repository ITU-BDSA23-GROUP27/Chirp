// hotkeys to move between pages
// ArrowLeft - previous page
// ArrowRight - next page

document.addEventListener('keydown', function (event) {
    if (event.key === 'ArrowLeft') {
        if (currentPage > 1) {
            window.location.href = '?page=' + (currentPage - 1);
        }
    } else if (event.key === 'ArrowRight') {
        if (currentPage < totalPageCount) {
            window.location.href = '?page=' + (currentPage + 1);
        }
    }
});