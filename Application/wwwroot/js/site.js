$(document).ready(function () {
    $('#contacts').empty();

});

$('#contact-types').on('change', function () {
    $('#contacts').append(GetContactHtml(this.value));
});

function GetContactHtml(type) {
    return '<li><span class="contact-type">' + type + '</span>:<input type="text" class="contact-value"></input></li>';
}