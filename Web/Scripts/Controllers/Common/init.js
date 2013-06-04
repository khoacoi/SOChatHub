$(document).ready(function () {
    ReplaceValidationSummary();
});


function ReplaceValidationSummary() {
    $('.alert_error_container').attr('data-hideModelError', $('#validationSummary').attr('data-hideModelError'));
    $('.alert_error_container').appendTo($('#validationSummary'));
}