// Expanding the height of the tag Textarea.
var textarea = document.querySelector('textarea');

textarea.addEventListener('keydown', autosize);

function autosize() {
    var el = this;
    setTimeout(function () {
        el.style.cssText = 'height:auto; padding:0';
        el.style.cssText = 'height:' + el.scrollHeight + 'px';
    }, 0);
}

// Remove attribute disabled in the input tag.

function checkParams() {
    var response = $('.page_area_text').val();

    if (response.length != 0) {
        $('.page_area_submit').removeAttr('disabled');
    } else {
        $('.page_area_submit').attr('disabled', 'disabled');
        $('.error_submit').text('*Fill in the response');
        
    }
}