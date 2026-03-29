
(function () {
    var path = window.location.pathname.toLowerCase();
    document.querySelectorAll('.nav-pill').forEach(function (link) {
        var href = link.getAttribute('href') || '';
        if (href !== '/' && path.startsWith(href.toLowerCase())) {
            link.classList.add('active-pill');
        } else if (href === '/' && path === '/') {
            link.classList.add('active-pill');
        }
    });
})();
