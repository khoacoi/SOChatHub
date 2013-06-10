


function CheckValidationErrorResponse(response, form) {
    var data = getResponseValidationObject(response);
    if (!data) return;

    removePreviousValidationErrors();

    //Do not show ValidationSummary when having a modal dialog.
    if ($(".ui-widget-overlay").length == 0) {
        ValidationSummary(data.State);
    }

    $.each(data.State, function (i, item) {
        commonValidationErrors(item);

        if (i == 0) {
            $("[name='" + item.Name + "']").focus();
        }
    });
}

function commonValidationErrors(item) {

    var elementError = $('[name="' + item.Name + '"]');
    elementError.removeClass('valid');
    elementError.attr('data-val', 'true');
    elementError.addClass('input-validation-error');
    var spanError = $('span[data-valmsg-for="' + item.Name + '"]');
    spanError.removeClass('field-validation-valid').addClass('field-validation-error');
    //if (spanError.length > 0) {
    spanError.empty();
    spanError.append('<span class="" for="' + item.Name + '" generated="true">' + item.Errors[0] + '</span>');
    //}
    if (elementError.is("select.requiredField")) {
        var divTag = elementError.parent("div.required");
        var spanTag = elementError.siblings("span.required");
        divTag.addClass("custom-error");
        spanTag.addClass("custom-error");
    }
}

function removePreviousValidationErrors() {
    $('span[generated=true]').remove();
    $('.input-validation-error').each(function (index, item) {
        $(this).removeClass("input-validation-error");
        $(this).addClass("input-validation-valid");
    });

    $('.field-validation-error').each(function (index, item) {
        $(this).removeClass("field-validation-error");
        $(this).addClass("field-validation-valid");
    });
}

function getResponseValidationObject(response) {
    if (response && response.Tag && response.Tag == "ValidationError")
        return response;
    return null;
}

function ValidationSummary(data) {
    var errorSummaryEle = $('div.alert_error_container');

    $("#loadingContainer").hide();
    $(errorSummaryEle).empty();

    var hideModelError = (errorSummaryEle.attr('data-hideModelError') === 'true');
    var hasAnyErrors = false;

    var html = '<div class="validation-summary-errors" data-valmsg-summary="true">';
    html += '<ul>';
    $(data).each(function (index, item) {
        $(item).each(function (i, error) {
            if (!hideModelError || hideModelError && error.Name === '') {
                hasAnyErrors = true;
                var errors = error.Errors.join('<br/><span style="margin-left: 40px"></span>');
                html += '<li>' + errors + '</li>';
            }
        });
    });
    html += '</ul>';
    html += '</div>';
    if (hasAnyErrors) {
        $(errorSummaryEle).html(html);
        var alertErrContainer = $(".alert_error_container:first");
        if ($(alertErrContainer).length > 0) {
            $('html, body').animate({
                scrollTop: $(alertErrContainer).offset().top
            }, 1000);
        }
    }
}