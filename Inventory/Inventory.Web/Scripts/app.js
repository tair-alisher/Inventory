function searchEmployees() {
    var employeeName = document.getElementById('search-input-value').value;
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: "/Equipments/FindEmployee",
        type: "Post",
        data: {
            __RequestVerificationToken: token,
            "name": employeeName
        },
        success: function (data) {

        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}