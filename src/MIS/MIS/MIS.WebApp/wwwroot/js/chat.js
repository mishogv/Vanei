let groupId = $('#group-id').val();

var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();

connection.on("AddToGroup",
    () => {
        connection.invoke("AddToGroup", groupId);
    });

connection.on("RemoveToGroup",
    () => {
        connection.invoke("RemoveToGroup", groupId);
    });

connection.on("NewMessage",
    function (message) {
        var chatInfo = `<div  class="w-75 bg-mis disabled border-0" rows="6" cols="100" style="resize: none;">[${message.username}]: ${message.text}</div >`;
        $("#messagesList").append(chatInfo);
    });

$("#sendButton").click(function () {
    var message = $("#messageInput").val();
    if (message.length > 0) {
        $("#messageInput").val('');
        connection.invoke("Send", groupId, message);
    }
});

connection.start().then(() => {
    connection.invoke('AddToGroup', groupId);
}).catch(function (err) {
    return console.error(err.toString());
});

$(document).ready(() => {
    $('#sendButton').click(function (event) {
        event.preventDefault();
    });
});

let onConnectionClose = () => {
    connection.invoke('RemoveFromGroup', groupId);
};

window.onbeforeunload = onConnectionClose;