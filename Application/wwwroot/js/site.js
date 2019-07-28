$(document).ready(function () {
    $('#contacts').empty();
});

$('#contact-types').on('change', function () {
    $('#contacts').append(GetContactHtml(this.value));
});

$('#submit-btn-new-person-form').on('click', function () {
    json = JSON.stringify(GetJsonNewContactToSubmit());
    //json = JSON.stringify({
    //    FirstName: 'Test',
    //    LastName: 'Test',
    //    MiddleName: 'Test'
    //});
    //$.ajax({
    //    url: '/Test',
    //    type: 'post',
    //    contentType: 'application/json',
    //    dataType: 'json',
    //    data: json,
    //    success: function (result) {
    //        log(result);
    //    },
    //    error: function (jqxhr, status, errorMsg) {
    //        log(errorMsg);
    //    }
    //});
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


function GetContactHtml(type) {
    return '<li><span class="contact-type">' + type + '</span>:<input type="text" class="contact-value"></input></li>';
}

function GetJsonNewContactToSubmit() {
    var result = serializeForm(); //$('#new-person-form').serialize();
    result.Contacts = serializeContacts();
    return result;
}

function serializeForm() {
    var unindexed_array = $('#new-person-form').serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

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