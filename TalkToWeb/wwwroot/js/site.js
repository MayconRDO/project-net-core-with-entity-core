// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function TestCors() {
    var tokenJWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6Im1heWNvbkBnbWFpbC5jb20iLCJzdWIiOiI4YWEwNDMzZC04MDI3LTQzN2UtOGQ5Zi1kZTI1MGUzZTM0MTciLCJleHAiOjE2MTQ3MTk3NjJ9.XHXDO22U8ZWeHObqpci_7AaZIDQ0LgOxuXPtxtQuZ2k";
    var service = "https://localhost:44372/api/message/8aa0433d-8027-437e-8d9f-de250e3e3417/cf6ee7b7-c13d-49f8-9906-7f2eba6abdcc";

    $("#resultado").html("... carregando");

    $.ajax({
        url: service,
        method: "GET",
        crossDomain: true,
        headers: { "Accept": "application/json" },
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + tokenJWT);
        },
        success: function (data, status, xhr) {
            //$("#resultado").html(data);
            console.info(data);
        }
    });

}

