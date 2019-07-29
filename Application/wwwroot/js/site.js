$(document).ready(function () {
    $('#contacts').empty();
    $(window).resize(setPopupContainerPosition);
    $(window).scroll(setPopupContainerPosition);
    setPopupContainerPosition();    
});

$('#open-new-person-block-link').on('click', function (e) {
    e.preventDefault();
    e.stopPropagation();
    scrollPos = $(window).scrollTop();
    $('#new-person-block').show();
    $('#black-background').show();
    $('html,body').css('overflow', 'hidden');
    $('html').scrollTop(scrollPos);
});

$('#black-background').click(function () {
    closePopup();
});

$('#close-popup-btn').click(function () {
    closePopup();
});

// добавить новую строку контактных данных
$('#contact-types').on('change', function (e) {
    e.preventDefault();
    e.stopPropagation();
    $('#contacts').append(getContactHtml(this.value));
});

// отправить пост запрос на создание записи
$('#submit-btn-new-person-form').on('click', function (e) {
    e.preventDefault();
    e.stopPropagation();
    json = JSON.stringify(GetNewPersonInfoToSubmit());
    $.ajax({
        url: '/Create',
        type: 'post',
        contentType: 'application/json',
        dataType: 'json',
        data: json,
        success: function (result){
            if (result.redirectUrl) {
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