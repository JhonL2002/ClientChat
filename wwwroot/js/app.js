window.navBarInterop = {
    toggleNavbar: function () {
        var navBar = document.querySelector(".navbar-layout");
        navBar.classList.toggle("chat-active");
    },
    closeNavbar: function () {
        var navBar = document.querySelector(".navbar-layout");
        if (navBar.classList.contains("chat-active")) {
            navBar.classList.remove("chat-active");
        }
    }
};

