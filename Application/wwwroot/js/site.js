$(document).ready(function () {
    $(window).resize(setPopupContainerPosition);
    $(window).scroll(setPopupContainerPosition);
    setPopupContainerPosition();    
});

$('#open-new-person-block-link').on('click', function (e) {
    e.preventDefault();
    e.stopPropagation();
    clearPopup();
    showPopup();
});

$('#black-background').click(function () {
    closePopup();
});

$('#close-popup-btn').click(function () {
    closePopup();
});

$('.open-edit-person-form-link').click(function () {
    Id = $(this).attr('value');
    object = {
        Id: Id
    };
    $.ajax({
        url: '/get-person',
        type: 'get',
        contentType: 'application/json',
        dataType: 'json',
        data: object,
        success: function (result) {
            if (result.obj) {
                clearPopup();
                showPopup();
                $('#first-name').val(result.obj.firstName);
                $('#last-name').val(result.obj.lastName);
                $('#middle-name').val(result.obj.middleName);
                $('#date-of-birth').val(getHtml5StringDate(result.obj.dateOfBirth));
                $('#org-name').val(result.obj.organizationName);
                $('#pos-name').val(result.obj.positionName);
                result.obj.contacts.forEach(function (item, i, arr) {
                    $('#contacts').append(getContactHtml(item.type, item.value));
                });
                submitBtn = $('#submit-btn-new-person-form');
                submitBtn.html('Изменить контакт');
                submitBtn.off();
                submitBtn.on('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    object = getNewPersonInfoToSubmit();
                    object.Id = Id;
                    json = JSON.stringify(object);
                    $.ajax({
                        url: '/update-person',
                        type: 'patch',
                        contentType: 'application/json',
                        dataType: 'json',
                        data: json,
                        success: function (result) {
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
    sendCreatePersonRequest();
});

function sendCreatePersonRequest() {
    json = JSON.stringify(getNewPersonInfoToSubmit());
    $.ajax({
        url: '/create',
        type: 'post',
        contentType: 'application/json',
        dataType: 'json',
        data: json,
        success: function (result) {
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
}

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
    visibleType = '';

    switch (type) {
        case 'Telephone':
            visibleType = 'Телефон';
            break;
        case 'Skype':
            visibleType = type;
            break;
        case 'Email':
            visibleType = type;
            break;
        case 'Other':
            visibleType = "Другое";
            break;
        default:
            visibleType = "Другое";
            break;
    }

    return '<li><span class="contact-type" value="' + type + '">' + visibleType
        + '</span>:<input type="text" class="contact-value" value="'+ value +'"></input><button onclick="removeContactInfoRow(this)">X</button></li>';
}

// удалить строку контактных данных из DOM
function removeContactInfoRow(e) {
    $(e).parent().remove();
}

// удалить строку контактных данных из Базы
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
function getNewPersonInfoToSubmit() {
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

// создать массив контактных данных
function serializeContacts() {
    result = [];
    listItems = $('#contacts li').get();
    listItems.forEach(function (item, i, arr) {
        type = $(item).find('.contact-type').attr('value');
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

// показать попап
function showPopup() {
    scrollPos = $(window).scrollTop();
    $('#new-person-block').show();
    $('#black-background').show();
    $('html,body').css('overflow', 'hidden');
    $('html').scrollTop(scrollPos);
}

// вернуть попап в исходное состояние
function clearPopup() {
    $('#first-name').val('');
    $('#last-name').val('');
    $('#middle-name').val('');
    $('#date-of-birth').val('');
    $('#org-name').val('');
    $('#pos-name').val('');
    $('#contacts').empty();
    submitBtn = $('#submit-btn-new-person-form');
    submitBtn.off();
    submitBtn.html('Создать контакт');
    submitBtn.on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        sendCreatePersonRequest();
    });
} 

// костыль, чтобы инициализировать дейтпикер на вьюшке
function getHtml5StringDate(commonDateStr) {
    date = new Date(commonDateStr);
    day = ("0" + date.getDate()).slice(-2);
    month = ("0" + (date.getMonth() + 1)).slice(-2);
    result = date.getFullYear() + "-" + month + "-" + day;
    return result;
}