function RegisterUserViewModelFuntion(options) {
    self = RegisterUserViewModel;

    self.save = function (form) {
        //var canSubmit = $(form).valid();
        //if (canSubmit) {
        //    $(form).submit(function (e) {
        //        e.preventDefault();
        //    });
        //if(self.Password() == self.ConfirmPassword())
        //{
            $.ajax({
                url: options.RegisterNewUserUrl,
                type: 'POST',
                data: ko.mapping.toJSON(self),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.IsSuccess) {
                        //location.href = data.RedirectUrl;
                        location.href = options.RedirectUrl;
                    }
                },
                error: function (error) {
                    alert("There was an error posting the data to the server: " + error.responseText);
                }
            });
        //}
    }

    ko.applyBindings(self);
}