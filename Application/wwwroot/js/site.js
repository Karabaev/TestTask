$(document).ready(function () {
    $('#contacts').empty();
    $(window).resize(setPopupContainerPosition);
    $(window).scroll(setPopupContainerPosition);
    setPopupContainerPosition();    
});

$('#open-new-person-block-link').on('click', function (e) {
    e.preventDefault();
    e.stopPropagation();
    showPopup();
});

$('#black-background').click(function () {
    closePopup();
});

$('#close-popup-btn').click(function () {
    closePopup();
});

$('.open-edit-person-form-link').click(function () {
    object = {
        Id: $(this).attr('value')
    };
    $.ajax({
        url: '/get-person',
        type: 'get',
        contentType: 'application/json',
        dataType: 'json',
        data: object,
        success: function (result) {
            if (result.obj) {
                popupWnd = $('new-person-block');
                popupWnd.find('#first-name').val(result.FirstName);
                popupWnd.find('#last-name').val(result.LastName);
                popupWnd.find('#middle-name').val(result.MiddleName);
                popupWnd.find('#date-of-birth').val(result.DateOfBirth);
                popupWnd.find('#org-name').val(result.OrganizationName);
                popupWnd.find('#pos-name').val(result.PositionName);

                for (i in result.Contacts) {
                    popupWnd.find('#contacts').append(getContactHtml(result.Contacts[i].Type, result.Contacts[i].Value));
                }

                showPopup();
            }
            else if (result.error) {
                alert(result.error);
            }
        },
        error: function (jqxhr, status, errorMsg) {
            console.error(status + " | " + errorMsg + " | " + jqxhr);
        }
    });
});

//function openEditForm(personGuid) {
//    json = JSON.stringify({
//        Id: personGuid
//    });
//    $.ajax({
//        url: '/get-person',
//        type: 'patch',
//        contentType: 'application/json',
//        dataType: 'json',
//        data: json,
//        success: function (result) {
//            if (result) {
//                popupWnd.find('#first-name').val(result.FirstName);
//                popupWnd.find('#last-name').val(result.LastName);
//                popupWnd.find('#middle-name').val(result.MiddleName);
//                popupWnd.find('#date-of-birth').val(result.DateOfBirth);
//                popupWnd.find('#org-name').val(result.OrganizationName);
//                popupWnd.find('#pos-name').val(result.PositionName);
                
//                for (i in result.Contacts) {
//                    popupWnd.find('#contacts').append(getContactHtml(result.Contacts[i].Type, result.Contacts[i].Value));
//                }

//            }
//            else if (result.error) {
//                alert(result.error);
//            }
//        },
//        error: function (jqxhr, status, errorMsg) {
//            console.error(status + " | " + errorMsg + " | " + jqxhr);
//        }
//    });
//    showPopup();
//}

// добавить новую строку контактных данных
$('#contact-types').on('change', function (e) {
    e.preventDefault();
    e.stopPropagation();
    $('#contacts').append(getContactHtml(this.value, ''));
});

// отправить пост запрос на создание записи
$('#submit-btn-new-person-form').on('click', function (e) {
    e.preventDefault();
    e.stopPropagation();
    json = JSON.stringify(GetNewPersonInfoToSubmit());
    $.ajax({
        url: '/create',
        type: 'post',
        contentType: 'application/json',
        dataType: 'json',
        data: json,
        success: function (result){
            if (result.redirectUrl) {
                window.location.href = result.redirectUrl;
            }
            else if (result.error) {
                alert(result.error);
            }
        },
        error: function (jqxhr, status, errorMsg) {
            console.error(status + " | " + errorMsg + " | " + jqxhr);
        }
    });
});

$('.remove-person-link').on('click', function (e) {
    e.preventDefault();
    e.stopPropagation();
    json = JSON.stringify({
        id: $(this).attr('value')
    });

    $.ajax({
        url: '/Remove',
        type: 'delete',
        contentType: 'application/json',
        dataType: 'json',
        data: json,
        success: function (result) {
            if (result.redirectUrl) {
                alert("Запись удалена успешно");
                window.location.href = result.redirectUrl;
            }
            else if (result.error) {
                alert(error);
            }
        },
        error: function (jqxhr, status, errorMsg) {
            console.error(status + " | " + errorMsg + " | " + jqxhr);
        }
    });
});

// получить верстку для элемента листа контактных данных
function getContactHtml(type, value) {
    return '<li><span class="contact-type">' + type
        + '</span>:<input type="text" class="contact-value" value="'+ value +'"></input><button onclick="removeContactInfoRow(this)">X</button></li>';
}

// удалить строку контактных данных из DOM
function removeContactInfoRow(e) {
    $(e).parent().remove();
}

function removeContactInfoFromBase(id) {
    json = JSON.stringify({
        Id: id
    });

    $.ajax({
        url: '/remove-contact-info',
        type: 'delete',
        contentType: 'application/json',
        dataType: 'json',
        data: json,
        success: function (result) {
            if (result.error) {
                alert(result.error);
                location.reload();
            }
        },
        error: function (jqxhr, status, errorMsg) {
            console.error(status + " | " + errorMsg + " | " + jqxhr);
        }
    });
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

        type = $(item).find('.contact-type').text();
        value = $(item).find('.contact-value').val();

        struct = {
            Type: type,
            Value: value
        };
        result.push(struct);
    });
    return result;
}

// поместить попап в центре
function setPopupContainerPosition() {
    containerWidth = 400;
    winWidth = $(window).width();
    winHeight = $(document).height();
    scrollPos = $(window).scrollTop();
    disWidth = (winWidth - containerWidth) / 2;
    disHeight = scrollPos + 150;
    $('.popup-container').css({ 'width': containerWidth + 'px', 'left': disWidth + 'px', 'top': disHeight + 'px' });
    $('#black-background').css({ 'width': winWidth + 'px', 'height': winHeight + 'px' });
}

// закрыть попап
function closePopup() {
    scrollPos = $(window).scrollTop();
    $('#new-person-block').hide();
    $('#black-background').hide();
    $("html,body").css("overflow", "auto");
    $('html').scrollTop(scrollPos);
}

function showPopup() {
    scrollPos = $(window).scrollTop();
    $('#new-person-block').show();
    $('#black-background').show();
    $('html,body').css('overflow', 'hidden');
    $('html').scrollTop(scrollPos);
}