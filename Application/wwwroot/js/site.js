$(document).ready(function () {
    $('#contacts').empty();
});

// добавить новую строку контактных данных
$('#contact-types').on('change', function () {
    $('#contacts').append(getContactHtml(this.value));
});

// отправить пост запрос на создание записи
$('#submit-btn-new-person-form').on('click', function () {
    json = JSON.stringify(GetNewPersonInfoToSubmit());
    $.ajax({
        url: '/Create',
        type: 'post',
        contentType: 'application/json',
        dataType: 'json',
        data: json,
        error: function (jqxhr, status, errorMsg) {
            alert(errorMsg);
        },
        statusCode: {
            200: function () {
                alert("Успех");
            }
        }
    });
});

// получить верстку для элемента листа контактных данных
function getContactHtml(type) {
    return '<li><span class="contact-type">' + type + '</span>:<input type="text" class="contact-value"></input></li>';
}

// собрать всю инфу для запроса создания новой записи
function GetNewPersonInfoToSubmit() {
    var result = serializeForm();
    result.Contacts = serializeContacts();
    return result;
}

// создать объект из данных формы создания
function serializeForm() {
    var unindexed_array = $('#new-person-form').serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

// создать массив контактные данные 
function serializeContacts() {
    result = [];
    listItems = $('#contacts li').get();
    listItems.forEach(function (item, i, arr) {
        type = item.firstChild.textContent;
        value = item.lastChild.value;
        struct = {
            Type: type,
            Value: value
        };
        result.push(struct);
    });
    return result;
}