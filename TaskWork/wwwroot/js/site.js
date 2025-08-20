// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.wait = function (title) {
    const loadText = document.getElementById("loadtextid");
    const loading = document.getElementById("loading");

    if (!loading || !loadText) return;

    if (title && title.trim() !== "") {
        loadText.textContent = title;
    }
    loading.style.display = "flex";
};

window.hideLoader = function () {
    const loading = document.getElementById("loading");
    if (loading) loading.style.display = "none";
};
postjson = function (n, t, i, r, u) {

    return $.ajax({

        url: n,

        type: "POST",

        contentType: "application/json; charset=utf-8",

        dataType: "json",

        success: i,

        complete: u,

        error: r,

        data: JSON.stringify(t)

    })

};

getjson = function (n, t, i, r) {

    return $.ajax({

        url: n,

        type: "GET",

        contentType: "application/json; charset=utf-8",

        dataType: "json",

        success: t,

        complete: r,

        error: i

    })

};

deletejson = function (n, t, i, r) {

    return $.ajax({

        url: n,

        type: "DELETE",

        contentType: "application/json; charset=utf-8",

        dataType: "json",

        success: t,

        complete: r,

        error: i

    })

};

gethtml = function (n, t, i, r) {

    return $.ajax({

        url: n,

        type: "GET",

        success: t,

        complete: r,

        error: i

    })

};

postdata = function (n, t, i, r, u) {

    return $.ajax({

        url: n,

        type: "POST",

        contentType: "application/json; charset=utf-8",

        dataType: "json",

        success: r,

        complete: i,

        error: u,

        data: JSON.stringify(t)

    })

};
