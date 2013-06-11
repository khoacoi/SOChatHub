function ChatViewModelFuntion(options) {
    self = ChatViewModel;
    self.chatMessage = ko.observable();
    var chat = $.connection.chatHub;

    self.chatMessageKeyDown = function (viewModel, event) {
        
        if (event.keyCode == 13) {
            self.sendMessage();
        }
        return true;

    }
    self.sendMessage = function () {
        if (self.chatMessage() != "") {
            // Call the Send method on the hub. 
            chat.server.sendToAll(self.chatMessage());
            autoScrollToBottom();
            // Clear text box and reset focus for next comment. 
            self.chatMessage("");
        }
        $('#message').focus();
    }

    chat.client.addNewMessageToPage = function (name, message) {
        // Add the message to the page. 
        $('#discussion').append('<li><strong>' + htmlEncode(name)
            + '</strong>: ' + messageHTMLEncode(message) + '</li>');
    };

    $.connection.hub.start().done(function () {
        self.sendMessage();
    });

    
    //scroll bar
    var scrollPanel = $('#discussionpanel');
    function autoScrollToBottom() {
        if (scrollPanel.get(0).scrollHeight > scrollPanel.height()) {
            if (scrollPanel.outerHeight() == (scrollPanel.get(0).scrollHeight - scrollPanel.scrollTop())) {
                scrollPanel.scrollTop(scrollPanel.prop("scrollHeight"));
            }
        }
    }
    

    function htmlEncode(value) {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;
    }

    function messageHTMLEncode(value) {
        return value;
    }
    // End ChatScript

    ko.applyBindings(self);

    // Set initial focus to message input box. 
    $('#message').focus();


}