function ChatViewModelFuntion(options) {
    self = ChatViewModel;
    self.chatMessage = ko.observable();

    ko.applyBindings(self);
    
    // SignalR Chat Hub
    //SignalR script to update the chat page and send messages.
    $(function () {
        var chat = $.connection.chatHub;

        chat.client.addNewMessageToPage = function (name, message) {
            // Add the message to the page. 
            $('#discussion').append('<li><strong>' + htmlEncode(name)
                + '</strong>: ' + htmlEncode(message) + '</li>');
        };
        

        $.connection.hub.start().done(function () {
            $('#sendmessage').click(function () {
                if (self.chatMessage() != "") {
                    // Call the Send method on the hub. 
                    chat.server.send(self.chatMessage());
                    // Clear text box and reset focus for next comment. 
                    self.chatMessage("");
                }
                $('#message').focus();
            });
        });
    });

    function htmlEncode(value) {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;
    }
    // End ChatScript


    // Set initial focus to message input box. 
    $('#message').focus();


}