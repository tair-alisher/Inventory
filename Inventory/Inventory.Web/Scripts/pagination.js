function pagination() {
    $(document).ready(function () {
        $(document).on("click", "#contentPager a[href]", post_request);
        $("#equipment-select-list").on("change", post_request_without_page);
        $("#employee-select-list").on("change", post_request_without_page);
        $("#repairPlace-select-list").on("change", post_request_without_page);
        $("#statusType-select-list").on("change", post_request_without_page);
    });
}

function post_request_without_page() {
    var name = $("#name").val();
    var equipmentId = $("#equipment-select-list").val();
    var employeeId = $("#employee-select-list").val();
    var repairPlaceId = $("#repairPlace-select-list").val();
    var statusTypeId = $("#statusType-select-list").val();

    $("#loading").show();
    $.ajax({
        url: "Search/HistoryFilter",
        type: 'POST',
        data: {
            //"name": name,
            "equipmentId": equipmentId,
            "employeeId": employeeId,
            "repairPlaceId": repairPlaceId,
            "statusTypeId": statusTypeId
        },
        cache: false,
        success: function (result) {
            $("#loading").hide();
            $("#results").html(result);
            $("#accordion").hide();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#results").html("Ошибка. Обновите страницу");
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function post_request() {
    var name = $("#name").val();

    var equipmentId = $("#equipment-select-list").val();
    var employeeId = $("#employee-select-list").val();
    var repairPlaceId = $("#repairPlace-select-list").val();
    var statusTypeId = $("#statusType-select-list").val();

    var parts = $(this).attr("href").split("?");
    var url = parts[0];
    var page = parts[1].split("=")[1];

    $("#loading").show();
    $.ajax({
        url: url,
        type: 'POST',
        data: {
            //"name": name,
            "page": page,
            "equipmentId": equipmentId,
            "employeeId": employeeId,
            "repairPlaceId": repairPlaceId,
            "statusTypeId": statusTypeId
        },
        cache: false,
        success: function (result) {
            $("#loading").hide();
            $("#results").html(result);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#results").html("Ошибка. Обновите страницу");
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function check_input(text) {
    $("#ajax-form").submit(function () {
        var input = $("#xyz-search-input").val();
        if (input == "") {
            alert(text);
            return false;
        }
    })
}