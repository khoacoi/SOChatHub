//var hasRequestOutcoming = false;
$(document).ready(function () {
    ReplaceValidationSummary();
    RegisterGlobalAjaxHandlers();
});


function ReplaceValidationSummary() {
    $('.alert_error_container').attr('data-hideModelError', $('#validationSummary').attr('data-hideModelError'));
    $('.alert_error_container').appendTo($('#validationSummary'));
}


function RegisterGlobalAjaxHandlers() {
    var form = $('form');
    $.ajaxPrefilter('json', function (options, originalOptions, xhr) {
        //if (hasRequestOutcoming && options.url.indexOf("AutoComplete") < 0)
        //    xhr.abort();

        //if (options.url.indexOf("AutoComplete") < 0)
        //    hasRequestOutcoming = true;
        //else
        //    hasRequestOutcoming = false;

        if (options.type.toLowerCase() === "post") {
            //Override succes function.
            var originalSuccess = options.success;
            options.success = function (data) {
                //hasRequestOutcoming = false;
                if (data != null && data.IsSuccess === false) {
                    if (options.validationFailed && typeof (options.validationFailed) === "function")
                        options.validationFailed(data.State);
                    else
                        CheckValidationErrorResponse(data, form);
                }

                if (originalSuccess && typeof (originalSuccess) === "function") {
                    originalSuccess(data);
                }

            };
        }
    });

    $(document).ajaxError(function (event, jqxhr, settings, exception) {
        if (exception != null && exception != "") {
            ShowDialog('Error', SERVER_EXCEPTION, 'error');
        }
        //hasRequestOutcoming = false;
    });
}