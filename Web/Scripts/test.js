$(document).ready(function () {
    RegisterGlobalAjaxHandlers();
});

function RegisterGlobalAjaxHandlers() {
    $.ajaxPrefilter('json', function (options, originalOptions, xhr) {
        options.success = function (data) {
            if (data != null && data.IsSuccess === false && data.Tag == "ValidationError") {
                    CheckValidationErrorResponse(data);
            }
        };
    });
}

function CheckValidationErrorResponse(data) {
    $("#validationSummary").empty();

    $.each(data.State, function (i, item) {
        var errorString = item.Name + ": <br/>";
        $(item.Errors).each(function (index, error) {
            errorString += "<div>" + error + "</div>";
            $("#validationSummary").append(errorString);
        });
        
    
    });

}