﻿const TYPE = "Тип";
const NUMBER = "Инвентаризационный номер";
const MODEL = "Модель";
const NAME = "Название";

function menuInit() {
    $(document).ready(function () {
        $("#sidebar").mCustomScrollbar({
            theme: "minimal"
        });

        $('#sidebarCollapse').on('click', function () {
            $('#sidebar, #content, .navbar').toggleClass('active');
            $('.collapse.in').toggleClass('in');
            $('a[aria-expanded=true]').attr('aria-expanded', 'false');
        });
    });
}

function searchEmployeesByEnter() {
    $(document).ready(function () {
        $("#search-input-value").keyup(function (event) {
            if (event.keyCode == 13) {
                searchEmployees();
            }
        });
    });
}

function activeMenuItem() {
    turnOffCurrentActiveMenuItem();

    var url = window.location.href.toLowerCase();
    if (url.indexOf('user') >= 0) {
        $('#user-page-menu-item').addClass('active');
    }
    else if (url.indexOf('equipment') >= 0) {
        $('#equipment-page-menu-item').addClass('active');
    }
    else if (url.indexOf('component') >= 0) {
        $('#component-page-menu-item').addClass('active');
    }
    else if (url.indexOf('history') >= 0 || url.indexOf('repairplace') >= 0 || url.indexOf('statustype') >= 0) {
        $('#tracking-page-menu-item').addClass('active');
    }
    else if (url.indexOf('login') >= 0) {
        $('#login-page-menu-item').addClass('active');
    }
    else if (url.indexOf('changeemail') >= 0) {
        $('#change-email-page-menu-item').addClass('active');
    }
    else if (url.indexOf('changepassword') >= 0) {
        $('#change-password-page-menu-item').addClass('active');
    }
    else {
        $('#main-page-menu-item').addClass('active');
    }
}

function turnOffCurrentActiveMenuItem() {
    var menuItems = [];
    menuItems.push($('#main-page-menu-item'));
    menuItems.push($('#user-page-menu-item'));
    menuItems.push($('#equipment-page-menu-item'));
    menuItems.push($('#component-page-menu-item'));
    menuItems.push($('#tracking-page-menu-item'));
    menuItems.push($('#login-page-menu-items'));
    menuItems.push($('#change-email-page-menu-item'));
    menuItems.push($('#change-password-page-menu-item'));

    for (var i = 0; i < menuItems.length; i++) {
        if (menuItems[i].hasClass('active') >= 0) {
            menuItems[i].removeClass('active');
        }
    }
}

function searchEmployees() {
    var employeeName = $("#search-input-value").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var oldSearchingText = document.getElementById('searching').innerText;

    document.getElementById('searching').innerText = "Идет поиск...";

    $.ajax({
        url: "/Equipment/FindEmployees",
        type: "Post",
        data: {
            __RequestVerificationToken: token,
            "name": employeeName
        },
        success: function (html) {
            $("#found-items-area").empty();
            $("#found-items-area").append(html);
            document.getElementById('searching').innerText = oldSearchingText;
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
            document.getElementById('searching').innerText = oldSearchingText;
        }
    });
    return false;
}

function clearSearch() {
    $("#found-items-area").empty();
    $("#search-input-value").val('');
}

function attachEmployee(employeeId) {
    if (document.body.contains(document.getElementById("pinned-" + employeeId))) {
        alert("Сотрудник уже прикриплен.");
        return false;
    }

    var employeeRow = $("#" + employeeId);
    var name = employeeRow.find("td.name")[0].innerText;

    var newTr = document.createElement("tr");
    newTr.id = "pinned-" + employeeId;

    var inputId = document.createElement("input");
    inputId.type = "hidden";
    inputId.name = "employeeId[]";
    inputId.value = employeeId;

    var button = document.createElement("button");
    button.classList.add("btn", "btn-danger");
    button.type = "button";
    button.innerText = "Убрать";
    button.setAttribute("onclick", "detachItem('" + employeeId + "')");

    var buttonTd = document.createElement("td");
    buttonTd.className = "input-group-btn";
    buttonTd.appendChild(inputId);
    buttonTd.appendChild(button);

    var nameTd = createTd("name", name);

    var label = document.createElement("label");

    var inputOwner = document.createElement("input");
    inputOwner.type = "radio";
    inputOwner.name = "ownerId";
    inputOwner.value = employeeId;

    label.appendChild(inputOwner);
    label.appendChild(document.createTextNode("Текущий владелец"));

    var ownerTd = document.createElement("td");
    ownerTd.appendChild(label);

    var emptyTd = document.createElement("td");

    newTr.appendChild(buttonTd);
    newTr.appendChild(nameTd);
    newTr.appendChild(ownerTd);
    newTr.appendChild(emptyTd);

    var attachedItems = document.getElementById("attached-items-tbody");
    attachedItems.appendChild(newTr);
}

function createTd(tdClass, value) {
    var td = document.createElement("td");
    td.className = tdClass;
    td.innerText = value;

    return td;
}

function searchComponents(type) {
    var searchValue = $("#search-input-value").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var oldSearchingText = document.getElementById('searching').innerText;

    document.getElementById('searching').innerText = "Идет поиск...";


    $.ajax({
        url: "/Component/FindComponents",
        type: "Post",
        data: {
            __RequestVerificationToken: token,
            "value": searchValue,
            "type": type
        },
        success: function (html) {
            $("#found-items-area").empty();
            $("#found-items-area").append(html);
            document.getElementById('searching').innerText = oldSearchingText;
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
            document.getElementById('searching').innerText = oldSearchingText;
        }
    });
    return false;
}

function attachComponent(componentId) {
    if (document.body.contains(document.getElementById("pinned-" + componentId))) {
        alert("Комплектующие уже в списке.");
        return false;
    }

    var componentInfo = $("#" + componentId);
    var type = componentInfo.find("td.type")[0].innerText;
    var model = componentInfo.find("td.model")[0].innerText;
    var name = componentInfo.find("td.name")[0].innerText;
    var number = componentInfo.find("td.number")[0].innerText;

    var inputId = document.createElement("input");
    inputId.type = "hidden";
    inputId.name = "componentId[]";
    inputId.value = componentId;

    var typeDiv = createDiv("type", TYPE, type);
    var modelDiv = createDiv("model", MODEL, model);
    var nameDiv = createDiv("name", NAME, name);
    var numberDiv = createDiv("number", NUMBER, number);

    var buttonDiv = createButtonDiv(componentId);

    var wrapDiv = document.createElement("div");
    wrapDiv.classList.add("col-md-8", "item");

    var newComponent = document.createElement("div");
    newComponent.className = "row";
    newComponent.id = "pinned-" + componentId;

    wrapDiv.appendChild(inputId);
    wrapDiv.appendChild(typeDiv);
    wrapDiv.appendChild(modelDiv);
    wrapDiv.appendChild(nameDiv);
    wrapDiv.appendChild(numberDiv);
    wrapDiv.appendChild(createElement("br"));
    wrapDiv.appendChild(buttonDiv);
    newComponent.appendChild(wrapDiv);

    var attachedItems = document.getElementById("attached-items");
    attachedItems.appendChild(newComponent);
}

function createElement(element) {
    return document.createElement(element);
}

function createDiv(divClass, title, value) {
    var wrapDiv = document.createElement("div");
    wrapDiv.classList.add(divClass, "row", "item-info-row");

    var firstDiv = document.createElement("div");
    firstDiv.classList.add("col-md-6", "item-info");

    var bold = document.createElement("b");
    bold.innerText = title;

    firstDiv.appendChild(bold);

    var secondDiv = document.createElement("div");
    secondDiv.classList.add("col-md-6", "item-info");
    secondDiv.innerText = value;

    wrapDiv.appendChild(firstDiv);
    wrapDiv.appendChild(secondDiv);

    return wrapDiv;
}

function createButtonDiv(componentId) {
    var removeBtn = createRemoveButton(componentId);
    var detailsBtn = createDetailsButton(componentId);

    var buttonDiv = document.createElement("div");
    buttonDiv.className = "btn-group";
    buttonDiv.classList.add("btn-group", "float-right");

    buttonDiv.appendChild(removeBtn);
    buttonDiv.appendChild(detailsBtn);

    return buttonDiv;
}

function createRemoveButton(componentId) {
    var button = document.createElement("button");
    button.classList.add("btn", "btn-danger");
    button.type = "button";
    button.setAttribute("onclick", "detachItem('" + componentId + "')");
    button.innerText = "Убрать";

    return button;
}

function createDetailsButton(componentId) {
    var button = document.createElement("a");
    button.setAttribute("href", "/Component/Details?id=" + componentId);
    button.classList.add("btn", "btn-primary");
    button.setAttribute("target", "_blank");
    button.innerText = "Подробности";

    return button;
}

function detachItem(id) {
    var toRemove = $("#pinned-" + id);
    toRemove.remove();
}

function modalRemovalWindow(url) {
    $(document).ready(function () {
        var firstPaginationPage = $('.pagination li:nth-child(2)>a');
        var elementId;
        $('.delete-prompt').on('click',function () {
            elementId = $(this).attr('id');
            $('#myModal').modal('show');
        });

        $('.delete-confirm').on('click', function () {
            var token = $('input[name="__RequestVerificationToken"]').val();
            if (elementId != '') {
                $.ajax({
                    url: url,
                    type: 'Post',
                    data: {
                        __RequestVerificationToken: token,
                        'id': elementId
                    },
                    success: function (data) {
                        if (data == 'Удаление невозможно.') {
                            $('.delete-confirm').css('display', 'none');
                            $('.delete-cancel').html('Закрыть');
                            $('.success-message').html('Удаление невозможно, у записи есть связи!');
                        }
                        else if (data) {
                            $("#" + elementId).remove();
                            $('#myModal').modal('hide');
                            //$.notify("Запись удалена успешно!", "success");
                            location.reload();
                        }
                    }, error: function (err) {
                        if (!$('.modal-header').hasClass('alert-danger')) {
                            $('.modal-header').removeClass('alert-success').addClass('alert-danger');
                            $('.delete-confirm').css('display', 'none');
                        }
                        $('.success-message').html(err.statusText);
                    }
                });
            }
        });
        //function to reset bootstrap modal popups
        $("#myModal").on("hidden.bs.modal", function () {
            $('.delete-confirm').css('display', 'inline-block');
            $('.delete-cancel').html('Нет');
            $('.success-message').html('').html('Вы действительно хотите удалить запись?');
        });
    });
}

function hideAccordion() {
    $("#accordion").hide();
}

function removeListAndPagination() {
    $("#listTable").remove();
    $("#paginationToDelete").remove();
}


function toPrevMain(from = "") {
    if (from == "list") {
        $("#employee-list").empty();
    } else if (from == "admin") {
        $("#name").val("");
        $("#position-select-list").val("");
        $("#department-select-list").val("");
        $("#administration-select-list").val("");
        $("#division-select-list").val("");
        $("#results").empty();
    } else {
        $("#results").empty();
    }
    $("#accordion").show();
}
function closeMessageDiv() {
    $("#message-div").remove();
}

function QRCodeSourcetoPrint(source) {
    return "<html><head><script>function step1(){\n" +
        "setTimeout('step2()', 10);}\n" +
        "function step2(){window.print();window.close()}\n" +
        "</scri" + "pt></head><body onload='step1()'>\n" +
        "<img src='" + source + "' /></body></html>";
}

function QRCodePrint(source) {
    Pagelink = "about:blank";
    var pwa = window.open(Pagelink, "_new");
    pwa.document.open();
    pwa.document.write(QRCodeSourcetoPrint(source));
    pwa.document.close();
}