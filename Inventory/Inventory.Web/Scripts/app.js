﻿function searchEmployees() {
    var employeeName = $("#search-input-value").val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: "/Equipment/FindEmployees",
        type: "Post",
        data: {
            __RequestVerificationToken: token,
            "name": employeeName
        },
        success: function (html) {
            $("#found-employees-area").append(html);
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function clearSearch() {
    $("#found-employees-area").empty();
    $("#search-input-value").val('');
}

function attachEmployee(employeeId) {
    var employeeRow = $("#" + employeeId);
    var name = employeeRow.find("td.name")[0].innerText;
    var room = employeeRow.find("td.room")[0].innerText;
    var position = employeeRow.find("td.position")[0].innerText;
    var department = employeeRow.find("td.department")[0].innerText;
    var administration = employeeRow.find("td.administration")[0].innerText;

    var newTr = document.createElement("tr");
    newTr.id = "pinned-" + employeeId;

    var button = document.createElement("button");
    button.classList.add("btn", "btn-danger");
    button.type = "button";
    button.innerText = "Убрать";
    button.dataset.id = employeeId;
    button.setAttribute("onclick", "detachEmployee(" + employeeId + ")");

    var inputId = document.createElement("input");
    inputId.type = "hidden";
    inputId.name = "employeeId[]";
    inputId.value = employeeId;

    var nameTd = createTd("name", name);
    var roomTd = createTd("room", room);
    var positionTd = createTd("position", position);
    var departmentTd = createTd("department", department);
    var administrationTd = createTd("administration", administration);

    var buttonTd = document.createElement("td");
    buttonTd.appendChild(inputId);
    buttonTd.appendChild(button);

    newTr.appendChild(buttonTd);
    newTr.appendChild(nameTd);
    newTr.appendChild(roomTd);
    newTr.appendChild(positionTd);
    newTr.appendChild(departmentTd);
    newTr.appendChild(administrationTd);

    var attachedEmployees = document.getElementById("attached-employees-tbody");
    attachedEmployees.appendChild(newTr);
}

function detachEmployee(employeeId) {
    var rowToRemove = $("#pinned-" + employeeId);
    rowToRemove.remove();
}


function createTd(tdClass, value) {
    var td = document.createElement("td");
    td.className = tdClass;
    td.innerText = value;

    return td;
}